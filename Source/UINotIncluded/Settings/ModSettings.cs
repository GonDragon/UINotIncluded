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
        public static DateFormat dateFormat = DateFormat.ddmmmYYYY;
        public static bool altInspectActive = true;
        public static bool vanillaArchitect = false;
        public static bool vanillaAnimals = false;

        public static List<String> hiddenDesignations = new List<String>();
        public static List<String> leftDesignations = new List<string> { "Forbid", "Uninstall", "Allow", "Claim" };
        public static List<String> mainDesignations = new List<string> { "Harvest", "Deconstruct", "Cancel", "Chop wood", "Mine" };
        public static List<String> rightDesignations = new List<string> { "Strip", "Open", "Smooth surface", "Tame", "Haul things", "Slaughter", "Cut plants", "Hunt" };

        public static bool initializedDesignations = false;
        public static readonly List<String>[] designationConfigs = new List<String>[] { hiddenDesignations, leftDesignations, mainDesignations, rightDesignations };

        public override void ExposeData()
        {
            Scribe_Values.Look(ref tabsOnTop, "tabsOnTop", false);
            Scribe_Values.Look(ref altInspectActive, "altInspectActive", true);
            Scribe_Values.Look(ref dateFormat, "dateFormat", DateFormat.MMDDYYYY);
            Scribe_Values.Look(ref vanillaArchitect, "vanillaArchitect", false);
            Scribe_Values.Look(ref vanillaAnimals, "vanillaAnimals", false);
            Scribe_Values.Look(ref initializedDesignations, "initializedDesignations", false);

            Scribe_Collections.Look(ref hiddenDesignations, "hiddenDesignations",LookMode.Value);
            Scribe_Collections.Look(ref leftDesignations, "leftDesignations", LookMode.Value);
            Scribe_Collections.Look(ref mainDesignations, "mainDesignations", LookMode.Value);
            Scribe_Collections.Look(ref rightDesignations, "rightDesignations", LookMode.Value);

            base.ExposeData();
        }

        public static void RestoreDesignationLists()
        {
            SetDefaultList(Settings.DesignationConfig.hidden);
            SetDefaultList(Settings.DesignationConfig.left);
            SetDefaultList(Settings.DesignationConfig.main);
            SetDefaultList(Settings.DesignationConfig.right);
            UINotIncludedStatic.Log("DesignationConfigs default restored.");
        }

        private static void SetDefaultList(Settings.DesignationConfig designationList)
        {
            switch (designationList)
            {
                case Settings.DesignationConfig.hidden:
                    hiddenDesignations = new List<string>();
                    return;
                case Settings.DesignationConfig.left:
                    leftDesignations = new List<string> { "Forbid", "Uninstall", "Allow", "Claim" };
                    break;
                case Settings.DesignationConfig.main:
                    mainDesignations = new List<string> { "Harvest", "Deconstruct", "Cancel", "Chop wood", "Mine" };
                    break;
                case Settings.DesignationConfig.right:
                    rightDesignations = new List<string> { "Strip", "Open", "Smooth surface", "Tame", "Haul things", "Slaughter", "Cut plants", "Hunt" };
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

            for (int i = 0; i < designationConfigs.Count(); i++)
            {
                foreach(String designatorName in designationConfigs[i])
                {
                    bool found = false;
                    foreach(Designator designator in avaibleDesignators)
                    {
                        if (designator.Label == designatorName) { arrayDesignatorConfigs[i].Add(designator); usedDesignators.Add(designator); found = true; break; }
                    }
                    if (!found) UINotIncludedStatic.Warning(String.Format("The designation named '{0}' was not found.", designatorName));
                }
            }

            foreach(Designator designator in avaibleDesignators)
            {
                UINotIncludedStatic.Log(String.Format("The designation name is '{0}'.", designator.Label));
                if (!usedDesignators.Contains(designator))
                {
                    arrayDesignatorConfigs[(int)Settings.DesignationConfig.hidden].Add(designator);
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
            if (!UINotIncludedSettings.initializedDesignations) {
                UINotIncludedStatic.Log("DesigationConfigs never initialized. Initializing.");
                UINotIncludedSettings.RestoreDesignationLists();
                UINotIncludedSettings.initializedDesignations = true;
                settings.Write();
            }
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            float columnWidth = inRect.width / 3;
            float heigth = (float)Math.Floor(inRect.height / 3);
            Rect column1 = new Rect(inRect.x, inRect.y, columnWidth, heigth);
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(column1);
            listingStandard.CheckboxLabeled("UINotIncluded.Setting.tabsOnTop".Translate(), ref UINotIncludedSettings.tabsOnTop, "UINotIncluded.Setting.tabsOnTop.Description".Translate());
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
            DoJobBarConfigurationWidget(new Rect(inRect.x, inRect.y + heigth, inRect.width, inRect.height - heigth));
            base.DoSettingsWindowContents(inRect);
        }

        public void DoJobBarConfigurationWidget(Rect rect)
        {
            float columnWidth = rect.width / 4;
            float curY = rect.y;


            Widgets.ButtonText(new Rect(rect.x,curY,rect.width,25f), "Reestore to default");
            curY += 25f;
            for(int i = 0; i < 4; i++)
            {
                Widgets.DrawAtlas(new Rect(rect.x + columnWidth * i, curY, columnWidth, rect.height - 25f).ContractedBy(3f), ContentFinder<Texture2D>.Get("GD/UI/ClockSCR"));
            }
        }

        public override string SettingsCategory()
        {
            return UINotIncludedStatic.Name;
        }
    }
}
