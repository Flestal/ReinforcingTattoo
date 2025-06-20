using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ReinforceTattooMOD
{
	public class ReinforceTattoo : Mod
	{
		public ReinforceTattooSettings settings;
		public static ReinforceTattoo instance;
		public ReinforceTattoo(ModContentPack content) : base(content)
		{
			instance = this;
			this.settings = GetSettings<ReinforceTattooSettings>();
			LongEventHandler.ExecuteWhenFinished(AfterDefsLoaded);
		}
		private void AfterDefsLoaded()
		{
			//ReinforceTattooDefOf.ReinforceTattoo.comps
			HediffDef tattooDef = ReinforceTattooDefOf.ReinforceTattoo;
			var compProps = tattooDef.comps.OfType<HediffCompProperties_ReinforceTattoo>().FirstOrDefault();
			if (compProps == null) return;

			compProps.pctBonusPerQuality = new List<float>() { 
				settings.pctBonusAwful,
				settings.pctBonusPoor,
				settings.pctBonusNormal,
				settings.pctBonusGood,
				settings.pctBonusExcellent,
				settings.pctBonusMasterwork,
				settings.pctBonusLegendary
			};
		}
		public override void DoSettingsWindowContents(Rect inRect)
		{
			base.DoSettingsWindowContents(inRect);
			Listing_Standard listingStandard = new Listing_Standard();
			listingStandard.Begin(inRect);

			listingStandard.Label("settingAlert".Translate().CapitalizeFirst());
			listingStandard.Label("settingCommon".Translate().CapitalizeFirst() + settings.factor.ToString("F1"));
			settings.factor = Mathf.Round(listingStandard.Slider(settings.factor, 0.1f, 5f) * 10) * 0.1f;
			listingStandard.Label("settingAwful".Translate().CapitalizeFirst() + settings.pctBonusAwful.ToString("F1"));
			settings.pctBonusAwful = Mathf.Round(listingStandard.Slider(settings.pctBonusAwful, 0.1f, 5f)*10)*0.1f;
			listingStandard.Label("settingPoor".Translate().CapitalizeFirst() + settings.pctBonusPoor.ToString("F1"));
			settings.pctBonusPoor = Mathf.Round(listingStandard.Slider(settings.pctBonusPoor, 0.1f, 5f) * 10) * 0.1f;
			listingStandard.Label("settingNormal".Translate().CapitalizeFirst() +settings.pctBonusNormal.ToString("F1"));
			settings.pctBonusNormal = Mathf.Round(listingStandard.Slider(settings.pctBonusNormal, 0.1f, 5f) * 10) * 0.1f;
			listingStandard.Label("settingGood".Translate().CapitalizeFirst() + settings.pctBonusGood.ToString("F1"));
			settings.pctBonusGood = Mathf.Round(listingStandard.Slider(settings.pctBonusGood, 0.1f, 5f) * 10) * 0.1f;
			listingStandard.Label("settingExcellent".Translate().CapitalizeFirst() + settings.pctBonusExcellent.ToString("F1"));
			settings.pctBonusExcellent = Mathf.Round(listingStandard.Slider(settings.pctBonusExcellent, 0.1f, 5f) * 10) * 0.1f;
			listingStandard.Label("settingMasterwork".Translate().CapitalizeFirst() + settings.pctBonusMasterwork.ToString("F1"));
			settings.pctBonusMasterwork = Mathf.Round(listingStandard.Slider(settings.pctBonusMasterwork, 0.1f, 5f) * 10) * 0.1f;
			listingStandard.Label("settingLegendary".Translate().CapitalizeFirst() + settings.pctBonusLegendary.ToString("F1"));
			settings.pctBonusLegendary = Mathf.Round(listingStandard.Slider(settings.pctBonusLegendary, 0.1f, 5f) * 10) * 0.1f;
			listingStandard.End();
		}
		public override string SettingsCategory()
		{
			return "ReinforceTattooSettings".Translate();
		}
		public override void WriteSettings()
		{
			base.WriteSettings();
		}
	}
}
