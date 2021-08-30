using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using UnityEngine;

namespace UINotIncluded
{
    public class UINotIncludedSettings : ModSettings
    {
        public static bool tabsOnTop = true;
        public static bool barOnRight = false;
        public static bool designationsOnLeft = false;
        public static DateFormat dateFormat = DateFormat.ddmmmYYYY;
        public static bool altInspectActive = true;
        public static bool vanillaArchitect = false;
        public static bool vanillaAnimals = false;
        public static bool useDesignatorBar = true;

        public static List<String> hiddenDesignations = new List<String>();
        public static List<String> leftDesignations = new List<string> { "Forbid", "Uninstall", "Allow", "Claim" };
        public static List<String> mainDesignations = new List<string> { "Deconstruct", "Cancel", "Harvest", "Chop wood", "Mine" };
        public static List<String> rightDesignations = new List<string> { "Strip", "Open", "Smooth surface", "Tame", "Haul things", "Slaughter", "Cut plants", "Hunt" };

        public static bool initializedDesignations = false;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref tabsOnTop, "tabsOnTop", true);
            Scribe_Values.Look(ref barOnRight, "barOnRight", false);
            Scribe_Values.Look(ref designationsOnLeft, "designationsOnLeft", false);
            Scribe_Values.Look(ref altInspectActive, "altInspectActive", true);
            Scribe_Values.Look(ref dateFormat, "dateFormat", DateFormat.MMDDYYYY);
            Scribe_Values.Look(ref vanillaArchitect, "vanillaArchitect", false);
            Scribe_Values.Look(ref vanillaAnimals, "vanillaAnimals", false);
            Scribe_Values.Look(ref useDesignatorBar, "useDesignatorBar", true);
            Scribe_Values.Look(ref initializedDesignations, "initializedDesignations", false);

            Scribe_Collections.Look(ref hiddenDesignations, "hiddenDesignations", LookMode.Value);
            Scribe_Collections.Look(ref leftDesignations, "leftDesignations", LookMode.Value);
            Scribe_Collections.Look(ref mainDesignations, "mainDesignations", LookMode.Value);
            Scribe_Collections.Look(ref rightDesignations, "rightDesignations", LookMode.Value);

            base.ExposeData();
        }

        public static List<String> GetDesignationList(DesignationConfig list)
        {
            switch(list)
            {
                case DesignationConfig.hidden:
                    return hiddenDesignations;
                case DesignationConfig.left:
                    return leftDesignations;
                case DesignationConfig.main:
                    return mainDesignations;
                case DesignationConfig.right:
                    return rightDesignations;
                default:
                    throw new NotImplementedException();
            }
        }

        public static int GetDesignationRows(DesignationConfig list)
        {
            switch (list)
            {
                case DesignationConfig.hidden:
                    return 0;
                case DesignationConfig.left:
                    return 2;
                case DesignationConfig.main:
                    return 1;
                case DesignationConfig.right:
                    return 2;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void RestoreDesignationLists()
        {
            SetDefaultList(DesignationConfig.hidden);
            SetDefaultList(DesignationConfig.left);
            SetDefaultList(DesignationConfig.main);
            SetDefaultList(DesignationConfig.right);
            DesignatorManager.Update();
            UINotIncludedStatic.Log("DesignationConfigs default restored.");
        }

        private static void SetDefaultList(DesignationConfig designationList)
        {
            switch (designationList)
            {
                case DesignationConfig.hidden:
                    hiddenDesignations = new List<string>();
                    return;
                case DesignationConfig.left:
                    leftDesignations = new List<string> { "Forbid", "Uninstall", "Allow", "Claim" };
                    break;
                case DesignationConfig.main:
                    mainDesignations = new List<string> { "Mine", "Chop wood", "Harvest", "Cancel", "Deconstruct" };
                    break;
                case DesignationConfig.right:
                    rightDesignations = new List<string> { "Haul things", "Smooth surface", "Slaughter", "Cut plants", "Hunt", "Tame" };
                    break;
                default:
                    throw new NotImplementedException();

            }
        }

        public static List<Designator>[] GetDesignationConfigs()
        {
            List<Designator> avaibleDesignators = DefDatabase<DesignationCategoryDef>.GetNamed("Orders").AllResolvedDesignators;
            List<Designator> usedDesignators = new List<Designator>();
            List<Designator>[] arrayDesignatorConfigs = new List<Designator>[] { new List<Designator>(), new List<Designator>(), new List<Designator>(), new List<Designator>() };

            for (int i = 0; i < 4; i++)
            {
                foreach (String designatorName in GetDesignationList((DesignationConfig)i))
                {
                    bool found = false;
                    foreach (Designator designator in avaibleDesignators)
                    {
                        if (designator.Label == designatorName) { arrayDesignatorConfigs[i].Add(designator); usedDesignators.Add(designator); found = true; break; }
                    }
                    if (!found) UINotIncludedStatic.Warning(String.Format("The designation named '{0}' was not found.", designatorName));
                }
            }

            foreach (Designator designator in avaibleDesignators)
            {
                if (!usedDesignators.Contains(designator))
                {
                    arrayDesignatorConfigs[(int)DesignationConfig.hidden].Add(designator);
                    hiddenDesignations.Add(designator.Label);
                };
            }

            return arrayDesignatorConfigs;
        }
    }

    class UINotIncludedMod : Mod
    {
        public UINotIncludedSettings settings;

        public UINotIncludedMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<UINotIncludedSettings>();
            if (!UINotIncludedSettings.initializedDesignations)
            {
                UINotIncludedStatic.Log("DesigationConfigs never initialized. Initializing.");
                UINotIncludedSettings.RestoreDesignationLists();
                UINotIncludedSettings.initializedDesignations = true;
                settings.Write();
            }
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            float columnWidth = inRect.width / 2;
            float heigth = (float)Math.Floor(inRect.height / 3);
            Rect column1 = new Rect(inRect.x, inRect.y, columnWidth, heigth).ContractedBy(2f);
            Rect column2 = new Rect(inRect.x + columnWidth, inRect.y, columnWidth, heigth).ContractedBy(2f);
            Listing_Standard listingStandard = new Listing_Standard();
            
            listingStandard.Begin(column1);
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.tabsOnTop".Translate(), ref UINotIncludedSettings.tabsOnTop, "UINotIncluded.Setting.tabsOnTop.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.barOnRight".Translate(), ref UINotIncludedSettings.barOnRight, "UINotIncluded.Setting.barOnRight.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.vanillaArchitect".Translate(), ref UINotIncludedSettings.vanillaArchitect, "UINotIncluded.Setting.vanillaArchitect.Description".Translate());
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.vanillaAnimals".Translate(), ref UINotIncludedSettings.vanillaAnimals, "UINotIncluded.Setting.vanillaAnimals.Description".Translate());
            if (listingStandard.ButtonTextLabeled("UINotIncluded.Setting.dateFormat".Translate(), UINotIncludedSettings.dateFormat.ToStringHuman()))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                foreach (DateFormat dateFormat in Enum.GetValues(typeof(DateFormat)))
                {
                    DateFormat localFormat = dateFormat;
                    options.Add(new FloatMenuOption(localFormat.ToStringHuman(), (Action)(() => UINotIncludedSettings.dateFormat = dateFormat)));
                }
                Find.WindowStack.Add((Window)new FloatMenu(options));
            }
            listingStandard.End();

            listingStandard.Begin(column2);
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.useDesignatorBar".Translate(), ref UINotIncludedSettings.useDesignatorBar, "UINotIncluded.Setting.useDesignatorBar.Description".Translate());
            if (UINotIncludedSettings.useDesignatorBar) listingStandard.CheckboxLabeled("UINotIncluded.Setting.designationsOnLeft".Translate(), ref UINotIncludedSettings.designationsOnLeft, "UINotIncluded.Setting.designationsOnLeft.Description".Translate());
            listingStandard.End();
            if (UINotIncludedSettings.useDesignatorBar) DoJobBarConfigurationWidget(new Rect(inRect.x, inRect.y + heigth, inRect.width, inRect.height - heigth));
            base.DoSettingsWindowContents(inRect);
        }

        private void DoJobBarConfigurationWidget(Rect rect)
        {
            float columnWidth = rect.width / 4;
            float curY = rect.y;

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(rect.x + (float)Math.Floor(rect.width / 4), curY, (float)Math.Floor(rect.width / 2), 25f), new GUIContent("UINotIncluded.Setting.DesignatorBar.Description".Translate()));
            Text.Anchor = TextAnchor.UpperLeft;
            curY += 25f;
            if (Widgets.ButtonText(new Rect(rect.x+ (float)Math.Floor(rect.width / 4), curY, (float)Math.Floor(rect.width / 2), 25f), "Restore to default")) UINotIncludedSettings.RestoreDesignationLists();
            curY += 25f;
            for (int i = 0; i < 4; i++)
            {
                DoJobsDesignatorColumn(new Rect(rect.x + columnWidth * i, curY, columnWidth, rect.height - 25f).ContractedBy(3f), ((DesignationConfig)i).ToStringHuman(), (DesignationConfig)i);
            }
        }

        private void DoJobsDesignatorColumn(Rect rect, String label, DesignationConfig designation)
        {
            List<String> elements = UINotIncludedSettings.GetDesignationList(designation);
            int rows = UINotIncludedSettings.GetDesignationRows(designation);
            int rowLength = (int)Math.Ceiling((float)elements.Count() / rows);
            float buttonHeight = 30f;
            float buttonContract = 4f;

            float curY = rect.y;
            Widgets.Label(new Rect(rect.x, curY, rect.width, buttonHeight), new GUIContent(label));
            curY += 25f;
            Widgets.DrawMenuSection(new Rect(rect.x, curY, rect.width, rect.height - (curY - rect.y)));

            ScrollInstance scroll = ScrollManager.GetInstance((int)designation);
            Rect scrollviewRect = new Rect(rect.x, curY, rect.width, rect.height - (curY - rect.y)).ContractedBy(3f);
            Rect scrollviewInRect = new Rect(0, 0, scrollviewRect.width -20f, buttonHeight * elements.Count());

            Widgets.BeginScrollView(scrollviewRect, ref scroll.pos, scrollviewInRect);
            curY = 0;
            bool shouldMove = false; String moveElement = null; ButtonArrowAction moveDirection = ButtonArrowAction.none;
            
            for(int i = 0; i < elements.Count(); i++)
            {
                String element = elements[i];
                ButtonArrowAction result = CustomButtons.ButtonLabelWithArrows(new Rect(0, curY, scrollviewInRect.width, buttonHeight).ContractedBy(buttonContract), element);
                if (result != ButtonArrowAction.none)
                {
                    shouldMove = true;
                    moveElement = element;
                    moveDirection = result;
                }
                curY += (buttonHeight - (float)Math.Floor(buttonContract / 2));
                if ((i + 1) % rowLength == 0 && (i+1) < elements.Count() && rows > 1)
                {
                    Text.Anchor = TextAnchor.MiddleCenter;
                    Text.Font = GameFont.Tiny;
                    Widgets.Label(new Rect(0, curY, scrollviewInRect.width, buttonHeight), "Row Jump");
                    Widgets.DrawLineHorizontal(0, curY + buttonHeight, scrollviewInRect.width);
                    curY += buttonHeight + buttonContract;
                    Text.Anchor = TextAnchor.UpperLeft;
                    Text.Font = GameFont.Small;
                }
            }

            if (shouldMove) DoMove(moveElement, moveDirection, designation);
            Widgets.EndScrollView();
        }

        private void DoMove(String element, ButtonArrowAction direction, DesignationConfig current)
        {
            List<String> currentList = UINotIncludedSettings.GetDesignationList(current);
            int curIndex = currentList.IndexOf(element);

            switch (direction)
            {
                case ButtonArrowAction.up:
                    if (curIndex == 0) return;
                    currentList.RemoveAt(curIndex);
                    currentList.Insert(curIndex - 1, element);
                    break;
                case ButtonArrowAction.down:
                    if (curIndex == currentList.Count() - 1) return;
                    currentList.RemoveAt(curIndex);
                    currentList.Insert(curIndex + 1, element);
                    break;
                case ButtonArrowAction.left:
                    if (current == DesignationConfig.hidden) return;
                    UINotIncludedSettings.GetDesignationList(current - 1).Add(element);
                    currentList.RemoveAt(curIndex);
                    break;
                case ButtonArrowAction.right:
                    if (current == DesignationConfig.right) return;
                    UINotIncludedSettings.GetDesignationList(current + 1).Add(element);
                    currentList.RemoveAt(curIndex);
                    break;
                default:
                    throw new NotImplementedException();
            }
            DesignatorManager.Update();
        }

        public override string SettingsCategory()
        {
            return UINotIncludedStatic.Name;
        }
    }
}
