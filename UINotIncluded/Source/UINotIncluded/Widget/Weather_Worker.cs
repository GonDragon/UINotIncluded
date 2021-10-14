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
    public class Weather_Worker : WidgetWorker
    {
        public override float GetWidth()
        {
            return (float)Math.Round(def.minWidth + 11.66f * (float)Settings.fontSize);
        }

        public override bool WidgetVisible
        {
            get => Find.CurrentMap != null;
        }

        public override void OnGUI(Rect rect)
        {
            this.Margins(ref rect);
            ExtendedToolbar.DoToolbarBackground(rect);
            this.Padding(ref rect);

            WeatherDef weatherPerceived = Find.CurrentMap.weatherManager.CurWeatherPerceived;
            Texture2D icon = ModTextures.WeatherIcon(weatherPerceived.GetModExtension<WeatherDefExtension>()?.icon ?? "GD/UI/Icons/Weather/Unknown");

            Rect iconSpace;
            if (!weatherPerceived.description.NullOrEmpty())
                iconSpace = DrawIcon(icon, rect.x, weatherPerceived.LabelCap + "\n" + weatherPerceived.description);
            else
                iconSpace = DrawIcon(icon, rect.x, weatherPerceived.LabelCap);
            rect.x += iconSpace.width;
            rect.width -= iconSpace.width;

            WidgetRow row = new WidgetRow(rect.x, rect.y, UIDirection.RightThenDown, gap: ExtendedToolbar.interGap);
            float temp = Mathf.RoundToInt(GenTemperature.CelsiusTo(Find.World.tileTemperatures.GetOutdoorTemp(Find.CurrentMap.Tile), Prefs.TemperatureMode));

            row.Label(temp.ToString() + new string[] { "°C", "°F", "°K" }[(int)Prefs.TemperatureMode], rect.width, null, rect.height);
        }
    }
}
