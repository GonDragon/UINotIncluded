using System;
using System.Reflection;
using Verse;

namespace UINotIncluded.Utility
{
    [StaticConstructorOnStartup]
    internal static class AssemblyChecker
    {
        private static Assembly[] loadedAssemblies;

        static AssemblyChecker()
        {
            loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        public static bool TypeLoaded(string type)
        {
            foreach (Assembly assembly in loadedAssemblies)
            {
                Type t = Type.GetType(type + string.Format(", {0}", assembly.FullName));
                if (t != null) return true;
            }
            return false;
        }
    }
}