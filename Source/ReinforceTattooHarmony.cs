using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using Verse;

namespace ReinforceTattooMOD
{
	[StaticConstructorOnStartup]
	public static class ReinforceTattooHarmony
	{
		static ReinforceTattooHarmony()
		{
			Harmony harmony = new Harmony("Flestal.ReinforceTattooMOD");

			harmony.PatchAll();
		}
	}

	[HarmonyPatch(typeof(PawnCapacityUtility), "CalculatePartEfficiency")]
	public static class CalculatePartEfficiency_Patch
	{
		public static void Postfix(HediffSet diffSet, BodyPartRecord part, ref float __result)
		{
			if (part == null || __result <= 0f)
			{
				return;
			}
			float sum = 0;
			foreach (Hediff hediff in diffSet.hediffs)
			{
				if (hediff.Part != part)
				{
					continue;
				}
				if(hediff.def== ReinforceTattooDefOf.ReinforceTattoo)
				{
					HediffComp_ReinforceTattoo comp = hediff.TryGetComp<HediffComp_ReinforceTattoo>();
					sum += comp!=null?comp.GetBonusForQuality():0;
				}
			}
			__result += sum;
		}
	}

	[HarmonyPatch(typeof(HealthCardUtility),"GenerateSurgeryOption")]
	public static class GenerateSurgeryOption_Patch
	{
		public static void Postfix(ref FloatMenuOption __result, Pawn pawn, RecipeDef recipe, BodyPartRecord part)
		{
			// 우리 수술 레시피가 아닐 경우 아무것도 하지 않음
			if (recipe.defName != "Install_ReinforceTattoo")
			{
				return;
			}
			Action newAction = delegate
			{
				// 부위 선택 다이얼로그를 띄웁니다.
				// 사용자가 부위를 선택하면 실행될 콜백을 람다식으로 전달합니다.
				Find.WindowStack.Add(new Dialog_InstallReinforceTattoo(pawn, (BodyPartRecord selectedPart) =>
				{
					// ★★★★★ 핵심 ★★★★★
					// public static 메서드이므로 직접 호출합니다.
					// 파라미터: (환자, 레시피, 선택된 부위)
					HealthCardUtility.CreateSurgeryBill(pawn, recipe, selectedPart);
				}));
			};

			// 기존 FloatMenuOption의 액션을 우리가 만든 새 액션으로 교체
			__result.action = newAction;
		}
	}
}
