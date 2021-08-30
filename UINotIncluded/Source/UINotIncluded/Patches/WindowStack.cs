using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using Verse;
using UnityEngine;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(Verse.WindowStack), "ImmediateWindow")]
    class WindowStackPatch
    {
        public static void Prefix(int ID, ref Rect rect)
        {
            if (ID == 76136312)
            {
                rect.y += 75f;
                if(UINotIncludedSettings.tabsOnTop) rect.y += UIManager.ExtendedBarHeight;
            }
        }
    }
}
