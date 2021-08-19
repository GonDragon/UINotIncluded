﻿using System;
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
    static class Weather
    {
        public const float width = 65f;
        public static void DoWeatherGUI(WidgetRow row, float height)
        {
            ExtendedToolbar.DoToolbarBackground(new Rect(row.FinalX, row.FinalY, width, height));
            float startX = row.FinalX;

            row.Gap(ExtendedToolbar.padding);
            WeatherDef weatherPerceived = Find.CurrentMap.weatherManager.CurWeatherPerceived;

            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Tiny;
            Texture2D icon = ContentFinder<Texture2D>.Get(weatherPerceived.GetModExtension<WeatherDefExtension>().icon);

            if (!weatherPerceived.description.NullOrEmpty())
                row.Icon(icon, weatherPerceived.LabelCap + "\n" + weatherPerceived.description);
            else
                row.Icon(icon, weatherPerceived.LabelCap);

            float temp = Mathf.RoundToInt(GenTemperature.CelsiusTo(Find.World.tileTemperatures.GetOutdoorTemp(Find.CurrentMap.Tile), Prefs.TemperatureMode));

            float climaWidth = width - (row.FinalX - startX) - ExtendedToolbar.padding;
            row.Label(temp.ToString() + new string[] { "°C", "°F", "°K" }[(int)Prefs.TemperatureMode], climaWidth,null,height);
            row.Gap(ExtendedToolbar.padding);
            Text.Anchor = TextAnchor.UpperLeft;
        }
    }
}