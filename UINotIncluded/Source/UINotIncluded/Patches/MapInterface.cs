using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using RimWorld;
using HarmonyLib;
using RimWorld.Planet;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(MapInterface), "MapInterfaceOnGUI_BeforeMainTabs")]
    class MapInterfaceOnGUI_BeforeMainTabsPatch
    {
        static void Prefix()
        {
            if (!(Find.CurrentMap == null) && (!WorldRendererUtility.WorldRenderedNow))
            {
                UIManager.Before_MainUIOnGUI();
            }
        }
    }

    [HarmonyPatch(typeof(MapInterface), "MapInterfaceOnGUI_AfterMainTabs")]
    class MapInterfaceOnGUI_AfterMainTabsPatch
    {
        static void Postfix()
        {
            UIManager.After_MainUIOnGUI();
        }
    }
}
