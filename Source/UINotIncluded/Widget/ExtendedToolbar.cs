using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    static class ExtendedToolbar
    {

        public static float height = 35f;
        public static float interGap = 5f;
        public static float padding = 3f;

        public static void ExtendedToolbarOnGUI(float x, float y, float width)
        {
            WidgetRow row = new WidgetRow();
            Rect rect = new Rect(x, y, width, height);
            GUI.DrawTexture(rect, SolidColorMaterials.NewSolidColorTexture(new ColorInt(111, 111, 111, (int)byte.MaxValue).ToColor));
            Widgets.DrawAtlas(new Rect(x + 1f, y + 1f, width - 2f, height - 2f), ContentFinder<Texture2D>.Get("GD/UI/ClockBG"));

            float rowHeight = height - 10f;
            row.Init(x + 6f, y + 5f, UIDirection.RightThenDown,width, padding);
            Weather.DoWeatherGUI(row, rowHeight);
            row.Gap(interGap);
            Time.DoTimeWidget(row, rowHeight);
            row.Gap(interGap);
            Timespeed.DoTimespeedControls(row, rowHeight);

        }

        public static void DoToolbarBackground(Rect rect)
        {
            Rect rect1 = new Rect(rect.x, rect.y-2, rect.width, rect.height+2);
            Widgets.DrawAtlas(rect1, ContentFinder<Texture2D>.Get("GD/UI/ClockSCR"));
        }
    }
}
