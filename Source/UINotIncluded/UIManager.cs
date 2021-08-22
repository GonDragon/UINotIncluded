using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

using UINotIncluded.Widget;

namespace UINotIncluded
{
    public static class UIManager
    {
        public static float ExtendedBarHeight => ExtendedToolbar.height;
        public static float ExtendedBarWidth => _width;

        private static readonly float _width = Math.Min(Math.Max(410f, (float)UI.screenWidth / 4), 900f);
        private static bool tabsOnTop = UINotIncludedSettings.tabsOnTop;
        private static bool architectVanilla = true;

        public static void Before_MainUIOnGUI()
        {
            if (tabsOnTop != UINotIncludedSettings.tabsOnTop)
            {
                tabsOnTop = UINotIncludedSettings.tabsOnTop;
                Find.ColonistBar.MarkColonistsDirty();
            }
            if(architectVanilla != UINotIncludedSettings.vanillaArchitect)
            {
                architectVanilla = UINotIncludedSettings.vanillaArchitect;
                SetArchitectVisibility(architectVanilla);
            }
        }
        public static void MainUIOnGUI()
        {
            float posY = UINotIncludedSettings.tabsOnTop ? 0f : UI.screenHeight - ExtendedToolbar.height;
            ExtendedToolbar.ExtendedToolbarOnGUI(0, posY, _width);
        }

        public static void After_MainUIOnGUI()
        {

        }

        private static void SetArchitectVisibility(bool visible)
        {
            DefDatabase<MainButtonDef>.GetNamed("Architect").buttonVisible = visible;
        }
    }
}
