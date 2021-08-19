using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using HarmonyLib;

using UINotIncluded.Widget;

namespace UINotIncluded
{
    public static class UIManager
    {
        public static float ExtendedBarHeight => ExtendedToolbar.height;
        private static bool tabsOnTop = UINotIncludedSettings.tabsOnTop;

        public static void Before_MainUIOnGUI() 
        {
            if (tabsOnTop != UINotIncludedSettings.tabsOnTop)
            {
                tabsOnTop = UINotIncludedSettings.tabsOnTop;
                Find.ColonistBar.MarkColonistsDirty();
            }
            if (Event.current.type == EventType.Layout) return;
            float posY = UINotIncludedSettings.tabsOnTop ? 0f : UI.screenHeight - ExtendedToolbar.height;
            ExtendedToolbar.ExtendedToolbarOnGUI(0, posY, (float)UI.screenWidth / 4);
        }
        public static void MainUIOnGUI()
        {
            
        }

        public static void After_MainUIOnGUI()
        {

        }
    }
}
