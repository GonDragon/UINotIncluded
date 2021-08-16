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
    [HarmonyPatch(typeof(ColonistBarDrawLocsFinder), "GetDrawLoc")]
    class GetDrawLocPatch
    {
        static void Prefix(ref float groupStartY)
        {
            groupStartY += 35f;
        }
    }
}
