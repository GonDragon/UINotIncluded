using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;

namespace UINotIncluded
{
    public abstract class BarElementMemory : IExposable
    {
        public abstract void LoadMemory();
        public abstract void Update();

        public abstract void Clear();
        public abstract void ExposeData();

        public abstract void Reset();
    }

    public class EmptyMemory : BarElementMemory
    {
        public override void Clear()
        {}

        public override void ExposeData()
        {}

        public override void LoadMemory()
        {}

        public override void Reset()
        {}

        public override void Update()
        {}
    }

    public class MainButtonMemory : BarElementMemory
    {
        public string label;
        public string iconPath;
        public bool minimized;

        public string defaultLabel;
        public string defaultIconPath;
        public bool defaultMinimized;

        private bool initialized = false;
        
        private string defName;
        private MainButtonDef _def;
        public MainButtonDef Def
        {
            get
            {
                if(_def == null)
                {
                    _def = DefDatabase<MainButtonDef>.GetNamed(defName);
                }
                return _def;
            }
        }



        public MainButtonMemory() { this.initialized = true;  } // Clear initializer for ExposeData

        public MainButtonMemory(MainButtonDef def)
        {
            this._def = def;
            this.defName = def.defName;
        }

        public override void LoadMemory()
        {
            if (!initialized)
            {
                this.defaultLabel = Def.label;
                this.defaultIconPath = Def.iconPath;
                this.defaultMinimized = Def.minimized;
                this.Clear();
                initialized = true;
                return;
            }

            this.Update();
                
        }

        public override void Update()
        {
            Def.minimized = this.minimized;
            if(Def.label != this.label)
            {
                Def.label = this.label;
                Def.ClearCachedData();
            }
            if(Def.iconPath != this.iconPath)
            {
                Def.iconPath = this.iconPath;
                HarmonyLib.Traverse.Create(Def).Field("icon").SetValue(null);
            }
            
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref label, "label");
            Scribe_Values.Look(ref iconPath, "iconPath");
            Scribe_Values.Look(ref minimized, "minimized");
            Scribe_Values.Look(ref defName, "button");

            Scribe_Values.Look(ref defaultLabel, "defaultLabel");
            Scribe_Values.Look(ref defaultIconPath, "defaultIconPath");
            Scribe_Values.Look(ref defaultMinimized, "defaultMinimized");
        }

        public override void Clear()
        {
            this.label = Def.label;
            this.iconPath = Def.iconPath;
            this.minimized = Def.minimized;
            Update();
        }

        public override void Reset()
        {
            this.label = this.defaultLabel;
            this.iconPath = this.defaultIconPath;
            this.minimized = this.defaultMinimized;
            Update();
        }
    }
}
