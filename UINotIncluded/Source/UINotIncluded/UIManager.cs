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

        private static bool tabsOnTop = Settings.TabsOnTop;
        private static readonly WidgetRow animalsRow = new WidgetRow();
        private static readonly JobDesignatorBar JobsBar = new JobDesignatorBar();

        private static bool updateToolbars = true;
        private static readonly List<ToolbarElement> topBarElements = new List<ToolbarElement>();
        private static readonly List<ToolbarElement> bottomBarElements = new List<ToolbarElement>();

        private static Utility.VUIEhelper vuie;

        public static Utility.VUIEhelper Helper
        {
            get
            {
                if(vuie == null)
                {
                    vuie = new Utility.VUIEhelper();
                }
                return vuie;
            }
        }


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

        public static void VUIE_BarsOnGUI()
        {
            if (!VUIE.UIDefOf.UI_EditMode.Worker.Active)
            {
                BarsOnGUI();
                return;
            }
            ExtendedToolbar.VUIE_ExtendedToolbarOnGUI(Settings.TopBarElements, new Rect(0f, 0f, UI.screenWidth, ExtendedToolbar.Height), Helper);
            ExtendedToolbar.VUIE_ExtendedToolbarOnGUI(Settings.BottomBarElements, new Rect(0f, UI.screenHeight - ExtendedToolbar.Height, UI.screenWidth, ExtendedToolbar.Height), Helper);

            ((VUIE.DragDropManager<Widget.Configs.ElementConfig>)Helper.dragDropManager).DragDropOnGUI(element => UINI.Log(string.Format("Element {0} discarded from the bars.",element.SettingLabel)));

            UINI_Mod.settings.Write();
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
            if (!Settings.vanillaAnimals) AnimalButtons.AnimalButtonsOnGUI(animalsRow, 10f, animalsY);
            if (Settings.useDesignatorBar && Find.MainTabsRoot.OpenTab == null) JobsBar.JobDesignatorBarOnGUI();
        }

        public static void After_MainUIOnGUI()
        {
        }
    }
}