using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace UINotIncluded
{
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
            if (!UINotIncludedSettings.tabsOnTop) return;
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
}
