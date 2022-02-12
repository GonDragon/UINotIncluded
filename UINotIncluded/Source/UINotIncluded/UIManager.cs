using RimWorld.Planet;
using System.Collections.Generic;
using UINotIncluded.Widget;
using UnityEngine;
using Verse;

namespace UINotIncluded
{
    public static class UIManager
    {
        public static float ExtendedBarHeight => ExtendedToolbar.Height;
        public static float ExtendedBarWidth => ExtendedToolbar.Width;
        public static float ResourceGap => (Settings.vanillaAnimals ? (Settings.TabsOnTop ? ExtendedToolbar.Height : 0f) : animalsRow.FinalY + 26f);

        public static readonly float archButtonWidth = ExtendedToolbar.Height;

        public static bool toggleAltInspector = false;

        private static bool tabsOnTop = Settings.TabsOnTop;
        private static readonly WidgetRow animalsRow = new WidgetRow();
        private static readonly JobDesignatorBar JobsBar = new JobDesignatorBar();
        private static readonly AltInspectorManager altInspectorManager = new AltInspectorManager();

        private static bool updateToolbars = true;
        private static readonly List<ToolbarElement> topBarElements = new List<ToolbarElement>();
        private static readonly List<ToolbarElement> bottomBarElements = new List<ToolbarElement>();

        private static void CheckForUpdate()
        {
            if (updateToolbars)
            {
                topBarElements.Clear();
                foreach (Widget.Configs.ElementConfig config in Settings.TopBarElements)
                {
                    topBarElements.Add(new ToolbarElement(config));
                }

                bottomBarElements.Clear();
                foreach (Widget.Configs.ElementConfig config in Settings.BottomBarElements)
                {
                    bottomBarElements.Add(new ToolbarElement(config));
                }

                updateToolbars = false;
            }
        }

        public static void BarsOnGUI()
        {
            ExtendedToolbar.ExtendedToolbarOnGUI(Settings.TopBarElements, new Rect(0f, 0f, UI.screenWidth, ExtendedToolbar.Height));
            ExtendedToolbar.ExtendedToolbarOnGUI(Settings.BottomBarElements, new Rect(0f, UI.screenHeight - ExtendedToolbar.Height, UI.screenWidth, ExtendedToolbar.Height));
        }

        public static void Before_MainUIOnGUI()
        {
            if (tabsOnTop != Settings.TabsOnTop)
            {
                tabsOnTop = Settings.TabsOnTop;
                Find.ColonistBar.MarkColonistsDirty();
            }
        }

        public static void MainUIOnGUI()
        {
            float animalsY = Settings.TabsOnTop ? 13f + ExtendedToolbar.Height : 13f;
            if (Find.CurrentMap == null || WorldRendererUtility.WorldRenderedNow) return;
            altInspectorManager.AltInspectorOnGUI();
            if (!Settings.vanillaAnimals) AnimalButtons.AnimalButtonsOnGUI(animalsRow, 10f, animalsY);
            if (Settings.useDesignatorBar && Find.MainTabsRoot.OpenTab == null) JobsBar.JobDesignatorBarOnGUI();
        }

        public static void After_MainUIOnGUI()
        {
        }
    }
}