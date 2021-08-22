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
        public static float ResourceGap => (vanillaAnimals ? ExtendedToolbar.height : animalsRow.FinalY + 26f);

        private static readonly float _width = Math.Min(Math.Max(410f, (float)UI.screenWidth / 4), 900f);
        private static bool tabsOnTop = UINotIncludedSettings.tabsOnTop;
        private static bool vanillaArchitect = true;
        private static bool vanillaAnimals = true;
        private static readonly WidgetRow animalsRow = new WidgetRow();

        public static void Before_MainUIOnGUI()
        {
            if (tabsOnTop != UINotIncludedSettings.tabsOnTop)
            {
                tabsOnTop = UINotIncludedSettings.tabsOnTop;
                Find.ColonistBar.MarkColonistsDirty();
            }
            if(vanillaArchitect != UINotIncludedSettings.vanillaArchitect)
            {
                vanillaArchitect = UINotIncludedSettings.vanillaArchitect;
                SetMainbuttonVisibility("Architect", vanillaArchitect);
            }
            if (vanillaAnimals != UINotIncludedSettings.vanillaAnimals)
            {
                vanillaAnimals = UINotIncludedSettings.vanillaAnimals;
                SetMainbuttonVisibility("Animals", vanillaAnimals);
                SetMainbuttonVisibility("Wildlife", vanillaAnimals);
            }
        }
        public static void MainUIOnGUI()
        {
            float toolbarY = UINotIncludedSettings.tabsOnTop ? 0f : UI.screenHeight - ExtendedToolbar.height;
            float animalsY = UINotIncludedSettings.tabsOnTop ? 13f + ExtendedToolbar.height : 13f;

            ExtendedToolbar.ExtendedToolbarOnGUI(0, toolbarY, _width);
            if(!vanillaAnimals) AnimalButtons.AnimalButtonsOnGUI(animalsRow, 10f, animalsY);
        }

        public static void After_MainUIOnGUI()
        {

        }

        private static void SetMainbuttonVisibility(string name, bool visible)
        {
            DefDatabase<MainButtonDef>.GetNamed(name).buttonVisible = visible;
        }
    }
}
