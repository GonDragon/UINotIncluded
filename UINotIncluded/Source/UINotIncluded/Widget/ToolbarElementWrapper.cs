using RimWorld;
using System;
using System.Runtime.Serialization;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    public class ToolbarElementWrapper : IExposable, IEquatable<ToolbarElementWrapper>
    {
        public string defName;
        public bool isWidget;
        private Action _configAction;
        private bool _configActionLoaded = false;
        private BarElementMemory _memory;
        public bool markedForDeletion = false;
        private Def defCache;
        public ToolbarElementWrapper()
        {
        }

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

        public Action ConfigAction
        {
            get
            {
                if (!_configActionLoaded)
                {
                    if (!isWidget) _configAction = () => Find.WindowStack.Add(new UINotIncluded.Windows.EditMainButton_Window((MainButtonMemory)Memory));
                    else _configAction = ((ExtendedWidgetDef)Def).Worker.ConfigAction(Memory);
                    _configActionLoaded = true;
                }
                return _configAction;
            }
        }

        public Def Def
        {
            get
            {
                if (defCache == null)
                {
                    if (isWidget)
                    {
                        defCache = DefDatabase<ExtendedWidgetDef>.GetNamedSilentFail(defName);
                    }
                    else defCache = DefDatabase<MainButtonDef>.GetNamedSilentFail(defName);
                    if (defCache == null)
                    {
                        UINI.Warning(string.Format("Error loading {0} def element from database.", defName));
                        markedForDeletion = true;
                        isWidget = true;
                        defCache = null;
                    }
                }
                return defCache;
            }
        }

        public bool FixedWidth
        {
            get
            {
                return isWidget ? ((ExtendedWidgetDef)Def).Worker.FixedWidth(Memory) : ((MainButtonDef)Def).minimized;
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

        public BarElementMemory Memory
        {
            get
            {
                if (_memory == null)
                {
                    Def elementDef = this.Def; // Load the Def before checking for if widget to silently catch deleted buttons defs
                    if (isWidget) _memory = ((ExtendedWidgetDef)elementDef).Worker.CreateMemory;
                    else _memory = new MainButtonMemory((MainButtonDef)elementDef);
                    _memory.LoadMemory();
                }
                return _memory;
            }
        }
        public bool MultipleInstances
        {
            get
            {
                if (isWidget) return ((ExtendedWidgetDef)Def).multipleInstances;
                return false;
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

        public void LoadMemory()
        {
            if (Def == null) markedForDeletion = true;
            else Memory.LoadMemory();
        }

        public bool Visible
        {
            get
            {
                if (!isWidget) return ((MainButtonDef)Def).buttonVisible;
                return ((ExtendedWidgetDef)Def).WidgetVisible;
            }
        }

        public float Width
        {
            get
            {
                if (!isWidget) return FixedWidth ? (float)Math.Floor((UIManager.ExtendedBarHeight / 2f) * 3f) : -1f;
                return Math.Max(((ExtendedWidgetDef)Def).minWidth, Memory.Width);
            }
        }
        public bool Equals(ToolbarElementWrapper other)
        {
            return (this.isWidget == other.isWidget) && (this.defName == other.defName);
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref isWidget, "isWidget");
            Scribe_Values.Look(ref defName, "wrapped");

            try
            {
                string label = "memory";
                if (Scribe.mode == LoadSaveMode.LoadingVars)
                {
                    string typeName = Scribe.loader.curXmlParent[label].Attributes.GetNamedItem("Class").Value;
                    Type memoryType = Type.GetType(typeName);
                    if(memoryType == null)
                    {
                        _memory = new BarElementMemory();
                        this.markedForDeletion = true;
                        throw new NotImplementedException();
                    } else
                    {
                        Scribe_Deep.Look(ref _memory, label);
                    }
                } else
                {
                    Scribe_Deep.Look(ref _memory, label);
                }                
            } catch
            {
                UINI.Warning(string.Format("Could not load memory of widget {0}. Memory class non existent.",defName));
            }
            
        }
        public void OnGUI(Rect rect)
        {
            if (isWidget) ((ExtendedWidgetDef)Def).OnGUI(rect, Memory);
            else ((MainButtonDef)Def).Worker.DoButton(rect);
        }
    }
}