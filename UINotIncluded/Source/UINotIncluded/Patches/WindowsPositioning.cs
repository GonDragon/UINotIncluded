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
            if (Current.ProgramState == ProgramState.Playing && !Settings.TabsOnBottom) __result += 35;
        }
    }

    [HarmonyPatch(typeof(MainTabWindow_Inspect), "PaneTopY", MethodType.Getter)]
    class MainTabWindow_InspectPatches
    {
        public static void Postfix(ref float __result)
        {
            if (!Settings.TabsOnBottom) __result += 35;
        }
    }

    [HarmonyPatch(typeof(Verse.WindowStack), "ImmediateWindow")]
    class WindowStackPatch
    {
        public static void Prefix(int ID, ref Rect rect)
        {
            if (ID == 76136312) //Learning Readout
            {
                if (Settings.togglersOnTop) rect.y += 75f;
                if (Settings.TabsOnTop) rect.y += UIManager.ExtendedBarHeight;
            }

            if (ID == 1593759361) //Debug Toolbar
            {
                if (Settings.TabsOnTop) rect.y += UIManager.ExtendedBarHeight;
            }
        }
    }

    public static class MainTabWindowPatchHelper
    {

        public static void CenterRectOnScreen(ref Rect rect)
        {
            CenterXOnScreen(ref rect);
            CenterYOnScreen(ref rect);
        }

        public static void CenterXOnScreen(ref Rect rect) => rect.x = (float)Math.Floor(UI.screenWidth / 2f) - (float)Math.Floor(rect.width / 2f);

        public static void CenterYOnScreen(ref Rect rect) => rect.y = (float)Math.Floor(UI.screenHeight / 2f) - (float)Math.Floor(rect.height / 2f);
    }



    [HarmonyPatch(typeof(MainTabWindow), "SetInitialSizeAndPosition")]
    public class MainTabWindowPatch
    {
        public static void Postfix(ref Rect ___windowRect, Window __instance)
        {
            Type windowType = __instance.GetType();

            float maxHeight = UI.screenHeight;
            if (Settings.TabsOnTop) maxHeight -= UIManager.ExtendedBarHeight;
            if (Settings.TabsOnBottom) maxHeight -= UIManager.ExtendedBarHeight;

            ___windowRect.height = Math.Min(___windowRect.height,maxHeight);

            if (windowType == typeof(MainTabWindow_Inspect) || windowType == typeof(MainTabWindow_Architect) || windowType == typeof(MainTabWindow_Research))
            {
                if(!Settings.TabsOnBottom) ___windowRect.y += 35f;
                return;
            }

            if(windowType == typeof(MainTabWindow_Animals) || windowType == typeof(MainTabWindow_Wildlife) || windowType == typeof(MainTabWindow_Ideos))
            {
                MainTabWindowPatchHelper.CenterYOnScreen(ref ___windowRect);
                return;
            }

            MainTabWindowPatchHelper.CenterRectOnScreen(ref ___windowRect);
        }
    }

    [HarmonyPatch(typeof(ArchitectCategoryTab), "DoInfoBox")]
    public class ArchitectCategoryTabPatches
    {
        public static void Prefix(ref Rect infoRect)
        {
            if (!Settings.TabsOnBottom) infoRect.y += UIManager.ExtendedBarHeight;
        }
    }
}
