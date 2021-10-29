using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    class BlankSpace_Worker : WidgetWorker
    {
        public override BarElementMemory CreateMemory => new BlankSpaceMemory();
        public override float GetWidth(BarElementMemory memory)
        {
            return FixedWidth(memory) ? ((BlankSpaceMemory)memory).width : -1f;
        }
        public override bool FixedWidth(BarElementMemory memory)
        {
            return ((BlankSpaceMemory)memory).fixedWidth;
        }
        public override Action ConfigAction(BarElementMemory memory)
        {
            return () => {
                    Find.WindowStack.Add(new UINotIncluded.Windows.EditBlankSpace_Window((BlankSpaceMemory)memory));
                };
}
        public override void OnGUI(Rect rect, BarElementMemory memory)
        { }
    }
}
