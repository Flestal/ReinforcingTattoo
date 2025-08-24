using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace ReinforceTattooMOD
{
    public class Recipe_InstallReinforceTattoo : RecipeWorker
    {
		// 의약품 정책별로 선택 가능한 부위 DefName을 미리 정의합니다.
		//private static readonly List<BodyPartDef> HerbalParts = new List<BodyPartDef>
		//{
		//    BodyPartDefOf.Arm, BodyPartDefOf.Leg, BodyPartDefOf.Hand, ReinforceTattooDefOf.Foot
		//};

		//private static readonly List<BodyPartDef> NormalParts = new List<BodyPartDef>
		//{
		//    BodyPartDefOf.Eye, ReinforceTattooDefOf.Ear, ReinforceTattooDefOf.Nose, ReinforceTattooDefOf.Jaw, ReinforceTattooDefOf.Tongue
		//};

		//private static readonly List<BodyPartDef> BestParts = new List<BodyPartDef>
		//{
		//	ReinforceTattooDefOf.Stomach, ReinforceTattooDefOf.Liver, BodyPartDefOf.Lung, ReinforceTattooDefOf.Kidney,
		//	BodyPartDefOf.Heart, ReinforceTattooDefOf.Brain, ReinforceTattooDefOf.Spine
		//};

		private static readonly List<string> HerbalParts = new List<string>
		{
			"Arm",
			"Leg", 
			"Hand", 
			"Foot"
		};

		private static readonly List<string> NormalParts = new List<string>
		{
			"Eye", 
			"Ear", 
			"Nose", 
			"Jaw", 
			"Tongue"
		};

		private static readonly List<string> BestParts = new List<string>
		{
			"Stomach", 
			"Liver",
			"Lung", 
			"Kidney",
			"Heart", 
			"Brain", 
			"Spine"
		};

		public static List<BodyPartRecord> GetEligibleParts(Pawn pawn)
		{
			var eligibleParts = new List<BodyPartRecord>();
			if (pawn == null) return eligibleParts;

			MedicalCareCategory policyCare = pawn.playerSettings?.medCare ?? MedicalCareCategory.NormalOrWorse;
			MedicalCareCategory availableCare = GetBestAvailableMedicine(pawn.Map);
			MedicalCareCategory medCare = (MedicalCareCategory)Math.Min((int)policyCare, (int)availableCare);

			foreach (BodyPartRecord part in pawn.health.hediffSet.GetNotMissingParts())
			{
				/*if (pawn.health.hediffSet.HasHediff(ReinforceTattooDefOf.ReinforceTattoo, part))
				{
					continue;
				}*/

				bool partAllowed = false;
				if (medCare >= MedicalCareCategory.HerbalOrWorse && HerbalParts.Contains(part.def.defName)) partAllowed = true;
				if (medCare >= MedicalCareCategory.NormalOrWorse && NormalParts.Contains(part.def.defName)) partAllowed = true;
				if (medCare >= MedicalCareCategory.Best && BestParts.Contains(part.def.defName)) partAllowed = true;

				if (partAllowed)
				{
					eligibleParts.Add(part);
				}
				else
				{
					if (!HerbalParts.Contains(part.def.defName) && !NormalParts.Contains(part.def.defName) && !BestParts.Contains(part.def.defName)) 
					{
						Log.Message("continue : " + part.def.ToString());
						continue; 
					}
					StringBuilder sb = new StringBuilder();
					sb.Append(part.def.defName).Append(" not allowed. ");
					sb.Append("requiredCare: ");
					if (BestParts.Contains(part.def.defName))
					{
						sb.Append(MedicalCareCategory.Best.ToString());
					}
					if (NormalParts.Contains(part.def.defName))
					{
						sb.Append(MedicalCareCategory.NormalOrWorse.ToString());
					}
					if (HerbalParts.Contains(part.def.defName))
					{
						sb.Append(MedicalCareCategory.HerbalOrWorse.ToString());
					}
					sb.Append(", policyCare: ").Append(policyCare.ToString());
					sb.Append(", availableCare: ").Append(availableCare.ToString());
					Log.Message(sb.ToString());
				}
			}
			return eligibleParts;
		}

		private static MedicalCareCategory GetBestAvailableMedicine(Map map)
		{
			if (map == null) return MedicalCareCategory.NoCare;
			if (map.listerThings.ThingsOfDef(ThingDefOf.MedicineUltratech).Where(t => !t.IsForbidden(Faction.OfPlayer)).Sum(t => t.stackCount) >= 5)
			{
				Log.Message("Best available medicine: Ultratech");
				return MedicalCareCategory.Best;
			}
			if (map.listerThings.ThingsOfDef(ThingDefOf.MedicineIndustrial).Where(t => !t.IsForbidden(Faction.OfPlayer)).Sum(t => t.stackCount) >= 5)
			{
				Log.Message("Best available medicine: Industrial");
				return MedicalCareCategory.NormalOrWorse;
			}
			if (map.listerThings.ThingsOfDef(ThingDefOf.MedicineHerbal).Where(t => !t.IsForbidden(Faction.OfPlayer)).Sum(t => t.stackCount) >= 5)
			{
				Log.Message("Best available medicine: Herbal");
				return MedicalCareCategory.HerbalOrWorse;
			}
			Log.Message("No available medicine");
			return MedicalCareCategory.NoCare;
		}



		// 수술이 완료되었을 때 호출되는 메서드
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
			Log.Message(billDoer + " apply on pawn " + pawn + ", bodypart : "+part);
            // 시술자가 있는지 확인 (개발자 모드 등으로 강제 실행 시 없을 수 있음)
            if (billDoer == null || part == null) return;
            
            // 1. 성공률 계산 (의사 능력 기반)
            float successChance = billDoer.GetStatValue(StatDefOf.MedicalSurgerySuccessChance)+0.1f;

            // 2. 성공/실패 판정
            if (Rand.Chance(successChance))
            {
                // 성공
                Messages.Message("ReinforceTattoo_Success".Translate(billDoer.LabelShort, pawn.LabelShort, part.Label),
                    pawn, MessageTypeDefOf.PositiveEvent);

                // 3. 품질 결정 (예술 능력 기반)
                QualityCategory quality = QualityUtility.GenerateQualityCreatedByPawn(billDoer, SkillDefOf.Artistic);

                // 4. 영구적인 Hediff 생성 및 품질 설정
                Hediff hediff = HediffMaker.MakeHediff(ReinforceTattooDefOf.ReinforceTattoo, pawn, part);

                // HediffComp를 가져와서 계산된 품질을 설정
                var comp = hediff.TryGetComp<HediffComp_ReinforceTattoo>();
                if (comp != null)
                {
                    comp.SetQuality(quality);
                }

                // 영구적인 흉터 상태로 만들기 위해 심각도를 1 이상으로 설정
                // HediffComp_GetsPermanent가 작동하여 영구 상태로 만듭니다.
                hediff.Severity = 1f;
				hediff.TryGetComp<HediffComp_GetsPermanent>().IsPermanent = true;
				(hediff as Hediff_Injury).
                pawn.health.AddHediff(hediff);
            }
            else
            {
                // 실패
                if (Rand.Chance(0.5f))
                {
                    // 경미한 실패
                    Messages.Message("ReinforceTattoo_FailMinor".Translate(billDoer.LabelShort, pawn.LabelShort).CapitalizeFirst(),
                        pawn, MessageTypeDefOf.NegativeEvent);
                }
                else
                {
                    // 치명적인 실패
                    Messages.Message("ReinforceTattoo_FailCatastrophic".Translate(billDoer.LabelShort, pawn.LabelShort, part.Label).CapitalizeFirst(),
                        pawn, MessageTypeDefOf.NegativeEvent, false);
                    
                    // 수술 부위에 심각한 부상 부여
                    HealthUtility.GiveRandomSurgeryInjuries(pawn, 20, part);
                }
            }
        }
    }
}
