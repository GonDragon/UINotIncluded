using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace UINotIncluded
{
    [StaticConstructorOnStartup]
    public static class WidgetGetter
    {
        public static readonly Dictionary<string, Func<IEnumerable<Widget.Configs.ElementConfig>>> availableGetters;

        private static string selectedGetterName;
        private static Func<IEnumerable<Widget.Configs.ElementConfig>> selectedGetterFunction;

        public static string SelectedGetterName
        {
            get
            {
                if (selectedGetterName == null) SelectGetter("All");
                return selectedGetterName;
            }
        }
        public static IEnumerable<Widget.Configs.ElementConfig> AvailableSelectedWidgets
        {
            get
            {
                if (selectedGetterFunction == null) SelectGetter("All");
                return selectedGetterFunction();
            }
        }

        static WidgetGetter()
        {
            availableGetters = new Dictionary<string, Func<IEnumerable<Widget.Configs.ElementConfig>>>();
            availableGetters["Available Buttons"] = AllAvailableMainTabButtons;
            availableGetters["All"] = AllAvailableElements;
        }

        public static void SelectGetter(string name)
        {
            if (!availableGetters.ContainsKey(name))
            {
                UINI.Error("Invalid WidgetGetter name");
                return;
            }

            selectedGetterName = name;
            selectedGetterFunction = availableGetters[name];
        }

        public static IEnumerable<Widget.Configs.ElementConfig> AllAvailableMainTabButtons()
        {
            foreach (Widget.Configs.ElementConfig config in UINotIncluded.Widget.WidgetManager.MainTabButtons)
            {
                if (!config.Repeatable && (Settings.TopBarElements.Select(e => e.defName).Contains(config.defName) || Settings.BottomBarElements.Select(e => e.defName).Contains(config.defName))) continue;
                yield return config;
            }
        }

        public static IEnumerable<Widget.Configs.ElementConfig> AllAvailableElements()
        {
            return AllAvailableMainTabButtons();
        }
    }
}
