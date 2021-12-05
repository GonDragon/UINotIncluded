using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UINotIncluded.Utility.Deprecated
{
    public static class DeprecationManager
    {
        public static List<ElementWrapper> DeprecatedTopBar;
        public static List<ElementWrapper> DeprecatedBottomBar;

        public static void UpdateBarsToNewVersion()
        {
            foreach (ElementWrapper element in DeprecatedTopBar) Settings.TopBarElements.Add(element.ConvertToConfig());
            foreach (ElementWrapper element in DeprecatedBottomBar) Settings.BottomBarElements.Add(element.ConvertToConfig());
        }
    }
}
