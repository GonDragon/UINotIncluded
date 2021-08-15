using RimWorld;
using Verse;
using harmony0;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(MainButtonsRoot), "MainButtonsOnGUI")]
    class MainButtonRootPatch
    {
        static void Postfix()
        {
            Log.Mesagge("UINotIncluded Postfix");
        }
    }
}