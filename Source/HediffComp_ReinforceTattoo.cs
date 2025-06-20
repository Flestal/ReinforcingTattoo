using System.Collections.Generic;
using System.Text;
using RimWorld;
using Verse;

namespace ReinforceTattooMOD
{
	// 1. XML의 <comps>에서 데이터를 담기 위한 클래스

	// 2. Hediff의 실제 로직을 담당하는 Comp 클래스
	public class HediffComp_ReinforceTattoo : HediffComp
	{
		// 위 Properties 클래스를 쉽게 가져오기 위한 Getter
		public HediffCompProperties_ReinforceTattoo Props
		{
			get
			{
				return (HediffCompProperties_ReinforceTattoo)this.props;
			}
		}
		private ReinforceTattooSettings settings;
		private QualityCategory Quality;
		private float offset_;

		public HediffComp_ReinforceTattoo()
		{
			settings = ReinforceTattoo.instance.settings;
		}
		public void SetQuality(QualityCategory Quality)
		{
			this.Quality = Quality;
		}

		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<QualityCategory>(ref this.Quality,"quality",QualityCategory.Normal,true);
		}
		

		// 능률 보너스 (예: 0.1f는 +10%)
		public float GetBonusForQuality()
		{
			// 심각도/부위 최대체력으로 오프셋 설정
			float offset = this.parent.Severity / this.parent.Part.def.GetMaxHealth(this.Pawn);
			offset_ = offset;
			// 품질을 정수 인덱스로 변환 (끔찍=0, 형편없음=1, ...)
			int qualityIndex = (int)this.Quality;

			// 품질 값 * 설정 값 + 오프셋 으로 보너스 반환
			if (this.Props.pctBonusPerQuality != null && qualityIndex >= 0 && qualityIndex < this.Props.pctBonusPerQuality.Count)
			{
				return this.Props.pctBonusPerQuality[qualityIndex] * settings.factor + offset;
			}

			// 해당하는 품질의 보너스가 없으면 0을 반환
			return 0f;
		}

		// 툴팁에 추가 정보를 표시합니다. (예: "신체 부위 능률: +12%")
		public override string CompTipStringExtra
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(base.CompTipStringExtra);
				float bonus = GetBonusForQuality();
				if (bonus > 0)
				{
					sb.AppendLine("PartEfficiency".Translate().CapitalizeFirst()+" : ");
					sb.Append("StatOffset".Translate().CapitalizeFirst() + " " + this.Props.pctBonusPerQuality[(int)this.Quality].ToStringPercent()+", ");
					sb.Append("PenaltyNeutralize".Translate().CapitalizeFirst() + " " + offset_.ToStringPercent());
				}
				return sb.ToString().TrimEndNewlines();
			}
		}

		// 건강 탭의 라벨 옆에 품질을 표시하기 위한 텍스트를 제공합니다.
		public override string CompLabelInBracketsExtra
		{
			get
			{
				return Quality.GetLabel();
			}
		}
	}
}
