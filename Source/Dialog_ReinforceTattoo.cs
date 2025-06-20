using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace ReinforceTattooMOD
{
	public class Dialog_InstallReinforceTattoo : Window
	{
		private readonly Pawn patient;
		private readonly Action<BodyPartRecord> onPartSelected;
		private readonly List<BodyPartRecord> availableParts;
		private Vector2 scrollPosition = Vector2.zero;

		public override Vector2 InitialSize => new Vector2(480f, 600f);

		public Dialog_InstallReinforceTattoo(Pawn patient, Action<BodyPartRecord> onPartSelected)
		{
			this.patient = patient;
			this.onPartSelected = onPartSelected;
			// RecipeWorker에서 분리된 로직을 호출하여 선택 가능한 부위 목록을 가져옵니다.
			this.availableParts = Recipe_InstallReinforceTattoo.GetEligibleParts(patient);

			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
		}

		public override void DoWindowContents(Rect inRect)
		{
			Listing_Standard listing = new Listing_Standard();
			listing.Begin(inRect);

			// 창 제목
			Text.Font = GameFont.Medium;
			listing.Label("ReinforceTattoo_SelectPartTitle".Translate(patient.LabelShortCap).CapitalizeFirst());
			Text.Font = GameFont.Small;
			listing.GapLine();

			// 부위 목록
			if (availableParts.NullOrEmpty())
			{
				listing.Label("ReinforceTattoo_NoEligibleParts".Translate().CapitalizeFirst());
			}
			else
			{
				// 스크롤 뷰 영역 계산
				Rect scrollViewRect = listing.GetRect(inRect.height - listing.CurHeight - 40f);
				Rect viewRect = new Rect(0f, 0f, scrollViewRect.width - 16f, availableParts.Count * 32f);

				Widgets.BeginScrollView(scrollViewRect, ref scrollPosition, viewRect);

				Listing_Standard scrollListing = new Listing_Standard();
				scrollListing.Begin(viewRect);

				foreach (BodyPartRecord part in availableParts)
				{
					if (scrollListing.ButtonText(part.LabelCap))
					{
						// 부위가 선택되면 콜백 함수를 실행하고 창을 닫습니다.
						this.onPartSelected(part);
						this.Close();
					}
				}

				scrollListing.End();
				Widgets.EndScrollView();
			}

			listing.End();
		}
	}
}
