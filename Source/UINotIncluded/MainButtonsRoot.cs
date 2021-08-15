using RimWorld;
using Verse;
using HarmonyLib;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(MainButtonsRoot), "MainButtonsOnGUI")]
    class MainButtonRootPatch
    {
        static void Postfix()
        {
            Log.Message("UINotIncluded Postfix");
        }
    }
}