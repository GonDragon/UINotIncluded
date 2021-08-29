using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using HarmonyLib;
using UnityEngine;
using RimWorld;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(PlaySettings), "DoPlaySettingsGlobalControls")]
    class DoPlaySettingsGlobalControlsPatch
    {
        static void Postfix(WidgetRow row, bool worldView)
        {
            if (worldView)
            {
                
            }
            else
            {
                row.ToggleableIcon(ref UINotIncludedSettings.altInspectActive, ModTextures.iconAltInspector, "UINotIncluded.Playsetting.altinspect.tooltip".Translate(), SoundDefOf.Mouseover_ButtonToggle);
            }
        }
    }
}
