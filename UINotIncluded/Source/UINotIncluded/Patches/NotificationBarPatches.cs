using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace UINotIncluded
{

    [HarmonyPatch(typeof(GlobalControls))]
    class GlobalControlsPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("TemperatureString"), HarmonyPriority(Priority.Low)]
        static bool TemperatureString_Patch()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(GlobalControlsUtility))]
    class GlobalControlsUtilityPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoPlaySettings)), HarmonyPriority(Priority.Low)]
        static bool DoPlaySettingsPatch(WidgetRow rowVisibility, bool worldView, ref float curBaseY)
        {
            
            if (!UINotIncludedSettings.tabsOnTop) curBaseY -= UIManager.ExtendedBarHeight;
            if (UINotIncludedSettings.useDesignatorBar && !UINotIncludedSettings.designationsOnLeft) curBaseY -= 88f;
            if (worldView && !UINotIncludedSettings.togglersOnTop) curBaseY -= 35;
            float borderGap = 4f;
            float initialY = UINotIncludedSettings.togglersOnTop ? (UINotIncludedSettings.tabsOnTop ? UIManager.ExtendedBarHeight + borderGap : borderGap) : curBaseY;
            rowVisibility.Init((float)UI.screenWidth - borderGap, initialY, UINotIncludedSettings.togglersOnTop ? UIDirection.LeftThenDown : UIDirection.LeftThenUp, UINotIncludedSettings.togglersOnTop ? 250f : 180f);
            Find.PlaySettings.DoPlaySettingsGlobalControls(rowVisibility, worldView);
            if (!UINotIncludedSettings.togglersOnTop) curBaseY = rowVisibility.FinalY;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoTimespeedControls)), HarmonyPriority(Priority.Low)]
        static bool DoTimespeedControls_Patch(ref float curBaseY)
        {
            curBaseY += 4f;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoDate)), HarmonyPriority(Priority.Low)]
        static bool DoDate_Patch(ref float curBaseY)
        {
            curBaseY += 42f;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoRealtimeClock)), HarmonyPriority(Priority.Low)]
        static bool DoRealtimeClock_Patch(ref float curBaseY)
        {
            curBaseY += 4f;
            return false;
        }
    }

    [HarmonyPatch(typeof(WeatherManager))]
    class WeatherManagerPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(WeatherManager.DoWeatherGUI)), HarmonyPriority(Priority.Low)]
        static bool DoWeatherGUI_Patch(Rect rect)
        {
            rect.height = 0;
            return false;
        }
    }
}
