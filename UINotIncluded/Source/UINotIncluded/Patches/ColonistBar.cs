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
    [HarmonyPatch(typeof(ColonistBar), "CheckRecacheEntries")]
    class CheckRecacheEntriesPatch
    {
        public static void Prefix(in bool ___entriesDirty,out bool __state)
        {
            __state = ___entriesDirty;
        }

        public static void Postfix(List<Vector2> ___cachedDrawLocs, bool __state)
        {
            if (__state && Settings.tabsOnTop)
            {
                Vector2[] copy = new Vector2[___cachedDrawLocs.Count];
                ___cachedDrawLocs.CopyTo(copy);
                ___cachedDrawLocs.Clear();
                foreach (Vector2 vector in copy) ___cachedDrawLocs.Add(new Vector2(vector.x, vector.y + UIManager.ExtendedBarHeight));
            }
        }
    }
}
