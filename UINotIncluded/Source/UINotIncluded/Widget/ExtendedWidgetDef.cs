using System;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    public class ExtendedWidgetDef : Def
    {
        public float minWidth = 0f;
        public bool multipleInstances = false;
        public int order;
        public System.Type workerClass;

        private WidgetWorker workerInt;

        public override TaggedString LabelCap
        {
            get
            {
                if (this.label == null) this.label = defName;

                return base.LabelCap + " (widget)";
            }
        }
        public bool WidgetVisible { get => Worker.WidgetVisible; }

        public WidgetWorker Worker
        {
            get
            {
                if (this.workerInt == null && this.workerClass != (System.Type)null)
                {
                    this.workerInt = (WidgetWorker)Activator.CreateInstance(this.workerClass);
                    this.workerInt.def = this;
                }
                return this.workerInt;
            }
        }

        public void OnGUI(Rect rect, BarElementMemory memory) => Worker.OnGUI(rect, memory);
    }
}