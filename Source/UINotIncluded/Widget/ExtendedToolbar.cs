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
        public static float interGap = 2;
        public static float padding = 1;
        public static float margin = 5;
        public static float tail = 11f; // I did the math wrong somewhere. This fixes that.

        public static void ExtendedToolbarOnGUI(float x, float y, float width)
        {
            WidgetRow row = new WidgetRow();
            Rect rect = new Rect(x, y, width, height);
            GUI.DrawTexture(rect, SolidColorMaterials.NewSolidColorTexture(new ColorInt(111, 111, 111, (int)byte.MaxValue).ToColor));
            
            Widgets.DrawAtlas(new Rect(x + 1f, y + 1f, width - 2f, height - 2f), ContentFinder<Texture2D>.Get("GD/UI/ClockBG"));

            float widgetSpace = (float)Math.Floor(width - margin * 2);
            float weatherSpace = (float)Math.Floor(widgetSpace / 6f) - 2 * padding - interGap;
            float timeSpace = (float)Math.Floor(widgetSpace / 2f) - 2 * padding - interGap;
            float timeSpeedSpace = (float)Math.Floor(widgetSpace / 3f) - (2 * padding) - tail;

            float rowHeight = height - 10f;
            row.Init(x + margin + padding, y + 5f, UIDirection.RightThenDown, widgetSpace, padding);
            Weather.DoWeatherGUI(row, rowHeight, weatherSpace);
            row.Gap(interGap + 2 * padding);
            Time.DoTimeWidget(row, rowHeight, timeSpace);
            row.Gap(interGap + 2 * padding);
            Timespeed.DoTimespeedControls(row, rowHeight, timeSpeedSpace);

        }

        public static void DoToolbarBackground(Rect rect)
        {
            Rect rect1 = new Rect(rect.x-padding, rect.y-2, rect.width+padding*2, rect.height+2);
            Widgets.DrawAtlas(rect1, ContentFinder<Texture2D>.Get("GD/UI/ClockSCR"));
        }
    }
}
