using System;

using Verse;

namespace UINotIncluded.Utility.Deprecated
{
    public class ElementWrapper : IExposable
    {
        private bool isWidget;
        private string defname;
        private Memory memory;

        public void ExposeData()
        {
            Scribe_Values.Look(ref isWidget, "isWidget", false);
            Scribe_Values.Look(ref defname, "wrapped");
            Scribe_Deep.Look(ref memory, "memory", null);
        }

        public Widget.Configs.ElementConfig ConvertToConfig()
        {
            if (memory.GetType() != typeof(Memory)) return memory.ConvertToConfig();

            if (DefDatabase<Widget.WidgetDef>.GetNamedSilentFail(defname) != null) return DefDatabase<Widget.WidgetDef>.GetNamedSilentFail(defname).GetNewConfig();

            UINI.Warning(string.Format("Error converting old widget. Defname: {0} - IsWidget {1}", defname, isWidget.ToString()));
            throw new NotImplementedException();
        }
    }
}