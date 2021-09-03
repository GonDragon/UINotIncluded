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
        public static float ResourceGap => (vanillaAnimals ? ExtendedToolbar.height : animalsRow.FinalY + 26f);

        public static float ExtendedBarWidth => currentSize;
        public static readonly float archButtonWidth = ExtendedToolbar.height;

        private static float currentSize = Math.Min(Math.Max(450f + 60f * (float)UINotIncludedSettings.fontSize, (float)UI.screenWidth / 4), 900f);


        private static bool tabsOnTop = UINotIncludedSettings.tabsOnTop;
        private static bool vanillaArchitect = true;
        private static bool vanillaAnimals = true;
        private static readonly WidgetRow animalsRow = new WidgetRow();
        private static readonly JobDesignatorBar JobsBar = new JobDesignatorBar();

        public static void SetCorrectWidth(GameFont fontsize)
        {
            float minSize = 450f + 60f * (float)fontsize;
            currentSize = Math.Min(Math.Max(minSize, (float)UI.screenWidth / 4), 900f);
        }

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
            float toolbarX;
            if (UINotIncludedSettings.barOnRight) { toolbarX = UI.screenWidth - ExtendedBarWidth; } else { toolbarX = UINotIncludedSettings.vanillaArchitect ? 0 : archButtonWidth; };
            float animalsY = UINotIncludedSettings.tabsOnTop ? 13f + ExtendedToolbar.height : 13f;

            ExtendedToolbar.ExtendedToolbarOnGUI(toolbarX, toolbarY, ExtendedBarWidth);
            if (!vanillaArchitect) ArchitectMenuButton.ArchitectButtonOnGUI(0f, toolbarY, archButtonWidth);
            if (Find.CurrentMap == null || WorldRendererUtility.WorldRenderedNow) return;
            if(!vanillaAnimals) AnimalButtons.AnimalButtonsOnGUI(animalsRow, 10f, animalsY);
            if (UINotIncludedSettings.useDesignatorBar && Find.MainTabsRoot.OpenTab == null) JobsBar.JobDesignatorBarOnGUI();
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
