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

        public bool WidgetVisible { get => Worker.WidgetVisible; }
    }

    public abstract class WidgetWorker
    {
        public float iconSize = 24f;
        internal ExtendedWidgetDef def;

        public abstract void OnGUI(Rect rect);

        public virtual float GetWidth() { return def.minWidth; }

        public virtual bool WidgetVisible { get => true; }

        public virtual BarElementMemory Memory => new EmptyMemory();

        public virtual void Margins(ref Rect rect)
        {

            rect = rect.ContractedBy(ExtendedToolbar.margin);
        }

        public virtual void Padding(ref Rect rect)
        {

            rect = rect.ContractedBy(ExtendedToolbar.padding);
        }

        public Rect DrawIcon(Texture2D icon, float curX, string tooltip = null)
        {
            Rect rect = new Rect(curX, 5f, iconSize, iconSize);
            GUI.DrawTexture(rect, icon);
            if (!tooltip.NullOrEmpty())
                TooltipHandler.TipRegion(rect, (TipSignal)tooltip);
            return rect;
        }
    }

    public class ToolbarElementWrapper : IExposable, IEquatable<ToolbarElementWrapper>
    {
        public bool isWidget;
        public string defName;
        private Def defCache;

        private BarElementMemory _memory;
        private bool _configActionLoaded = false;
        private Action _configAction;
        public Action ConfigAction
        {
            get
            {
                if(!_configActionLoaded)
                {
                    if (!isWidget) _configAction = () => Find.WindowStack.Add(new UINotIncluded.Windows.EditMainButton_Window((MainButtonMemory)Memory));
                    _configActionLoaded = true;
                }
                return _configAction;
            }
        }

        public BarElementMemory Memory
        {
            get
            {
                if(_memory == null)
                {
                    if (isWidget) _memory = ((ExtendedWidgetDef)Def).Worker.Memory;
                    else _memory = new MainButtonMemory((MainButtonDef)Def);
                    _memory.LoadMemory();
                }
                return _memory;
            }
        }

        public Def Def
        {
            get
            {
                if(defCache == null)
                {
                    if (isWidget) defCache = DefDatabase<ExtendedWidgetDef>.GetNamed(defName);
                    else defCache = DefDatabase<MainButtonDef>.GetNamed(defName);
                }
                return defCache;
            }
        }

        public string LabelCap
        {
            get
            {
                if (isWidget) return this.defName + " (widget)";
                return Def.LabelCap;
            }
        }

        public bool FixedWidth
        {
            get
            {
                return isWidget || ((MainButtonDef)Def).minimized;
            }
        }

        public float Width
        {
            get
            {
                if (!isWidget) return FixedWidth ? UIManager.ExtendedBarHeight : -1f;
                return ((ExtendedWidgetDef)Def).Width;
            }
        }

        public bool Visible
        {
            get
            {
                if (!isWidget) return ((MainButtonDef)Def).buttonVisible;
                return ((ExtendedWidgetDef)Def).WidgetVisible;
            }
        }

        public ToolbarElementWrapper() { } //So it can be instantiated by the Scribe

        public ToolbarElementWrapper(ExtendedWidgetDef widget)
        {
            this.isWidget = true;
            this.defName = widget.defName;
            defCache = widget;
        }

        public ToolbarElementWrapper(MainButtonDef button)
        {
            this.isWidget = false;
            this.defName = button.defName;
            defCache = button;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref isWidget, "isWidget");
            Scribe_Values.Look(ref defName, "wrapped");
            Scribe_Deep.Look(ref _memory, "memory");
        }

        public bool Equals(ToolbarElementWrapper other)
        {
            return (this.isWidget == other.isWidget) && (this.defName == other.defName);

        }

        public void OnGUI(Rect rect)
        {
            if (isWidget) ((ExtendedWidgetDef)Def).OnGUI(rect);
            else ((MainButtonDef)Def).Worker.DoButton(rect);
        }
    }
}
