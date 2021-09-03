using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using RimWorld;
using Verse;

namespace UINotIncluded
{
    [StaticConstructorOnStartup]
    public static class UINotIncludedStatic
    {
        public const string Name = "UINotIncluded";
        public const string Author = "GonDragon";
        public const string Id = Author + "." + Name;
        public const string Version = "1.0.3";

        public static readonly Harmony Harmony;
        static UINotIncludedStatic()
        {
            Harmony = new Harmony(Id);
            Harmony.PatchAll();
            CompatibilityPatches();
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
                if (LoadedModManager.RunningModsListForReading.Any(x => x.Name == "Smart Speed")) Widget.Timespeed.SetSmartspeedMode();
            }
            catch (TypeLoadException ex) { Error(String.Format("Error checking if SmartSpeed its installed.\n{0}", ex.ToString())); }
        }
    }
}
