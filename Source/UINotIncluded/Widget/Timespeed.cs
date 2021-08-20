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
        public static void DoTimespeedControls(WidgetRow row, float height, float width)
        {
            ExtendedToolbar.DoToolbarBackground(new Rect(row.FinalX, row.FinalY, width, height));
            row.Gap(ExtendedToolbar.padding);

            Rect timerRect = new Rect(row.FinalX, row.FinalY, width, height);
            TimeControls.DoTimeControlsGUI(timerRect);
        }
    }
}
