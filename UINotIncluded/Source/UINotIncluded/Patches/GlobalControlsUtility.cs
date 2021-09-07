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
    [HarmonyPatch(typeof(GlobalControlsUtility))]
    class GlobalControlsUtilityPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoPlaySettings))]
        static bool DoPlaySettingsPatch(WidgetRow rowVisibility, bool worldView, ref float curBaseY)
        {
            if (worldView && !UINotIncludedSettings.togglersOnTop) curBaseY -= 35;
            float borderGap = 4f;
            float initialY = UINotIncludedSettings.togglersOnTop ? (UINotIncludedSettings.tabsOnTop ? UIManager.ExtendedBarHeight + borderGap : borderGap) : curBaseY;
            rowVisibility.Init((float)UI.screenWidth - borderGap, initialY, UINotIncludedSettings.togglersOnTop ? UIDirection.LeftThenDown : UIDirection.LeftThenUp, UINotIncludedSettings.togglersOnTop ? 250f : 180f);
            Find.PlaySettings.DoPlaySettingsGlobalControls(rowVisibility, worldView);
            if (!UINotIncludedSettings.togglersOnTop) curBaseY = rowVisibility.FinalY;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoTimespeedControls))]
        static bool DoTimespeedControlsPatch()
        {
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoDate))]
        static bool DoDatePatch()
        {
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoRealtimeClock))]
        static bool DoRealtimeClockPatch()
        {
            return false;
        }
    }
}
