using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using UnityEngine;
using RimWorld.Planet;
using Verse;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(WorldInspectPane), "PaneTopY", MethodType.Getter)]
    class WorldInspectPanePatches
    {
        public static void Postfix(ref float __result)
        {
            if (Current.ProgramState == ProgramState.Playing && UINotIncludedSettings.tabsOnTop) __result += 35;
        }
    }
}
