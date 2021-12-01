using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace UINotIncluded.Widget
{
    [StaticConstructorOnStartup]
    public static class WidgetManager
    {
        public static IEnumerable<Configs.ElementConfig> MainTabButtons
        {
            get
            {
                foreach (MainButtonDef button in DefDatabase<MainButtonDef>.AllDefs.OrderBy(def => def.order))
                {
                    if (button.defName == "Inspect") continue;
                    yield return new Configs.ButtonConfig(button);
                }
            }        
        }
    }
}
