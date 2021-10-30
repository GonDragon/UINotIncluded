using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;

namespace UINotIncluded.Widget
{
    public class ExtendedWidgetDef : Def
    {
        public System.Type workerClass;
        public override TaggedString LabelCap
        {
            get
            {
                if (this.label == null) this.label = defName;

                return base.LabelCap + " (widget)";
            }
        }
        public float minWidth = 0f;
        public bool multipleInstances = false;
        public int order;
        private WidgetWorker workerInt;

        public void OnGUI(Rect rect, BarElementMemory memory) => Worker.OnGUI(rect, memory);

        public WidgetWorker Worker
        {
            get
            {
                if(this.workerInt == null && this.workerClass != (System.Type)null)
                {
                    this.workerInt = (WidgetWorker)Activator.CreateInstance(this.workerClass);
                    this.workerInt.def = this;
                }
                return this.workerInt;
            }
        }

        public bool WidgetVisible { get => Worker.WidgetVisible; }
    }
}
