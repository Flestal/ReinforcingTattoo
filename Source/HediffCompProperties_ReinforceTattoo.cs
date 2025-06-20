using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReinforceTattooMOD
{
	public class HediffCompProperties_ReinforceTattoo : HediffCompProperties
	{
		// XML의 pctBonusPerQuality 리스트를 저장할 변수
		public List<float> pctBonusPerQuality = new List<float>();

		public HediffCompProperties_ReinforceTattoo()
		{
			// 이 Properties를 사용하는 Comp의 클래스를 지정
			this.compClass = typeof(HediffComp_ReinforceTattoo);
		}
	}
}
