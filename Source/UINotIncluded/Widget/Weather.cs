using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    static class Weather
    {
        public const float width = 90f;

        public static float DoWeatherGUI(WidgetRow row, float height)
        {
            Rect background = new Rect(row.FinalX -2, row.FinalY, width+2, height);
            ExtendedToolbar.DoToolbarBackground(background);

            float climaWidth = width / 3;
            WeatherDef weatherPerceived = Find.CurrentMap.weatherManager.CurWeatherPerceived;
            Rect rect = new Rect(row.FinalX, row.FinalY, climaWidth, height);

            Text.Anchor = TextAnchor.MiddleLeft;
            Text.Font = GameFont.Tiny;

            Texture2D icon = ContentFinder<Texture2D>.Get(weatherPerceived.GetModExtension<WeatherDefExtension>().icon);

            if (!weatherPerceived.description.NullOrEmpty())
                row.Icon(icon, weatherPerceived.LabelCap + "\n" + weatherPerceived.description);
            else
                row.Icon(icon, weatherPerceived.LabelCap);
            Text.Anchor = TextAnchor.UpperLeft;
            return width;
        }
    }
}
