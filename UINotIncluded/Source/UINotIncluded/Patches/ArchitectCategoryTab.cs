using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using UnityEngine;
using RimWorld;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(ArchitectCategoryTab), "DoInfoBox")]
    public class ArchitectCategoryTabPatches
    {
        public static void Prefix(ref Rect infoRect)
        {
            if (UINotIncludedSettings.tabsOnTop) infoRect.y += UIManager.ExtendedBarHeight;
        }
    }
}
