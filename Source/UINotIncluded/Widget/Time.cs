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
    static class Time
    {
        public static void DoTimeWidget(WidgetRow row, float height, float width)
        {
            if (Find.CurrentMap == null) return;

            float gadgetsWidth = width - 2 * ExtendedToolbar.padding - ExtendedToolbar.interGap;
            float timeWidth = (float)Math.Floor(gadgetsWidth / 3);
            float dateWidth = gadgetsWidth - timeWidth;
            
            Vector2 pos = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
            Time.DoHours(row, height, pos, timeWidth);
            row.Gap(ExtendedToolbar.interGap + 2 * ExtendedToolbar.padding);
            Time.DoDate(row, height, pos, dateWidth);
            

        }

        public static void DoHours(WidgetRow row, float height, Vector2 pos, float width)
        {
            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Tiny;
            ExtendedToolbar.DoToolbarBackground(new Rect(row.FinalX, row.FinalY, width, height));


            float hour = GenDate.HourFloat((long)Find.TickManager.TicksAbs, pos.x);
            int minutes = (int)Math.Floor((hour - Math.Floor(hour)) * 6) * 10;
            string label = Math.Floor(hour).ToString() + ":" + minutes.ToString("D2") + " hs";

            row.Label(label, width, null,height);
            Text.Anchor = TextAnchor.UpperLeft;
        }

        public static void DoDate(WidgetRow row, float height, Vector2 pos, float width)
        {
            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Tiny;

            ExtendedToolbar.DoToolbarBackground(new Rect(row.FinalX, row.FinalY, width, height));


            Season season = GenDate.Season((long)Find.TickManager.TicksAbs, pos);
            Rect iconSpace = row.Icon(season.GetIconTex(), season.LabelCap());

            float labelWidth = width - iconSpace.width;
            row.Label(UINotIncludedSettings.dateFormat.GetFormated((long)Find.TickManager.TicksAbs, pos.x), labelWidth, GetDateDescription(pos,season),height);
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
