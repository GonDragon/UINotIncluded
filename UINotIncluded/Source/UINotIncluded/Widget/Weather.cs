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
    public class Weather : ExtendedWidget
    {
        public override float MinimunWidth => 45f;

        public override float MaximunWidth => 100f;

        public override void OnGUI(Rect rect)
        {

            ExtendedToolbar.DoToolbarBackground(rect);
            Rect space = rect.ContractedBy(ExtendedToolbar.padding);

            WeatherDef weatherPerceived = Find.CurrentMap.weatherManager.CurWeatherPerceived;
            Texture2D icon = ModTextures.WeatherIcon(weatherPerceived.GetModExtension<WeatherDefExtension>().icon);

            Rect iconSpace;
            if (!weatherPerceived.description.NullOrEmpty())
                iconSpace = DrawIcon(icon,space.x, weatherPerceived.LabelCap + "\n" + weatherPerceived.description);
            else
                iconSpace = DrawIcon(icon, space.x, weatherPerceived.LabelCap);
            space.x += iconSpace.width;
            space.width -= iconSpace.width;

            WidgetRow row = new WidgetRow(space.x, space.y, UIDirection.RightThenDown, gap: ExtendedToolbar.interGap);
            float temp = Mathf.RoundToInt(GenTemperature.CelsiusTo(Find.World.tileTemperatures.GetOutdoorTemp(Find.CurrentMap.Tile), Prefs.TemperatureMode));

            row.Label(temp.ToString() + new string[] { "°C", "°F", "°K" }[(int)Prefs.TemperatureMode], space.width, null, space.height);
        }
    }
}
