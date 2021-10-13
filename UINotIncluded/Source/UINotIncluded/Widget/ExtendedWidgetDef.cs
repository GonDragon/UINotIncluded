using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;

namespace UINotIncluded.Widget
{
    public class ExtendedWidgetDef : Def
    {
        public System.Type workerClass;

        public float minWidth = 0f;
        public float Width => Worker.GetWidth();
        public int order;
        private WidgetWorker workerInt;


        public void OnGUI(Rect rect) => Worker.OnGUI(rect);

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
    }

    public abstract class WidgetWorker
    {
        public float iconSize = 24f;
        internal ExtendedWidgetDef def;

        public abstract void OnGUI(Rect rect);

        public virtual float GetWidth() { return def.minWidth; }

        public Rect DrawIcon(Texture2D icon, float curX, string tooltip = null)
        {
            Rect rect = new Rect(curX, 4f, iconSize, iconSize);
            GUI.DrawTexture(rect, icon);
            if (!tooltip.NullOrEmpty())
                TooltipHandler.TipRegion(rect, (TipSignal)tooltip);
            return rect;
        }
    }

    public class ToolbarElementWrapper : IExposable
    {
        public bool isWidget;
        ExtendedWidgetDef widget;
        MainButtonDef button;
        
        public ToolbarElementWrapper(ExtendedWidgetDef widget)
        {
            this.isWidget = true;
            this.widget = widget;
        }

        public ToolbarElementWrapper(MainButtonDef button)
        {
            this.isWidget = false;
            this.button = button;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref isWidget, "isWidget");
            if(isWidget) { Scribe_Defs.Look(ref widget, "wraped"); } else { Scribe_Defs.Look(ref button, "wraped"); }
        }

        public void OnGUI(Rect rect)
        {
            if (this.isWidget) this.widget.OnGUI(rect);
            else button.Worker.DoButton(rect);
        }
    }
}
