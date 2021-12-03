using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace UINotIncluded.Widget
{
    [StaticConstructorOnStartup]
    public static class WidgetManager
    {
        public static IEnumerable<Configs.ButtonConfig> MainTabButtons
        {
            get
            {
                foreach (MainButtonDef button in DefDatabase<MainButtonDef>.AllDefs.OrderBy(def => def.order))
                {
                    if (button.defName == "Inspect") continue;
                    Configs.ButtonConfig config = new Configs.ButtonConfig(button);
                    config.RefreshIcon();
                    yield return config;
                }
            }        
        }

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

        static WidgetManager()
        {
            availableGetters = new Dictionary<string, Func<IEnumerable<Widget.Configs.ElementConfig>>>();
            AddGetter("Available Buttons", AllAvailableMainTabButtons);
            AddGetter("Available Widgets", AllAvailableWidgets);
            AddGetter("All", AllAvailableElements);
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
            foreach (Widget.Configs.ButtonConfig config in UINotIncluded.Widget.WidgetManager.MainTabButtons)
            {
                if (!config.Repeatable && (Settings.TopBarElements.Contains(config) || Settings.BottomBarElements.Contains(config))) continue;
                yield return config;
            }
        }

        public static IEnumerable<Widget.Configs.ElementConfig> AllAvailableWidgets()
        {
            foreach(WidgetDef widgetDef in DefDatabase<WidgetDef>.AllDefs)
            {
                Widget.Configs.ElementConfig config = widgetDef.GetNewConfig();
                if (!config.Repeatable && (Settings.TopBarElements.Contains(config) || Settings.BottomBarElements.Contains(config))) continue;
                yield return config;
            }
        }

        public static IEnumerable<Widget.Configs.ElementConfig> AllAvailableElements()
        {
            foreach (Widget.Configs.ElementConfig config in AllAvailableMainTabButtons()) yield return config;
            foreach (Widget.Configs.ElementConfig config in AllAvailableWidgets()) yield return config;
        }

        public static void AddGetter(string name, Func<IEnumerable<Widget.Configs.ElementConfig>> getterFunction)
        {
            availableGetters[name] = getterFunction;
        }
    }
}
