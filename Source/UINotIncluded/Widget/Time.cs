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
        public static void DoTimeWidget(WidgetRow row, float height)
        {
            if (Find.CurrentMap == null) return;

            
            Vector2 pos = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
            Time.DoHours(row, height, pos);
            row.Gap(ExtendedToolbar.interGap);
            Time.DoDate(row, height, pos);
            

        }

        public static void DoHours(WidgetRow row, float height, Vector2 pos)
        {
            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Tiny;
            float width = 60f;
            float startX = row.FinalX;
            ExtendedToolbar.DoToolbarBackground(new Rect(row.FinalX, row.FinalY, width, height));
            row.Gap(ExtendedToolbar.padding);

            float hour = GenDate.HourFloat((long)Find.TickManager.TicksAbs, pos.x);
            int minutes = (int)Math.Floor((hour - Math.Floor(hour)) * 6) * 10;
            string label = Math.Floor(hour).ToString() + ":" + minutes.ToString("D2") + " hs";

            float labelWidth = width - (row.FinalX - startX) - ExtendedToolbar.padding;
            row.Label(label, labelWidth, null,height);
            row.Gap(ExtendedToolbar.padding);
            Text.Anchor = TextAnchor.UpperLeft;
        }

        public static void DoDate(WidgetRow row, float height, Vector2 pos)
        {
            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Small;
            float width = 130f;
            float startX = row.FinalX;
            ExtendedToolbar.DoToolbarBackground(new Rect(row.FinalX, row.FinalY, width, height));
            row.Gap(ExtendedToolbar.padding);

            Season season = GenDate.Season((long)Find.TickManager.TicksAbs, pos);

            row.Icon(season.GetIconTex(), season.LabelCap());
            float labelWidth = width - (row.FinalX - startX) - ExtendedToolbar.padding;
            row.Label(UINotIncludedSettings.dateFormat.GetFormated((long)Find.TickManager.TicksAbs, pos.x), labelWidth, null,height);
            row.Gap(ExtendedToolbar.padding);
            Text.Anchor = TextAnchor.UpperLeft;

        }
    }
}
