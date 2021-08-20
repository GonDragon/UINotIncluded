using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using HarmonyLib;

using UINotIncluded.Widget;

namespace UINotIncluded
{
    public static class UIManager
    {
        public static float ExtendedBarHeight => ExtendedToolbar.height;
        public static float ExtendedBarWidth => _width;

        [TweakValue("A.Extended Bar Width", 410f, 900f)]
        private static float _width = Math.Min(Math.Max(410f, (float)UI.screenWidth / 4), 900f);
        private static bool tabsOnTop = UINotIncludedSettings.tabsOnTop;

        public static void Before_MainUIOnGUI()
        {
            if (Find.CurrentMap == null || WorldRendererUtility.WorldRenderedNow || Find.UIRoot.screenshotMode.FiltersCurrentEvent) return;
            if (tabsOnTop != UINotIncludedSettings.tabsOnTop)
            {
                tabsOnTop = UINotIncludedSettings.tabsOnTop;
                Find.ColonistBar.MarkColonistsDirty();
            }
            if (Event.current.type == EventType.Layout) return;
            float posY = UINotIncludedSettings.tabsOnTop ? 0f : UI.screenHeight - ExtendedToolbar.height;
            ExtendedToolbar.ExtendedToolbarOnGUI(0, posY, _width);
        }
        public static void MainUIOnGUI()
        {
            if (Find.CurrentMap == null || WorldRendererUtility.WorldRenderedNow || Find.UIRoot.screenshotMode.FiltersCurrentEvent) return;
        }

        public static void After_MainUIOnGUI()
        {
            if (Find.CurrentMap == null || WorldRendererUtility.WorldRenderedNow || Find.UIRoot.screenshotMode.FiltersCurrentEvent) return;
        }
    }
}
