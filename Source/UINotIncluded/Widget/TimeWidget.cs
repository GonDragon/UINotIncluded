using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Verse;
using RimWorld;
using RimWorld.Planet;

namespace UINotIncluded.Widget
{
    static class TimeWidget
    {
        private static readonly float extraSize = 12f; // Width with WidgetRow is inconsistent. Dirty fix.
        public static void DoTimeWidget(WidgetRow row, float height, float width)
        {
            if (Find.CurrentMap == null) return;

            
            float gadgetsWidth = width - 2 * ExtendedToolbar.padding;
            float datetimeWidth = gadgetsWidth;
            if (Prefs.ShowRealtimeClock)
            {
                float realtimeWidth = (float)Math.Floor(gadgetsWidth * 0.27f);
                TimeWidget.DoRealtimeClock(row, height, realtimeWidth);
                row.Gap(ExtendedToolbar.interGap + 2 * ExtendedToolbar.padding);
                datetimeWidth -= realtimeWidth - 2 * ExtendedToolbar.padding - ExtendedToolbar.interGap;
            } else
            {
                datetimeWidth += extraSize;
            }
            TimeWidget.DoDateAndTime(row, height, datetimeWidth);
        }

        public static void DoRealtimeClock(WidgetRow row, float height, float width)
        {
            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Tiny;
            ExtendedToolbar.DoToolbarBackground(new Rect(row.FinalX, row.FinalY, width, height));

            Rect iconSpace = row.Icon(ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/world"));
            String label = DateTime.Now.ToString("HH:mm");

            row.Label(label, width - iconSpace.width, null,height);
            Text.Anchor = TextAnchor.UpperLeft;
        }

        public static void DoDateAndTime(WidgetRow row, float height, float width)
        {
            float startX = row.FinalX;
            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Tiny;

            ExtendedToolbar.DoToolbarBackground(new Rect(row.FinalX, row.FinalY, width, height));
            Vector2 pos = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);

            Season season = GenDate.Season((long)Find.TickManager.TicksAbs, pos);
            row.Icon(season.GetIconTex(), season.LabelCap());

            float hour = GenDate.HourFloat((long)Find.TickManager.TicksAbs, pos.x);
            int minutes = (int)Math.Floor((hour - Math.Floor(hour)) * 6) * 10;
            string timestamp = Math.Floor(hour).ToString() + ":" + minutes.ToString("D2") + " hs";
            string datestamp = UINotIncludedSettings.dateFormat.GetFormated((long)Find.TickManager.TicksAbs, pos.x);

            row.Label(datestamp,-1, GetDateDescription(pos,season),height);
            row.Label(timestamp, width - (row.FinalX - startX));
            Text.Anchor = TextAnchor.UpperLeft;

        }

        private static string GetDateDescription(Vector2 pos, Season season)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 4; ++i)
            {
                Quadrum quadrum = (Quadrum)i;
                stringBuilder.AppendLine(quadrum.Label() + " - " + quadrum.GetSeason(pos.y).LabelCap());
            }
            TaggedString description = "DateReadoutTip".Translate((NamedArgument)GenDate.DaysPassed, (NamedArgument)15, (NamedArgument)season.LabelCap(), (NamedArgument)15, (NamedArgument)GenDate.Quadrum((long)GenTicks.TicksAbs, pos.x).Label(), (NamedArgument)stringBuilder.ToString());
            return description.ToString();
        }
    }
}
