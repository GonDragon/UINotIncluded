using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            const string Name = Id;
            const string Version = "1.0.0";

            Log.Message("UINotIncluded Initialized");
        }
    }
}
