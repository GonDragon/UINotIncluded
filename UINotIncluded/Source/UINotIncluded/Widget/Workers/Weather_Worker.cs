using System;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget.Workers
{
    public class Weather_Worker : WidgetWorker
    {
        private float widthCache = -1;
        private GameFont fontCache = GameFont.Tiny;
        private TemperatureDisplayMode cacheMode = Prefs.TemperatureMode;
        private float cacheHeight = ExtendedToolbar.Height;
        private readonly Configs.WeatherConfig config;

        public new Func<bool> Visible = () => (Find.CurrentMap != null);

        public Weather_Worker(Configs.WeatherConfig config)
        {
            this.config = config;
        }

        public override float Width
        {
            get
            {
                if (widthCache < 0 || fontCache != Settings.fontSize || cacheMode != Prefs.TemperatureMode || cacheHeight != ExtendedToolbar.Height)
                {
                    GameFont font = Text.Font;
                    Text.Font = Settings.fontSize;
                    widthCache = (float)Math.Round(Text.CalcSize((1000f).ToStringTemperature()).x) + IconSize;
                    fontCache = Settings.fontSize;
                    cacheMode = Prefs.TemperatureMode;
                    cacheHeight = ExtendedToolbar.Height;
                    Text.Font = font;
                }
                return widthCache;
            }
        }

        public override bool FixedWidth => true;

        public override void OnGUI(Rect rect)
        {
            Rect innerRect = new Rect(rect);
            this.Margins(ref innerRect);
            ExtendedToolbar.DoWidgetBackground(innerRect);
            this.Padding(ref innerRect);

            WeatherDef weatherPerceived = Find.CurrentMap.weatherManager.CurWeatherPerceived;
            Texture2D icon = ModTextures.WeatherIcon(weatherPerceived.GetModExtension<WeatherDefExtension>()?.icon ?? "GD/UI/Icons/Weather/Unknown");

            Rect iconSpace;
            if (!weatherPerceived.description.NullOrEmpty())
                iconSpace = DrawIcon(icon, innerRect.x, innerRect.y, weatherPerceived.LabelCap + "\n" + weatherPerceived.description);
            else
                iconSpace = DrawIcon(icon, innerRect.x, innerRect.y, weatherPerceived.LabelCap);
            innerRect.x += iconSpace.width;
            innerRect.width -= iconSpace.width;

            WidgetRow row = new WidgetRow(innerRect.x, rect.y, UIDirection.RightThenDown, gap: ExtendedToolbar.interGap);
            string tempLabel = Mathf.Round(Find.World.tileTemperatures.GetOutdoorTemp(Find.CurrentMap.Tile)).ToStringTemperature("F0");

            Text.Anchor = TextAnchor.MiddleLeft;
            row.Label(tempLabel, innerRect.width, null, rect.height);
            Text.Anchor = TextAnchor.UpperLeft;
        }
    }
}