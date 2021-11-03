using RimWorld;
using RimWorld.Planet;
using UINotIncluded.Widget;
using UnityEngine;
using Verse;

namespace UINotIncluded
{
    public static class UIManager
    {
        public static float ExtendedBarHeight => ExtendedToolbar.Height;
        public static float ExtendedBarWidth => ExtendedToolbar.Width;
        public static float ResourceGap => (vanillaAnimals ? (Settings.TabsOnTop ? ExtendedToolbar.Height : 0f) : animalsRow.FinalY + 26f);

        public static readonly float archButtonWidth = ExtendedToolbar.Height;

        public static bool toggleAltInspector = false;

        private static bool tabsOnTop = Settings.TabsOnTop;
        private static bool vanillaAnimals = true;
        private static readonly WidgetRow animalsRow = new WidgetRow();
        private static readonly JobDesignatorBar JobsBar = new JobDesignatorBar();
        private static readonly AltInspectorManager altInspectorManager = new AltInspectorManager();

        internal static void BarsOnGUI()
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
            if (vanillaAnimals != Settings.vanillaAnimals)
            {
                vanillaAnimals = Settings.vanillaAnimals;
                SetMainbuttonVisibility("Animals", vanillaAnimals);
                SetMainbuttonVisibility("Wildlife", vanillaAnimals);
            }
        }

        public static void MainUIOnGUI()
        {
            //float toolbarY = Settings.tabsOnTop ? 0f : UI.screenHeight - ExtendedToolbar.Height;
            //float toolbarX;
            //if (Settings.barOnRight) { toolbarX = UI.screenWidth - ExtendedBarWidth; } else { toolbarX = Settings.vanillaArchitect ? 0 : archButtonWidth; };
            float animalsY = Settings.TabsOnTop ? 13f + ExtendedToolbar.Height : 13f;
            //if (!vanillaArchitect) ArchitectMenuButton.ArchitectButtonOnGUI(0f, toolbarY, archButtonWidth);
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