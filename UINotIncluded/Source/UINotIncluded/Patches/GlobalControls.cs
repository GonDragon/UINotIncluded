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
using System.Reflection;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(GlobalControls), "GlobalControlsOnGUI")]
    class GlobalControlsOnGUIPatch
    {
        static bool Prefix(GlobalControls __instance, WidgetRow ___rowVisibility)
        {
            if (Event.current.type == EventType.Layout)
                return false;            

            MethodInfo DoCountdownTimer = AccessTools.Method(typeof(GlobalControls), "DoCountdownTimer");

            float posX = (float)UI.screenWidth - 200f;
            float posY = (float)UI.screenHeight - 20f;

            if (!UINotIncludedSettings.tabsOnTop) posY -= UIManager.ExtendedBarHeight;
            if (UINotIncludedSettings.useDesignatorBar && !UINotIncludedSettings.designationsOnLeft) posY -= 80f;

            //GenUI.DrawTextWinterShadow(new Rect((float)(UI.screenWidth - 270), (float)(UI.screenHeight - 450), 270f, 450f));
            
            posY -= 4f;
            GlobalControlsUtility.DoPlaySettings(___rowVisibility, false, ref posY);
            posY -= 4f;

            float width = 154f;
            float height = Find.CurrentMap.gameConditionManager.TotalHeightAt(width);
            Rect rect3 = new Rect((float)UI.screenWidth - width, posY - height, width, height);
            Find.CurrentMap.gameConditionManager.DoConditionsUI(rect3);
            
            posY -= rect3.height;
            TimedDetectionRaids component = Find.CurrentMap.Parent.GetComponent<TimedDetectionRaids>();
            if (component != null && component.NextRaidCountdownActiveAndVisible)
            {
                posY -= 26f;
                Rect rect4 = new Rect(posX, posY, 193f, 26f);
                Text.Anchor = TextAnchor.MiddleRight;
                TimedDetectionRaids timedForcedExit = component;
                DoCountdownTimer.Invoke(__instance, new object[] { rect4, timedForcedExit });
                Text.Anchor = TextAnchor.UpperLeft;
            }
            posY -= 10;
            Find.LetterStack.LettersOnGUI(posY);
            return false;
        }
    }
}
