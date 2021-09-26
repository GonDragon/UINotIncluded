﻿using System;
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
        public static float ExtendedBarHeight => ExtendedToolbar.Height;
        public static float ExtendedBarWidth => ExtendedToolbar.Width;
        public static float ResourceGap => (vanillaAnimals ? ExtendedToolbar.Height : animalsRow.FinalY + 26f);

        public static readonly float archButtonWidth = ExtendedToolbar.Height;

        public static bool toggleAltInspector = false;


        private static bool tabsOnTop = Settings.tabsOnTop;
        private static bool vanillaArchitect = true;
        private static bool vanillaAnimals = true;
        private static readonly WidgetRow animalsRow = new WidgetRow();
        private static readonly JobDesignatorBar JobsBar = new JobDesignatorBar();
        private static readonly AltInspectorManager altInspectorManager = new AltInspectorManager();

        public static void Before_MainUIOnGUI()
        {
            if (tabsOnTop != Settings.tabsOnTop)
            {
                tabsOnTop = Settings.tabsOnTop;
                Find.ColonistBar.MarkColonistsDirty();
            }
            if(vanillaArchitect != Settings.vanillaArchitect)
            {
                vanillaArchitect = Settings.vanillaArchitect;
                SetMainbuttonVisibility("Architect", vanillaArchitect);
            }
            if (vanillaAnimals != Settings.vanillaAnimals)
            {
                vanillaAnimals = Settings.vanillaAnimals;
                SetMainbuttonVisibility("Animals", vanillaAnimals);
                SetMainbuttonVisibility("Wildlife", vanillaAnimals);
            }
        }
        public static void MainUIOnGUI()
        {
            float toolbarY = Settings.tabsOnTop ? 0f : UI.screenHeight - ExtendedToolbar.Height;
            float toolbarX;
            if (Settings.barOnRight) { toolbarX = UI.screenWidth - ExtendedBarWidth; } else { toolbarX = Settings.vanillaArchitect ? 0 : archButtonWidth; };
            float animalsY = Settings.tabsOnTop ? 13f + ExtendedToolbar.Height : 13f;

            ExtendedToolbar.ExtendedToolbarOnGUI(toolbarX, toolbarY);
            if (!vanillaArchitect) ArchitectMenuButton.ArchitectButtonOnGUI(0f, toolbarY, archButtonWidth);
            if (Find.CurrentMap == null || WorldRendererUtility.WorldRenderedNow) return;
            altInspectorManager.AltInspectorOnGUI();
            if (!vanillaAnimals) AnimalButtons.AnimalButtonsOnGUI(animalsRow, 10f, animalsY);
            if (Settings.useDesignatorBar && Find.MainTabsRoot.OpenTab == null) JobsBar.JobDesignatorBarOnGUI();
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
