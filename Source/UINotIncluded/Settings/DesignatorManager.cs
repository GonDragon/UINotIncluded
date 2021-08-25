using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;

namespace UINotIncluded
{
    static class DesignatorManager
    {
        private static bool updated = false;
        private static List<Designator>[] CacheJobs;

        public static List<Designator>[] GetDesignationConfigs()
        {
            if (updated) return CacheJobs;
            CacheJobs = UINotIncludedSettings.GetDesignationConfigs();
            updated = true;
            return CacheJobs;
        }

        public static void Update()
        {
            updated = false;
        }
    }
}
