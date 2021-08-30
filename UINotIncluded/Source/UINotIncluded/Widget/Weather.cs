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
    static class Weather
    {
        public static void DoWeatherGUI(WidgetRow row, float height, float width)
        {
            ExtendedToolbar.DoToolbarBackground(new Rect(row.FinalX, row.FinalY, width, height));
            WeatherDef weatherPerceived = Find.CurrentMap.weatherManager.CurWeatherPerceived;

            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Tiny;
            Texture2D icon = ModTextures.WeatherIcon(weatherPerceived.GetModExtension<WeatherDefExtension>().icon);

            Rect iconSpace;
            if (!weatherPerceived.description.NullOrEmpty())
                iconSpace = row.Icon(icon, weatherPerceived.LabelCap + "\n" + weatherPerceived.description);
            else
                iconSpace = row.Icon(icon, weatherPerceived.LabelCap);

            float temp = Mathf.RoundToInt(GenTemperature.CelsiusTo(Find.World.tileTemperatures.GetOutdoorTemp(Find.CurrentMap.Tile), Prefs.TemperatureMode));

            float climaWidth = width - iconSpace.width - ExtendedToolbar.padding;
            row.Label(temp.ToString() + new string[] { "°C", "°F", "°K" }[(int)Prefs.TemperatureMode], climaWidth,null,height);
            Text.Anchor = TextAnchor.UpperLeft;
        }
    }
}
