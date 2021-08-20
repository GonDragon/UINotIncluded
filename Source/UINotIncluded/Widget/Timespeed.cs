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
    static class Timespeed
    {
        public static float width = 140f;
        public static void DoTimespeedControls(WidgetRow row, float height)
        {

            float startX = row.FinalX;
            ExtendedToolbar.DoToolbarBackground(new Rect(row.FinalX, row.FinalY, width, height));
            row.Gap(ExtendedToolbar.padding);
            float width2 = width - ExtendedToolbar.padding * 2;

            Rect timerRect = new Rect(row.FinalX, row.FinalY, width2, height);
            TimeControls.DoTimeControlsGUI(timerRect);
            row.Gap(width2 + ExtendedToolbar.padding);
        }
    }
}
