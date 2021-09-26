﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace UINotIncluded
{
    public class Settings : ModSettings
    {
        public static bool barOnRight = false;
        public static DateFormat dateFormat = DateFormat.ddmmmYYYY;
        public static bool designationsOnLeft = false;
        public static GameFont fontSize = GameFont.Tiny;
        public static List<String> hiddenDesignations;
        public static bool initializedDesignations = false;
        public static List<String> leftDesignations;
        public static List<String> mainDesignations;
        public static List<String> rightDesignations;
        public static bool tabsOnTop = true;
        public static bool togglersOnTop = true;
        public static bool useDesignatorBar = true;
        public static bool vanillaAnimals = false;
        public static bool vanillaArchitect = false;
        public static bool vanillaReadout;
        private static readonly Dictionary<string, Designator> _avaibleDesignators = new Dictionary<string, Designator>();

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

        public static void InitialiceDesignators()
        {
            if (_avaibleDesignators.Count() != 0) return;

            foreach (Designator designator in DefDatabase<DesignationCategoryDef>.GetNamed("Orders").AllResolvedDesignators)
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

        public override void ExposeData()
        {
            Scribe_Values.Look(ref tabsOnTop, "tabsOnTop", true);
            Scribe_Values.Look(ref togglersOnTop, "togglersOnTop", true);
            Scribe_Values.Look(ref barOnRight, "barOnRight", false);
            Scribe_Values.Look(ref designationsOnLeft, "designationsOnLeft", false);
            Scribe_Values.Look(ref dateFormat, "dateFormat", DateFormat.MMDDYYYY);
            Scribe_Values.Look(ref vanillaArchitect, "vanillaArchitect", false);
            Scribe_Values.Look(ref vanillaAnimals, "vanillaAnimals", false);
            Scribe_Values.Look(ref vanillaReadout, "vanillaReadout", false);
            Scribe_Values.Look(ref useDesignatorBar, "useDesignatorBar", true);
            Scribe_Values.Look(ref initializedDesignations, "initializedDesignations", false);
            Scribe_Values.Look(ref fontSize, "fontSize", GameFont.Tiny);

            Scribe_Collections.Look(ref hiddenDesignations, "hiddenDesignations", LookMode.Value);
            Scribe_Collections.Look(ref leftDesignations, "leftDesignations", LookMode.Value);
            Scribe_Collections.Look(ref mainDesignations, "mainDesignations", LookMode.Value);
            Scribe_Collections.Look(ref rightDesignations, "rightDesignations", LookMode.Value);

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
        public Settings settings;
        private readonly SettingPages settingPages = new SettingPages();

        public UINI_Mod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            float pageTittleHeight = 26f;
            float pageTittleWidth = (float)Math.Floor(inRect.width / 3);

            Rect contentRect = new Rect(inRect.x, inRect.y + pageTittleHeight, inRect.width, inRect.height - pageTittleHeight);

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(inRect.x + pageTittleWidth + pageTittleHeight, inRect.y, pageTittleWidth - 2 * pageTittleHeight, pageTittleHeight), settingPages.label);
            if (Widgets.ButtonImage(new Rect(inRect.x + pageTittleWidth, inRect.y, pageTittleHeight, pageTittleHeight), ModTextures.chevronLeft)) settingPages.Prev();
            if (Widgets.ButtonImage(new Rect(inRect.x + 2 * pageTittleWidth - pageTittleHeight, inRect.y, pageTittleHeight, pageTittleHeight), ModTextures.chevronRight)) settingPages.Next();
            Text.Anchor = TextAnchor.UpperLeft;

            settingPages.DoPage(contentRect);
            base.DoSettingsWindowContents(inRect);
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
            float heigth = (float)Math.Floor(inRect.height / 3);
            Rect column1 = new Rect(columnWidth - columnWidth/2, inRect.y, columnWidth, heigth).ContractedBy(2f);
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(column1);
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.tabsOnTop".Translate(), ref Settings.tabsOnTop, "UINotIncluded.Setting.tabsOnTop.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.togglersOnTop".Translate(), ref Settings.togglersOnTop, "UINotIncluded.Setting.togglersOnTop.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.barOnRight".Translate(), ref Settings.barOnRight, "UINotIncluded.Setting.barOnRight.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.vanillaArchitect".Translate(), ref Settings.vanillaArchitect, "UINotIncluded.Setting.vanillaArchitect.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.vanillaAnimals".Translate(), ref Settings.vanillaAnimals, "UINotIncluded.Setting.vanillaAnimals.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.vanillaReadout".Translate(), ref Settings.vanillaReadout, "UINotIncluded.Setting.vanillaReadout.Description".Translate());
            if (listingStandard.ButtonTextLabeled("UINotIncluded.Setting.dateFormat".Translate(), Settings.dateFormat.ToStringHuman()))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                foreach (DateFormat dateFormat in Enum.GetValues(typeof(DateFormat)))
                {
                    DateFormat localFormat = dateFormat;
                    options.Add(new FloatMenuOption(localFormat.ToStringHuman(), (Action)(() => Settings.dateFormat = dateFormat)));
                }
                Find.WindowStack.Add((Window)new FloatMenu(options));
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
            listingStandard.End();
        }

        private void DoDesignatorPage(Rect inRect)
        {
            float columnWidth = inRect.width / 2;
            float heigth = (float)Math.Floor(inRect.height / 4);
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

            DragManager.hoveringOver = null;
            for (int i = 0; i < 4; i++)
            {
                Widget.CustomLists.Draggable(((DesignationConfig)i).ToStringHuman(), new Rect(rect.x + columnWidth * i, curY, columnWidth, rect.height - 25f).ContractedBy(3f), designation_lists[i], (object designator) => { return ((Designator)designator).defaultLabel; });
            }
            DragManager.DrawGhost();
            if (DragManager.DraggStops())
            {
                if (DragManager.Dragging)
                {
                    DragManager.MoveDragged();
                    DragManager.UseDragged();
                    DesignatorManager.Push();
                }
            }
        }

        private struct Page
        {
            public Action<Rect> action;
            public string label;
        }
    }
}