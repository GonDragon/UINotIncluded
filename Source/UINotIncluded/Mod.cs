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
    public static class Mod
    {
        static Mod()
        {
            const string Id = "UINotIncluded";
            const string author = "GonDragon";

            var harmony = new Harmony($"{author}.{Id}");
            harmony.PatchAll();

            Log.Message("UINotIncluded Initialized. Harmony id:");
            Log.Message($"{author}.{Id}");
            Log.Message("Patched methods:");
            var myOriginalMethods = harmony.GetPatchedMethods();
            foreach (var method in myOriginalMethods) {
                Log.Message(method.Name);
            }
        }
    }
}
