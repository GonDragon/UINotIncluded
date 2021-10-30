using System;

using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    public class Weather_Worker : WidgetWorker
    {
        private float widthCache = -1;
        private GameFont fontCache = GameFont.Tiny;
        private TemperatureDisplayMode cacheMode = Prefs.TemperatureMode;

        public override float GetWidth()
        {
            if (widthCache < 0 || fontCache != Settings.fontSize || cacheMode != Prefs.TemperatureMode)
            {
                widthCache = (float)Math.Round(Math.Max(def.minWidth, Text.CalcSize((1000f).ToStringTemperature()).x * (((float)Settings.fontSize / 3f) + 1f)));
                fontCache = Settings.fontSize;
                cacheMode = Prefs.TemperatureMode;
            }
            return widthCache;
        }

        public override bool WidgetVisible
        {
            get => Find.CurrentMap != null;
        }

        public override void OnGUI(Rect rect, BarElementMemory memory)
        {
            this.Margins(ref rect);
            ExtendedToolbar.DoWidgetBackground(rect);
            this.Padding(ref rect);

            WeatherDef weatherPerceived = Find.CurrentMap.weatherManager.CurWeatherPerceived;
            Texture2D icon = ModTextures.WeatherIcon(weatherPerceived.GetModExtension<WeatherDefExtension>()?.icon ?? "GD/UI/Icons/Weather/Unknown");

            Rect iconSpace;
            if (!weatherPerceived.description.NullOrEmpty())
                iconSpace = DrawIcon(icon, rect.x, rect.y, weatherPerceived.LabelCap + "\n" + weatherPerceived.description);
            else
                iconSpace = DrawIcon(icon, rect.x, rect.y, weatherPerceived.LabelCap);
            rect.x += iconSpace.width;
            rect.width -= iconSpace.width;

            WidgetRow row = new WidgetRow(rect.x, rect.y, UIDirection.RightThenDown, gap: ExtendedToolbar.interGap);
            string tempLabel = Mathf.Round(Find.World.tileTemperatures.GetOutdoorTemp(Find.CurrentMap.Tile)).ToStringTemperature("F0");

            row.Label(tempLabel, rect.width, null, rect.height);
        }
    }
}