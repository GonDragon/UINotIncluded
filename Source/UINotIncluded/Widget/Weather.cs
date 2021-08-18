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
        public const float width = 65f;

        public static float DoWeatherGUI(WidgetRow row, float height)
        {
            Traverse globalControls = Traverse.Create(Find.MapUI.globalControls);

            Rect background = new Rect(row.FinalX -2, row.FinalY, width+2, height);
            ExtendedToolbar.DoToolbarBackground(background);

            float climaWidth = width / 2;
            WeatherDef weatherPerceived = Find.CurrentMap.weatherManager.CurWeatherPerceived;
            Rect rect = new Rect(row.FinalX, row.FinalY, climaWidth, height);

            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Tiny;
            Texture2D icon = ContentFinder<Texture2D>.Get(weatherPerceived.GetModExtension<WeatherDefExtension>().icon);

            if (!weatherPerceived.description.NullOrEmpty())
                row.Icon(icon, weatherPerceived.LabelCap + "\n" + weatherPerceived.description);
            else
                row.Icon(icon, weatherPerceived.LabelCap);

            float temp = Mathf.RoundToInt(GenTemperature.CelsiusTo(Find.World.tileTemperatures.GetOutdoorTemp(Find.CurrentMap.Tile), Prefs.TemperatureMode));
            


            row.Label(temp.ToString() + new string[] {"°C", "°F", "°K" }[(int)Prefs.TemperatureMode], climaWidth,null,height);

            Text.Anchor = TextAnchor.UpperLeft;
            return width;
        }
    }
}
