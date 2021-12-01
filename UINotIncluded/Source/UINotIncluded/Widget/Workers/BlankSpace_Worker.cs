using System;

using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    internal class BlankSpace_Worker : WidgetWorker
    {
        public override BarElementMemory CreateMemory => new BlankSpaceMemory();

        public override Action ConfigAction(BarElementMemory memory)
        {
            return () =>
            {
                Find.WindowStack.Add(new UINotIncluded.Windows.EditBlankSpace_Window((BlankSpaceMemory)memory));
            };
        }

        public override void OnGUI(Rect rect)
        {
            throw new NotImplementedException();
        }
    }
}