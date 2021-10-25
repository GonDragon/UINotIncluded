using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace UINotIncluded
{

    [HarmonyPatch(typeof(GlobalControls))]
    class GlobalControlsPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("TemperatureString"), HarmonyPriority(Priority.High)]
        public static bool TemperatureString_Patch()
        {
            return Settings.vanillaWeather;
        }
    }

    [HarmonyPatch(typeof(GlobalControlsUtility))]
    class GlobalControlsUtilityPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoPlaySettings)), HarmonyPriority(Priority.Low)]
        static bool DoPlaySettingsPatch(WidgetRow rowVisibility, bool worldView, ref float curBaseY)
        {
            
            if (Settings.TabsOnBottom) curBaseY -= UIManager.ExtendedBarHeight;
            if (Settings.useDesignatorBar && !Settings.designationsOnLeft && !worldView) curBaseY -= 88f;
            if (Settings.togglersOnTop) curBaseY += 35;
            float borderGap = 4f;
            float initialY = Settings.togglersOnTop ? (Settings.TabsOnTop ? UIManager.ExtendedBarHeight + borderGap : borderGap) : curBaseY;
            rowVisibility.Init((float)UI.screenWidth - borderGap, initialY, Settings.togglersOnTop ? UIDirection.LeftThenDown : UIDirection.LeftThenUp, Settings.togglersOnTop ? 250f : 180f);
            Find.PlaySettings.DoPlaySettingsGlobalControls(rowVisibility, worldView);
            if (!Settings.togglersOnTop) curBaseY = rowVisibility.FinalY;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoTimespeedControls)), HarmonyPriority(Priority.Low)]
        static bool DoTimespeedControls_Patch(ref float curBaseY)
        {
            if (Settings.vanillaControlSpeed) return true;
            curBaseY += 4f;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoDate)), HarmonyPriority(Priority.Low)]
        static bool DoDate_Patch()
        {
            return Settings.vanillaDate;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoDate))]
        static void DoDate_PostifxPatch(ref float curBaseY)
        {
            if (!Settings.vanillaWeather) curBaseY += 50;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoRealtimeClock)), HarmonyPriority(Priority.Low)]
        static bool DoRealtimeClock_Patch(ref float curBaseY)
        {
            if (Settings.vanillaRealtime)return true;
            curBaseY += 4f;
            return false;
        }
    }

    [HarmonyPatch(typeof(WeatherManager))]
    class WeatherManagerPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(WeatherManager.DoWeatherGUI))]
        static bool DoWeatherGUI_PrefixPatch()
        {
            return Settings.vanillaWeather;
        }
    }

    [HarmonyPatch(typeof(PlaySettings), "DoPlaySettingsGlobalControls")]
    class DoPlaySettingsGlobalControlsPatch
    {
        public static void Postfix(WidgetRow row, bool worldView)
        {
            if (worldView)
            {

            }
            else
            {
                row.ToggleableIcon(ref UIManager.toggleAltInspector, ModTextures.iconAltInspector, "UINotIncluded.Playsetting.altinspect.tooltip".Translate(), SoundDefOf.Mouseover_ButtonToggle);
            }
        }
    }
}
