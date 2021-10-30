using HarmonyLib;
using RimWorld;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(MapInterface), "MapInterfaceOnGUI_BeforeMainTabs")]
    internal class MapInterfaceOnGUI_BeforeMainTabsPatch
    {
        public static void Prefix()
        {
            UIManager.Before_MainUIOnGUI();
        }
    }

    [HarmonyPatch(typeof(MapInterface), "MapInterfaceOnGUI_AfterMainTabs")]
    internal class MapInterfaceOnGUI_AfterMainTabsPatch
    {
        public static void Postfix()
        {
            UIManager.After_MainUIOnGUI();
        }
    }

    [HarmonyPatch(typeof(MainButtonsRoot), "MainButtonsOnGUI")]
    internal class MainButtonRootPatch
    {
        public static void Postfix()
        {
            UIManager.MainUIOnGUI();
        }
    }

    [HarmonyPatch(typeof(MainButtonsRoot), "DoButtons")]
    internal class DoButtonsPatch
    {
        public static bool Prefix()
        {
            UIManager.BarsOnGUI();
            return false;
        }
    }
}