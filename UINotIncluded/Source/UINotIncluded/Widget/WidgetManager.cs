using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace UINotIncluded.Widget
{
    [StaticConstructorOnStartup]
    public static class WidgetManager
    {
        const string all_name = "Widgets and Buttons";
        const string buttons_name = "Available Buttons";
        const string widgets_name = "Available Widgets";

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
                if (selectedGetterName == null) SelectGetter(all_name);
                return selectedGetterName;
            }
        }
        public static IEnumerable<Widget.Configs.ElementConfig> AvailableSelectedWidgets(bool allowAlreadyOnBars = false)
        {
            if (selectedGetterFunction == null) SelectGetter(all_name);

            foreach(Widget.Configs.ElementConfig config in selectedGetterFunction())
            {
                if (!allowAlreadyOnBars && !config.Repeatable && (Settings.TopBarElements.Contains(config) || Settings.BottomBarElements.Contains(config))) continue;
                yield return config;
            }
        }

        static WidgetManager()
        {
            availableGetters = new Dictionary<string, Func<IEnumerable<Widget.Configs.ElementConfig>>>();
            AddGetter(buttons_name, AllMainButtons);
            AddGetter(widgets_name, AllWidgets);
            AddGetter(all_name, AllElements);
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

        public static IEnumerable<Widget.Configs.ElementConfig> AllMainButtons()
        {
            foreach (Widget.Configs.ButtonConfig config in UINotIncluded.Widget.WidgetManager.MainTabButtons) yield return config;
        }

        public static IEnumerable<Widget.Configs.ElementConfig> AllWidgets()
        {
            foreach(WidgetDef widgetDef in DefDatabase<WidgetDef>.AllDefs)
            {
                Widget.Configs.ElementConfig config = widgetDef.GetNewConfig();
                yield return config;

            }
        }

        public static IEnumerable<Widget.Configs.ElementConfig> AllElements()
        {
            foreach (Widget.Configs.ElementConfig config in AllMainButtons()) yield return config;
            foreach (Widget.Configs.ElementConfig config in AllWidgets()) yield return config;
        }

        public static void AddGetter(string name, Func<IEnumerable<Widget.Configs.ElementConfig>> getterFunction)
        {
            availableGetters[name] = getterFunction;
        }
    }
}
