using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using RimWorld;
using HarmonyLib;
using UnityEngine;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(ResourceReadout), "DoReadoutSimple")]
    class DoReadoutSimplePatch
    {
        static void Prefix(ref Rect rect)
        {
            rect.y += UIManager.Instance.ClockHeight;
        }
    }

    [HarmonyPatch(typeof(ResourceReadout), "DoReadoutCategorized")]
    class DoReadoutCategorizedPatch
    {
        static void Prefix(ref Rect rect)
        {
            rect.y += UIManager.Instance.ClockHeight;
        }
    }
}
