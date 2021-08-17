using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using RimWorld;
using RimWorld.Planet;
using HarmonyLib;
using UnityEngine;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(GlobalControls), "GlobalControlsOnGUI")]
    class GlobalControlsOnGUIPatch
    {

    }
}
