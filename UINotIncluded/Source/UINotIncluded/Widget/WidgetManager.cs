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
        public const string ALL = "Widgets and Buttons";
        public const string BUTTONS = "Available Buttons";
        public const string WIDGETS = "Available Widgets";

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
        public static readonly Dictionary<string, Func<IEnumerable<Widget.Configs.ButtonConfig>>> buttonGetters;

        private static string selectedGetterName;
        private static Func<IEnumerable<Widget.Configs.ElementConfig>> selectedGetterFunction;

        public static string SelectedGetterName
        {
            get
            {
                if (selectedGetterName == null) SelectGetter(ALL);
                return selectedGetterName;
            }
        }
        public static IEnumerable<Widget.Configs.ElementConfig> AvailableSelectedWidgets(bool allowAlreadyOnBars = false)
        {
            if (selectedGetterFunction == null) SelectGetter(ALL);

            foreach(Widget.Configs.ElementConfig config in selectedGetterFunction())
            {
                if (!allowAlreadyOnBars && !config.Repeatable && (Settings.TopBarElements.Exists(e => e.Equivalent(config)) || Settings.BottomBarElements.Exists(e => e.Equivalent(config)))) continue;
                yield return config;
            }
        }

        static WidgetManager()
        {
            availableGetters = new Dictionary<string, Func<IEnumerable<Widget.Configs.ElementConfig>>>();
            buttonGetters = new Dictionary<string, Func<IEnumerable<Configs.ButtonConfig>>>();
            AddButtonGetter(BUTTONS, AllMainButtons);
            AddGetter(WIDGETS, AllWidgets);
            AddGetter(ALL, AllElements);
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

        public static IEnumerable<Widget.Configs.ButtonConfig> AllMainButtons()
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

        public static void AddButtonGetter(string name, Func<IEnumerable<Widget.Configs.ButtonConfig>> getterFunction)
        {
            buttonGetters[name] = getterFunction;
            AddGetter(name, getterFunction);
        }
    }
}
