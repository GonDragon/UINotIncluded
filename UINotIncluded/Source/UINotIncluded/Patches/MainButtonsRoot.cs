using System;
using System.Collections.Generic;
using System.Linq;

using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(MainButtonsRoot), "MainButtonsOnGUI")]
    class MainButtonRootPatch
    {
        static void Postfix()
        {
            UIManager.MainUIOnGUI();
        }
    }

    [HarmonyPatch(typeof(MainButtonsRoot), "DoButtons")]
    class DoButtonsPatch
    {
        static bool Prefix(List<MainButtonDef> ___allButtonsInOrder)
        {
            var allButtonsInOrder = ___allButtonsInOrder;
            int height = 35;
            int posY = UINotIncludedSettings.tabsOnTop ? 0 : UI.screenHeight - height;

            float num1 = 0;
            for (int index = 0; index < allButtonsInOrder.Count; ++index)
            {
                if (allButtonsInOrder[index].buttonVisible)
                    num1 += allButtonsInOrder[index].minimized ? 0.5f : 1f;
            }
            GUI.color = Color.white;
            double spaceReserved = UIManager.extendedBarWidth;
            if (!UINotIncludedSettings.vanillaArchitect) spaceReserved += UIManager.archButtonWidth;
            int num2 = (int)(((double)UI.screenWidth - spaceReserved) / (double)num1);
            int num3 = num2 / 2;
            int lastIndex = allButtonsInOrder.FindLastIndex((Predicate<MainButtonDef>)(x => x.buttonVisible));
            int num4 = UINotIncludedSettings.barOnRight ? (UINotIncludedSettings.vanillaArchitect ? 0 : (int)UIManager.archButtonWidth) : (int)spaceReserved;
            for (int index = 0; index < allButtonsInOrder.Count; ++index)
            {
                if (allButtonsInOrder[index].buttonVisible)
                {
                    int num5 = allButtonsInOrder[index].minimized ? num3 : num2;
                    if (index == lastIndex)
                    {
                        num5 = UI.screenWidth - num4;
                        if (UINotIncludedSettings.barOnRight) num5 -= (int)UIManager.extendedBarWidth;
                    }                        
                    Rect rect = new Rect((float)num4, (float)posY, (float)num5, height);
                    allButtonsInOrder[index].Worker.DoButton(rect);
                    num4 += num5;
                }
            }
            return false;
        }
    }
}