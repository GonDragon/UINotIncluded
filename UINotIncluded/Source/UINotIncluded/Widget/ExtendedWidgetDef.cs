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

    public abstract class WidgetWorker
    {
        public float iconSize = 24f;
        public ExtendedWidgetDef def;
        public virtual Action ConfigAction(BarElementMemory memory)
        {
            return null;
        }

        public abstract void OnGUI(Rect rect, BarElementMemory memory);

        public virtual float GetWidth() { return def.minWidth; }
        public virtual float GetWidth(BarElementMemory memory) { return GetWidth(); }

        public virtual bool FixedWidth(BarElementMemory memory) { return true; }

        public virtual bool WidgetVisible => true;

        public virtual BarElementMemory CreateMemory => new EmptyMemory();

        public virtual void Margins(ref Rect rect)
        {

            rect = rect.ContractedBy(ExtendedToolbar.margin);
        }

        public virtual void Padding(ref Rect rect)
        {

            rect = rect.ContractedBy(ExtendedToolbar.padding);
        }

        public Rect DrawIcon(Texture2D icon, float curX, float curY, string tooltip = null)
        {
            Rect rect = new Rect(curX, curY, iconSize, iconSize);
            GUI.DrawTexture(rect, icon);
            if (!tooltip.NullOrEmpty())
                TooltipHandler.TipRegion(rect, (TipSignal)tooltip);
            return rect;
        }
    }
}
