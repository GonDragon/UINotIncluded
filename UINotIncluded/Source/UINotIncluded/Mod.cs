using System;
using System.Linq;
using System.Reflection;

using HarmonyLib;
using RimWorld;
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
            CompatibilityPatches();
            //LoadMainButtonsSettings();

            try
            {
                if (!Settings.initializedDesignations)
                {
                    UINI.Log("DesigationConfigs never initialized. Initializing.");
                    Settings.RestoreDesignationLists();
                    Settings.initializedDesignations = true;
                    LoadedModManager.GetMod<UINI_Mod>().WriteSettings();
                }
            } catch
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
        }

        public static void Log(string message) => Verse.Log.Message(PrefixMessage(message));
        public static void Warning(string message) => Verse.Log.Warning(PrefixMessage(message));
        public static void Error(string message) => Verse.Log.Error(PrefixMessage(message));
        public static void ErrorOnce(string message, string key) => Verse.Log.ErrorOnce(PrefixMessage(message), key.GetHashCode());
        public static void Message(string message) => Messages.Message(message, MessageTypeDefOf.TaskCompletion, false);
        private static string PrefixMessage(string message) => $"[{Name} v{Version}] {message}";

        static void CompatibilityPatches()
        {
            try
            {
                if (LoadedModManager.RunningModsListForReading.Any(x => x.Name == "Smart Speed")) Widget.Timespeed_Worker.SetSmartspeedMode();
            }
            catch (TypeLoadException ex) { Error(String.Format("Error checking if SmartSpeed its installed.\n{0}", ex.ToString())); }
        }

        static bool IsErroring(Widget.ToolbarElementWrapper wrapper)
        {
            return (wrapper.defName == "ErroringWidget") || wrapper.Def == null;
        }
    }
}
