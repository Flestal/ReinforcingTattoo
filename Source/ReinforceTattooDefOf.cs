using RimWorld;
using Verse;

namespace ReinforceTattooMOD
{
	[DefOf]
	public static class ReinforceTattooDefOf
	{
		static ReinforceTattooDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ReinforceTattooDefOf));
		}
		public static HediffDef ReinforceTattoo;

		// --- 바닐라 BodyPartDefOf에 없는 부위들을 직접 참조하기 위해 추가 ---
		// Herbal Parts
		public static BodyPartDef Foot;

		// Normal Parts
		public static BodyPartDef Ear;
		public static BodyPartDef Nose;
		public static BodyPartDef Jaw;
		public static BodyPartDef Tongue;

		// Best Parts
		public static BodyPartDef Stomach;
		public static BodyPartDef Liver;
		public static BodyPartDef Kidney;
		public static BodyPartDef Brain;
		public static BodyPartDef Spine;
	}
}
