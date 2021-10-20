using System;
using System.Collections.Generic;
using System.Linq;

using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(MapInterface), "MapInterfaceOnGUI_BeforeMainTabs")]
    class MapInterfaceOnGUI_BeforeMainTabsPatch
    {
        public static void Prefix()
        {
            UIManager.Before_MainUIOnGUI();
        }
    }

    [HarmonyPatch(typeof(MapInterface), "MapInterfaceOnGUI_AfterMainTabs")]
    class MapInterfaceOnGUI_AfterMainTabsPatch
    {
        public static void Postfix()
        {
            UIManager.After_MainUIOnGUI();
        }
    }

    [HarmonyPatch(typeof(MainButtonsRoot), "MainButtonsOnGUI")]
    class MainButtonRootPatch
    {
        public static void Postfix()
        {
            UIManager.MainUIOnGUI();
        }
    }

    [HarmonyPatch(typeof(MainButtonsRoot), "DoButtons")]
    class DoButtonsPatch
    {
        public static bool Prefix()
        {
            UIManager.BarsOnGUI();
            return false;
        }
    }
}