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
        private static List<Designator>[] CacheJobs = new List<Designator>[] { new List<Designator>(), new List<Designator>(), new List<Designator>(), new List<Designator>() };

        public static List<Designator>[] GetDesignationConfigs()
        {
            if (updated) return CacheJobs;
            foreach (List<Designator> designatorslist in CacheJobs) designatorslist.Clear();
            CacheJobs = Settings.GetDesignationConfigs();
            updated = true;
            return CacheJobs;
        }

        public static void Update()
        {
            updated = false;
        }
    }
}
