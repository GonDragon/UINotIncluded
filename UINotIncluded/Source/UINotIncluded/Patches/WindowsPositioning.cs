using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using UnityEngine;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(WorldInspectPane), "PaneTopY", MethodType.Getter)]
    class WorldInspectPanePatches
    {
        public static void Postfix(ref float __result)
        {
            if (Current.ProgramState == ProgramState.Playing && Settings.TabsOnTop) __result += 35;
        }
    }

    [HarmonyPatch(typeof(MainTabWindow_Inspect), "PaneTopY", MethodType.Getter)]
    class MainTabWindow_InspectPatches
    {
        public static void Postfix(ref float __result)
        {
            if (Settings.TabsOnTop) __result += 35;
        }
    }

    [HarmonyPatch(typeof(Verse.WindowStack), "ImmediateWindow")]
    class WindowStackPatch
    {
        public static void Prefix(int ID, ref Rect rect)
        {
            if (ID == 76136312)
            {
                if (Settings.togglersOnTop) rect.y += 75f;
                if (Settings.TabsOnTop) rect.y += UIManager.ExtendedBarHeight;
            }
        }
    }

    public static class MainTabWindowPatchHelper
    {

        public static void CenterRectOnScreen(ref Rect rect)
        {
            float newX = (float)Math.Floor(UI.screenWidth / 2f) - (float)Math.Floor(rect.width / 2f);
            float newY = (float)Math.Floor(UI.screenHeight / 2f) - (float)Math.Floor(rect.height / 2f);

            rect.x = newX;
            rect.y = newY;
        }
    }



    [HarmonyPatch(typeof(MainTabWindow), "SetInitialSizeAndPosition")]
    public class MainTabWindowPatch
    {
        public static void Postfix(ref Rect ___windowRect, Window __instance)
        {
            if (!Settings.TabsOnTop) return;
            Type windowType = __instance.GetType();

            if (windowType == typeof(MainTabWindow_Inspect))
            {
                ___windowRect.y += 35f;
                return;
            }

            if (windowType == typeof(MainTabWindow_Architect) || windowType == typeof(MainTabWindow_Research)) { ___windowRect.y = UI.screenHeight - ___windowRect.height; return; }

            MainTabWindowPatchHelper.CenterRectOnScreen(ref ___windowRect);
        }
    }

    [HarmonyPatch(typeof(ArchitectCategoryTab), "DoInfoBox")]
    public class ArchitectCategoryTabPatches
    {
        public static void Prefix(ref Rect infoRect)
        {
            if (Settings.TabsOnTop) infoRect.y += UIManager.ExtendedBarHeight;
        }
    }
}
