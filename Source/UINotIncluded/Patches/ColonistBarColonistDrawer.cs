using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(ColonistBarColonistDrawer), "DrawColonist")]
    class DrawColonistPatch
    {
        static void Prefix(ref Rect rect)
        {
            if (UINotIncludedSettings.tabsOnTop) rect.y += UIManager.Instance.ExtendedBarHeight;
        }
    }
}
