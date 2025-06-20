using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReinforceTattooMOD
{
	public class ReinforceTattooSettings : ModSettings
	{
		public float factor = 1f;
		public float pctBonusAwful = 0.1f;
		public float pctBonusPoor = 0.2f;
		public float pctBonusNormal = 0.3f;
		public float pctBonusGood = 0.5f;
		public float pctBonusExcellent = 0.8f;
		public float pctBonusMasterwork = 1.0f;
		public float pctBonusLegendary = 2.0f;
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.factor,"efficiency",defaultValue:1f,forceSave:true);
			Scribe_Values.Look<float>(ref this.pctBonusAwful, "pctBonusAwful", defaultValue:0.1f,forceSave:true);
			Scribe_Values.Look<float>(ref this.pctBonusPoor, "pctBonusPoor", defaultValue:0.2f,forceSave:true);
			Scribe_Values.Look<float>(ref this.pctBonusNormal, "pctBonusNormal", defaultValue:0.3f,forceSave:true);
			Scribe_Values.Look<float>(ref this.pctBonusGood, "pctBonusGood", defaultValue:0.5f,forceSave:true);
			Scribe_Values.Look<float>(ref this.pctBonusExcellent, "pctBonusExcellent", defaultValue:0.8f,forceSave:true);
			Scribe_Values.Look<float>(ref this.pctBonusMasterwork, "pctBonusMasterwork", defaultValue:1.0f,forceSave:true);
			Scribe_Values.Look<float>(ref this.pctBonusLegendary, "pctBonusLegendary", defaultValue:2.0f,forceSave:true);
		}
	}
}
