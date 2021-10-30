using System;

using RimWorld;
using Verse;
using UnityEngine;

namespace UINotIncluded.Widget
{
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
                    else _configAction = ((ExtendedWidgetDef)Def).Worker.ConfigAction(Memory);
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
                    Def elementDef = this.Def; // Load the Def before checking for if widget to silently catch deleted buttons defs
                    if (isWidget) _memory = ((ExtendedWidgetDef)elementDef).Worker.CreateMemory;
                    else _memory = new MainButtonMemory((MainButtonDef)elementDef);
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
                    if (isWidget)
                    {
                        defCache = DefDatabase<ExtendedWidgetDef>.GetNamedSilentFail(defName);
                    }
                    else defCache = DefDatabase<MainButtonDef>.GetNamedSilentFail(defName);
                    if(defCache == null)
                    {
                        UINI.Warning(string.Format("Error loading {0} def element from database.",defName));
                        defName = "ErroringWidget";
                        isWidget = true;
                        defCache = DefDatabase<ExtendedWidgetDef>.GetNamed("ErroringWidget");
                    }
                }
                return defCache;
            }
        }

        public string LabelCap
        {
            get
            {
                if (isWidget) return ((ExtendedWidgetDef)Def).Worker.LabelCap(Memory);
                if (!((MainButtonDef)Def).buttonVisible) return Def.defName + " (hidden)";
                return Def.defName;
            }
        }

        public bool FixedWidth
        {
            get
            {
                return isWidget ? ((ExtendedWidgetDef)Def).Worker.FixedWidth(Memory) : ((MainButtonDef)Def).minimized;
            }
        }

        public float Width
        {
            get
            {
                if (!isWidget) return FixedWidth ? (float)Math.Floor((UIManager.ExtendedBarHeight / 2f) * 3f) : -1f;
                return Math.Max(((ExtendedWidgetDef)Def).minWidth,Memory.Width);
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

        public int Order
        {
            get
            {
                if (!isWidget) return ((MainButtonDef)Def).order;
                return ((ExtendedWidgetDef)Def).order;
            }
        }

        public bool MultipleInstances
        {
            get
            {
                if(isWidget) return ((ExtendedWidgetDef)Def).multipleInstances;
                return false;
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
            if (isWidget) ((ExtendedWidgetDef)Def).OnGUI(rect, Memory);
            else ((MainButtonDef)Def).Worker.DoButton(rect);
        }
    }
}
