using UnityEngine;

namespace UINotIncluded.Widget
{
    /* Empty worker to load on errors */

    internal class Erroring_Worker : WidgetWorker
    {
        public override bool WidgetVisible => false;

        public override void OnGUI(Rect rect, BarElementMemory memory)
        { }
    }
}