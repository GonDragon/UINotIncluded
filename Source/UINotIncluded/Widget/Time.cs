using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;

namespace UINotIncluded.Widget
{
    static class Time
    {
        public const float width = 65f;
        public static void DoTimeWeather(WidgetRow row, float height)
        {
            Rect background = new Rect(row.FinalX - 2, row.FinalY, width + 2, height);
            ExtendedToolbar.DoToolbarBackground(background);
            row.Label("This contain text");

        }
    }
}
