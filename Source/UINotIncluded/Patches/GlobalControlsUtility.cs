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
        static bool DoPlaySettingsPatch(WidgetRow rowVisibility, bool worldView)
        {
            float borderGap = 4f;
            float gapY = UINotIncludedSettings.tabsOnTop ? UIManager.ExtendedBarHeight + borderGap : borderGap;
            rowVisibility.Init((float)UI.screenWidth - borderGap, gapY, UIDirection.LeftThenDown, 250f);
            Find.PlaySettings.DoPlaySettingsGlobalControls(rowVisibility, worldView);
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
