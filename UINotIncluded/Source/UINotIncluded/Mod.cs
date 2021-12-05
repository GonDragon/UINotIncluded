using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace UINotIncluded
{
    [StaticConstructorOnStartup]
    public static class UINI
    {
        public static string Author => "GonDragon";
        public static string Name => Assembly.GetName().Name;
        public static string Id => Author + "." + Name;

        public static string Version => Assembly.GetName().Version.ToString();

        private static Assembly Assembly
        {
            get
            {
                return Assembly.GetAssembly(typeof(UINI));
            }
        }

        public static readonly Harmony Harmony;

        static UINI()
        {
            Harmony = new Harmony(Id);
            Harmony.PatchAll();

            try
            {
                if (!Settings.initializedDesignations)
                {
                    UINI.Log("DesigationConfigs never initialized. Initializing.");
                    Settings.RestoreDesignationLists();
                    Settings.initializedDesignations = true;
                    LoadedModManager.GetMod<UINI_Mod>().WriteSettings();
                }
            }
            catch
            {
                UINI.Error("Error initializing settings for Designation Bar");
            }

            try
            {
                if (!Settings.initializedDefaultBar)
                {
                    UINI.Log("TabsBar never initialized. Initializing.");
                    Settings.RestoreDefaultMainBar();
                    Settings.initializedDefaultBar = true;
                    LoadedModManager.GetMod<UINI_Mod>().WriteSettings();
                }
            }
            catch
            {
                UINI.Error("Error initializing settings for TabsBar");
            }

            CompatibilityPatches();
            VersionPatcher();
            LoadMainButtonsSettings();
        }

        private static void LoadMainButtonsSettings()
        {
            if (Settings.TopBarElements.Count == 0 && Settings.BottomBarElements.Count == 0)
            {
                UINI.Warning("Empty bottom and topbar detected. Repopulating");
                Settings.RestoreDefaultMainBar();
            }
            bool DeleteNonExistentButtons(List<Widget.Configs.ElementConfig> configList)
            {
                List<int> indexToDelete = new List<int>();
                bool deleted = false;

                int i = 0;
                foreach(Widget.Configs.ElementConfig config in configList)
                {
                    try
                    {
                        if(config.GetType() == typeof(Widget.Configs.ButtonConfig)) ((Widget.Configs.ButtonConfig)config).Def.GetType();
                    } catch
                    {
                        UINI.Warning(string.Format("Def {0} not found. Marked for removal.", ((Widget.Configs.ButtonConfig)config).defName));
                        indexToDelete.Add(i);
                    } finally
                    {
                        i++;
                    }
                }

                foreach(int k in indexToDelete.OrderByDescending(k => k))
                {
                    configList.RemoveAt(k);
                    deleted = true;
                }

                return deleted;
            }

            bool somethingDeleted = DeleteNonExistentButtons(Settings.TopBarElements);
            somethingDeleted |= DeleteNonExistentButtons(Settings.BottomBarElements);

            if (somethingDeleted) LoadedModManager.GetMod<UINI_Mod>().WriteSettings();
        }

        public static void Log(string message) => Verse.Log.Message(PrefixMessage(message));

        public static void Warning(string message) => Verse.Log.Warning(PrefixMessage(message));

        public static void Error(string message) => Verse.Log.Error(PrefixMessage(message));

        public static void ErrorOnce(string message, string key) => Verse.Log.ErrorOnce(PrefixMessage(message), key.GetHashCode());

        public static void Message(string message) => Messages.Message(message, MessageTypeDefOf.TaskCompletion, false);

        private static string PrefixMessage(string message) => $"[{Name} v{Version}] {message}";

        private static void CompatibilityPatches()
        {
            try
            {
                if (LoadedModManager.RunningModsListForReading.Any(x => x.Name == "Smart Speed")) Widget.Workers.Timespeed_Worker.SetSmartspeedMode();
            }
            catch (TypeLoadException ex) { Error(String.Format("Error checking if SmartSpeed its installed.\n{0}", ex.ToString())); }
        }

        private static void VersionPatcher()
        {
            Version lastVersionUsed = new Version(Settings.lastVersion ?? "0.0.0.0");

            if (lastVersionUsed < new Version(1, 1, 0, 0))
            {
                UINI.Log("Old config-file detected. Updating to 1.1.0.0+");
                Utility.Deprecated.DeprecationManager.UpdateBarsToNewVersion();
            }

            if(lastVersionUsed < Assembly.GetName().Version)
            {
                Settings.lastVersion = Version;
                LoadedModManager.GetMod<UINI_Mod>().WriteSettings();
            }
        }
    }
}