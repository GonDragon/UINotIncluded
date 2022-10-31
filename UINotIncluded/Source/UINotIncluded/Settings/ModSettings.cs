﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UINotIncluded.Widget;
using UnityEngine;
using Verse;

namespace UINotIncluded
{
    public class Settings : ModSettings
    {
        public static bool designationsOnLeft = false;
        public static GameFont fontSize;
        public static List<String> hiddenDesignations;
        public static bool initializedDefaultBar;
        public static List<String> leftDesignations;
        public static List<String> mainDesignations;
        public static List<String> rightDesignations;
        public static bool togglersOnTop = false;
        public static bool useDesignatorBar = false;
        public static bool vanillaAnimals = true;
        public static bool settingsChecked;
        public static bool centeredWindows = false;

        public static bool vanillaReadout = true;
        public static bool vanillaControlSpeed = true;
        public static bool vanillaDate = true;
        public static bool vanillaRealtime = true;
        public static bool vanillaWeather = true;
        public static bool vanillaTemperature = true;
        
        public static bool forceCustomTab = true;

        public static float barsHeight = 35f;

        public static string lastVersion;

        private static readonly Dictionary<string, Designator> _avaibleDesignators = new Dictionary<string, Designator>();

        public static bool TabsOnTop => Settings.TopBarElements.Count() > 0;
        public static bool TabsOnBottom => Settings.BottomBarElements.Count() > 0;

        private static List<Widget.Configs.ElementConfig> topBar;
        private static List<Widget.Configs.ElementConfig> bottomBar;

        private static BarStyle _barStyle;
        private static Type _barStyleType;

        public static BarStyle BarStyle
        {
            get
            {
                if (_barStyle == null)
                {
                    if (_barStyleType == null) _barStyleType = typeof(BarStyle_VanillaBluePlus);
                    _barStyle = (BarStyle)Activator.CreateInstance(_barStyleType);
                }
                return _barStyle;
            }

            set
            {
                _barStyle = value;
                _barStyleType = value.GetType();
            }
        }

        public static List<Widget.Configs.ElementConfig> TopBarElements
        {
            get
            {
                if (topBar == null) topBar = new List<Widget.Configs.ElementConfig>();
                return topBar;
            }
        }

        public static List<Widget.Configs.ElementConfig> BottomBarElements
        {
            get
            {
                if (bottomBar == null) bottomBar = new List<Widget.Configs.ElementConfig>();
                return bottomBar;
            }
        }

        public static List<Designator>[] GetDesignationConfigs()
        {
            List<Designator>[] arrayDesignatorConfigs = new List<Designator>[4];

            for (int i = 0; i < 4; i++)
            {
                arrayDesignatorConfigs[i] = GetDesignationList((DesignationConfig)i);
            }
            return arrayDesignatorConfigs;
        }

        public static List<Designator> GetDesignationList(DesignationConfig list)
        {
            switch (list)
            {
                case DesignationConfig.hidden:
                    return StringToDesignation(hiddenDesignations);

                case DesignationConfig.left:
                    return StringToDesignation(leftDesignations);

                case DesignationConfig.main:
                    return StringToDesignation(mainDesignations);

                case DesignationConfig.right:
                    return StringToDesignation(rightDesignations);

                default:
                    throw new NotImplementedException();
            }
        }

        public static List<Designator> CachedDesignators;
        public static void InitialiceDesignators()
        {
            if (_avaibleDesignators.Count() != 0) return;


            if(CachedDesignators == null)
            {
                CachedDesignators = new List<Designator>();
                foreach (Designator designator in DefDatabase<DesignationCategoryDef>.GetNamed("Orders").AllResolvedDesignators)
                {
                    CachedDesignators.Add(designator);
                }
            }

            foreach (Designator designator in CachedDesignators)
            {
                _avaibleDesignators[designator.GetType().ToString()] = designator;
            }

            foreach (string key in _avaibleDesignators.Keys)
            {
                if (!(leftDesignations.Contains(key) || mainDesignations.Contains(key) || rightDesignations.Contains(key) || hiddenDesignations.Contains(key))) hiddenDesignations.Add(key);
            }
        }

        public static void RestoreDesignationLists()
        {
            SetDefaultList(DesignationConfig.hidden);
            SetDefaultList(DesignationConfig.left);
            SetDefaultList(DesignationConfig.main);
            SetDefaultList(DesignationConfig.right);
            _avaibleDesignators.Clear();
            InitialiceDesignators();
            DesignatorManager.Pull();
            UINI.Log("DesignationConfigs default restored.");
        }

        public static void RestoreDefaultMainBar()
        {
            TopBarElements.Clear();
            BottomBarElements.Clear();

            foreach (Widget.Configs.ButtonConfig element in WidgetManager.AllMainButtons())
            {
                BottomBarElements.Add(element);
            }
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref togglersOnTop, "togglersOnTop", true);
            Scribe_Values.Look(ref designationsOnLeft, "designationsOnLeft", false);

            Scribe_Values.Look(ref vanillaAnimals, "vanillaAnimals", false);
            Scribe_Values.Look(ref centeredWindows, "centeredWindows", false);

            Scribe_Values.Look(ref settingsChecked, "settingsChecked", false);

            Scribe_Values.Look(ref vanillaReadout, "vanillaReadout", false);
            Scribe_Values.Look(ref vanillaControlSpeed, "vanillaControlSpeed", false);
            Scribe_Values.Look(ref vanillaDate, "vanillaDate", false);
            Scribe_Values.Look(ref vanillaRealtime, "vanillaRealtime", false);
            Scribe_Values.Look(ref vanillaWeather, "vanillaWeather", false);
            Scribe_Values.Look(ref vanillaTemperature, "vanillaTemperature", false);

            Scribe_Values.Look(ref useDesignatorBar, "useDesignatorBar", true);
            Scribe_Values.Look(ref initializedDefaultBar, "initializedDefaultBar", false);
            Scribe_Values.Look(ref fontSize, "fontSize", GameFont.Tiny);

            Scribe_Values.Look(ref _barStyleType, "StyleType", typeof(BarStyle_RustyOrange));

            Scribe_Collections.Look(ref hiddenDesignations, "hiddenDesignations", LookMode.Value);
            Scribe_Collections.Look(ref leftDesignations, "leftDesignations", LookMode.Value);
            Scribe_Collections.Look(ref mainDesignations, "mainDesignations", LookMode.Value);
            Scribe_Collections.Look(ref rightDesignations, "rightDesignations", LookMode.Value);

            Scribe_Collections.Look(ref topBar, "top", LookMode.Deep);
            Scribe_Collections.Look(ref bottomBar, "bottom", LookMode.Deep);

            Scribe_Values.Look(ref lastVersion, "lastVersion");
            Scribe_Values.Look(ref barsHeight, "barsHeight", 35f);

            Scribe_Values.Look(ref forceCustomTab, "forceCustomTab", true);

            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                Scribe_Collections.Look(ref Utility.Deprecated.DeprecationManager.DeprecatedTopBar, "topBar", LookMode.Deep);
                Scribe_Collections.Look(ref Utility.Deprecated.DeprecationManager.DeprecatedBottomBar, "bottomBar", LookMode.Deep);
            }

            base.ExposeData();
        }

        private static void SetDefaultList(DesignationConfig designationList)
        {
            switch (designationList)
            {
                case DesignationConfig.hidden:
                    hiddenDesignations = new List<string>();
                    return;

                case DesignationConfig.left:
                    leftDesignations = new List<string> { "RimWorld.Designator_Forbid", "RimWorld.Designator_Uninstall", "RimWorld.Designator_Unforbid", "RimWorld.Designator_Claim" };
                    break;

                case DesignationConfig.main:
                    mainDesignations = new List<string> { "RimWorld.Designator_Mine", "RimWorld.Designator_PlantsHarvestWood", "RimWorld.Designator_PlantsHarvest", "RimWorld.Designator_Cancel", "RimWorld.Designator_Deconstruct" };
                    break;

                case DesignationConfig.right:
                    rightDesignations = new List<string> { "RimWorld.Designator_Haul", "RimWorld.Designator_SmoothSurface", "RimWorld.Designator_Slaughter", "RimWorld.Designator_PlantsCut", "RimWorld.Designator_Hunt", "RimWorld.Designator_Tame" };
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private static List<Designator> StringToDesignation(List<string> classes)
        {
            InitialiceDesignators();
            List<Designator> result = new List<Designator>();

            foreach (string designator_class in classes)
            {
                if (_avaibleDesignators.ContainsKey(designator_class)) result.Add(_avaibleDesignators[designator_class]);
            }

            return result;
        }

        public static void UpdateDesignations(List<Designator> designations, DesignationConfig list)
        {
            switch (list)
            {
                case DesignationConfig.hidden:
                    Settings.UpdateList(designations, hiddenDesignations);
                    break;

                case DesignationConfig.left:
                    Settings.UpdateList(designations, leftDesignations);
                    break;

                case DesignationConfig.main:
                    Settings.UpdateList(designations, mainDesignations);
                    break;

                case DesignationConfig.right:
                    Settings.UpdateList(designations, rightDesignations);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private static void UpdateList(List<Designator> designations, List<String> names)
        {
            names.Clear();
            foreach (Designator designation in designations) names.Add(designation.GetType().ToString());
        }
    }

    internal class UINI_Mod : Mod
    {
        public static Settings settings;
        private readonly SettingPages settingPages = new SettingPages();

        public UINI_Mod(ModContentPack content) : base(content)
        {
            settings = GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Settings.settingsChecked = true;
            float pageTittleHeight = 26f;
            float pageTittleWidth = (float)Math.Floor(inRect.width / 3);

            Rect contentRect = new Rect(inRect.x, inRect.y + pageTittleHeight, inRect.width, inRect.height - pageTittleHeight - 30f);

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(inRect.x + pageTittleWidth + pageTittleHeight, inRect.y, pageTittleWidth - 2 * pageTittleHeight, pageTittleHeight), settingPages.label);
            if (Widgets.ButtonImage(new Rect(inRect.x + pageTittleWidth, inRect.y, pageTittleHeight, pageTittleHeight), ModTextures.chevronLeft)) settingPages.Prev();
            if (Widgets.ButtonImage(new Rect(inRect.x + 2 * pageTittleWidth - pageTittleHeight, inRect.y, pageTittleHeight, pageTittleHeight), ModTextures.chevronRight)) settingPages.Next();
            Text.Anchor = TextAnchor.UpperLeft;

            settingPages.DoPage(contentRect);
            base.DoSettingsWindowContents(inRect);
        }

        public override void WriteSettings()
        {
            settingPages.cacheAvaibleElements = null;
            base.WriteSettings();
        }

        public override string SettingsCategory()
        {
            return UINI.Name;
        }
    }

    internal class SettingPages
    {
        public string label;
        private readonly List<Page> pages;
        private int current;
        public List<Widget.Configs.ElementConfig> cacheAvaibleElements;

        public SettingPages()
        {
            pages = new List<Page>
            {
                new Page
                {
                    label = "General",
                    action = inRect => DoGeneralPage(inRect)
                },
                new Page
                {
                    label = "Designator",
                    action = inRect => DoDesignatorPage(inRect)
                },
                new Page
                {
                    label = "Toolbars",
                    action = inRect => DoToolbarDraggables(inRect)
                }
            };

            current = 0;
            label = pages[current].label;
        }

        public void DoPage(Rect inRect)
        {
            pages[current].action(inRect);
        }

        public void Next()
        {
            current++;
            if (current == pages.Count()) current = 0;
            label = pages[current].label;
        }

        public void Prev()
        {
            current--;
            if (current < 0) current = pages.Count() - 1;
            label = pages[current].label;
        }

        private void DoGeneralPage(Rect inRect)
        {
            float columnWidth = inRect.width / 2;
            float heigth = inRect.height;
            Rect column1 = new Rect(columnWidth - columnWidth / 2, inRect.y, columnWidth, heigth).ContractedBy(2f);
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(column1);
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.togglersOnTop".Translate(), ref Settings.togglersOnTop, "UINotIncluded.Setting.togglersOnTop.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.vanillaAnimals".Translate(), ref Settings.vanillaAnimals, "UINotIncluded.Setting.vanillaAnimals.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.centeredWindows".Translate(), ref Settings.centeredWindows, "UINotIncluded.Setting.centeredWindows.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.forceCustomTab".Translate(), ref Settings.forceCustomTab, "UINotIncluded.Setting.forceCustomTab.Description".Translate());

            listingStandard.CheckboxLabeled("UINotIncluded.Setting.vanillaReadout".Translate(), ref Settings.vanillaReadout, "UINotIncluded.Setting.vanillaReadout.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.vanillaRealtime".Translate(), ref Settings.vanillaRealtime, "UINotIncluded.Setting.vanillaRealtime.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.vanillaWeather".Translate(), ref Settings.vanillaWeather, "UINotIncluded.Setting.vanillaWeather.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.vanillaTemperature".Translate(), ref Settings.vanillaTemperature, "UINotIncluded.Setting.vanillaTemperature.Description".Translate());

            listingStandard.CheckboxLabeled("UINotIncluded.Setting.vanillaDate".Translate(), ref Settings.vanillaDate, "UINotIncluded.Setting.vanillaDate.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.vanillaControlSpeed".Translate(), ref Settings.vanillaControlSpeed, "UINotIncluded.Setting.vanillaControlSpeed.Description".Translate());


            listingStandard.Gap();

            if(listingStandard.ButtonTextLabeled("UINotIncluded.Setting.barHeight".Translate(Settings.barsHeight), "Default".Translate()))
            {
                Settings.barsHeight = 35f;
                if (Find.CurrentMap != null) Find.ColonistBar.MarkColonistsDirty();
            }
            float newHeight = listingStandard.Slider(Settings.barsHeight, 25f, 50f);
            if(newHeight != Settings.barsHeight)
            {
                Settings.barsHeight = newHeight;
                if(Find.CurrentMap != null) Find.ColonistBar.MarkColonistsDirty();
            }

            if (listingStandard.ButtonTextLabeled("UINotIncluded.Setting.fontSize".Translate(), Settings.fontSize.ToString()))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                foreach (GameFont font in Enum.GetValues(typeof(GameFont)))
                {
                    options.Add(new FloatMenuOption(font.ToString(), (Action)(() => { Settings.fontSize = font; })));
                }
                Find.WindowStack.Add((Window)new FloatMenu(options));
            }

            if (listingStandard.ButtonTextLabeled("UINotIncluded.Setting.style".Translate(), Settings.BarStyle.Name))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                foreach (Type styleType in new Type[] { typeof(BarStyle_RustyOrange), typeof(BarStyle_VanillaBlue), typeof(BarStyle_VanillaBluePlus) })
                {
                    BarStyle style = (BarStyle)Activator.CreateInstance(styleType);
                    options.Add(new FloatMenuOption(style.Name, (Action)(() => Settings.BarStyle = style)));
                }
                Find.WindowStack.Add((Window)new FloatMenu(options));
            }

            listingStandard.End();
        }

        private void DoDesignatorPage(Rect inRect)
        {
            float columnWidth = inRect.width / 2;
            float heigth = 30f;
            Rect column1 = new Rect(inRect.x, inRect.y, columnWidth, heigth).ContractedBy(2f);
            Rect column2 = new Rect(inRect.x + columnWidth, inRect.y, columnWidth, heigth).ContractedBy(2f);
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(column1);
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.useDesignatorBar".Translate(), ref Settings.useDesignatorBar, "UINotIncluded.Setting.useDesignatorBar.Description".Translate());
            listingStandard.End();
            listingStandard.Begin(column2);
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.designationsOnLeft".Translate(), ref Settings.designationsOnLeft, "UINotIncluded.Setting.designationsOnLeft.Description".Translate());
            listingStandard.End();
            DoJobBarConfigurationWidget(new Rect(inRect.x, inRect.y + heigth, inRect.width, inRect.height - heigth));
        }

        private void DoJobBarConfigurationWidget(Rect rect)
        {
            float columnWidth = rect.width / 4;
            float curY = rect.y;
            List<Designator>[] designation_lists = DesignatorManager.GetDesignationConfigs();

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(rect.x + (float)Math.Floor(rect.width / 4), curY, (float)Math.Floor(rect.width / 2), 25f), new GUIContent("UINotIncluded.Setting.DesignatorBar.Description".Translate()));
            Text.Anchor = TextAnchor.UpperLeft;
            curY += 25f;
            if (Widgets.ButtonText(new Rect(rect.x + (float)Math.Floor(rect.width / 4), curY, (float)Math.Floor(rect.width / 2), 25f), "Restore to default")) Settings.RestoreDesignationLists();
            curY += 25f;

            DragManager<Designator> manager = new DragManager<Designator>(
                OnUpdate: DesignatorManager.Push,
                GetLabel: (Designator designator) => { return designator.defaultLabel; });

            DragMemory.hoveringOver = null;
            for (int i = 0; i < 4; i++)
            {
                Widget.CustomLists.Draggable<Designator>(((DesignationConfig)i).ToStringHuman(), new Rect(rect.x + columnWidth * i, curY, columnWidth, rect.height - 25f).ContractedBy(3f), designation_lists[i], (Designator designator) => { return ((Designator)designator).defaultLabel; }, manager);
            }
            manager.Update();
        }

        private void DoToolbarDraggables(Rect rect)
        {
            float columnWidth = rect.width / 3;
            float curY = rect.y;

            if (cacheAvaibleElements == null) cacheAvaibleElements = WidgetManager.AvailableSelectedWidgets().ToList();

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(rect.x + (float)Math.Floor(rect.width / 4), curY, (float)Math.Floor(rect.width / 2), 25f), new GUIContent("UINotIncluded.Setting.Toolbars.Description".Translate()));
            Text.Anchor = TextAnchor.UpperLeft;
            curY += 25f;
            if (Widgets.ButtonText(new Rect(rect.x + (float)Math.Floor(rect.width / 4), curY, (float)Math.Floor(rect.width / 2), 25f), "Restore to default"))
            {
                Settings.RestoreDefaultMainBar();
                UpdateCache();
            }
            curY += 25f;

            DragManager<Widget.Configs.ElementConfig> manager = new DragManager<Widget.Configs.ElementConfig>(
                OnUpdate: () => UpdateCache(),
                GetLabel: (Widget.Configs.ElementConfig element) => { return element.SettingLabel; },
                OnClick: (Widget.Configs.ElementConfig element) =>
                {
                    if (element.Configurable) return element.Worker.OpenConfigWindow;
                    return null;
                });

            Rect selectTypeRect = new Rect(rect.x, curY, columnWidth, 30f);
            if (Widgets.ButtonText(selectTypeRect.ContractedBy(2f), WidgetManager.SelectedGetterName))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                foreach (String widgetGetter in WidgetManager.availableGetters.Keys)
                {
                    options.Add(new FloatMenuOption(widgetGetter, (Action)(() => { WidgetManager.SelectGetter(widgetGetter); UpdateCache(); })));
                }
                Find.WindowStack.Add((Window)new FloatMenu(options));
            }

            DragMemory.hoveringOver = null;

            Widget.CustomLists.Draggable<Widget.Configs.ElementConfig>("Available", new Rect(rect.x, curY + selectTypeRect.height, columnWidth, rect.height - 25f - selectTypeRect.height).ContractedBy(3f), cacheAvaibleElements, (Widget.Configs.ElementConfig element) => { return element.SettingLabel; }, manager, false);
            Widget.CustomLists.Draggable<Widget.Configs.ElementConfig>("Top Bar", new Rect(rect.x + columnWidth, curY, columnWidth, rect.height - 25f).ContractedBy(3f), Settings.TopBarElements, (Widget.Configs.ElementConfig element) => { return element.SettingLabel; }, manager);
            Widget.CustomLists.Draggable<Widget.Configs.ElementConfig>("Bottom Bar", new Rect(rect.x + columnWidth * 2, curY, columnWidth, rect.height - 25f).ContractedBy(3f), Settings.BottomBarElements, (Widget.Configs.ElementConfig element) => { return element.SettingLabel; }, manager);

            manager.Update();
        }

        private void UpdateCache()
        {
            cacheAvaibleElements.Clear();
            foreach (Widget.Configs.ElementConfig element in WidgetManager.AvailableSelectedWidgets()) cacheAvaibleElements.Add(element);
        }

        private struct Page
        {
            public Action<Rect> action;
            public string label;
        }
    }
}