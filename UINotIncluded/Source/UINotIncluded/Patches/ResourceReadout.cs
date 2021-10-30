using HarmonyLib;
using RimWorld;
using UnityEngine;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(ResourceReadout), "DoReadoutSimple"), HarmonyPriority(Priority.High)]
    internal class DoReadoutSimplePatch
    {
        public static void Prefix(ref Rect rect)
        {
            rect.y += UIManager.ResourceGap;
        }
    }

    [HarmonyPatch(typeof(ResourceReadout), "DoReadoutCategorized"), HarmonyPriority(Priority.High)]
    internal class DoReadoutCategorizedPatch
    {
        public static void Prefix(ref Rect rect)
        {
            rect.y += UIManager.ResourceGap;
        }
    }
}