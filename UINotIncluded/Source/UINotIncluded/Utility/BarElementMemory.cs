using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace UINotIncluded
{
    public class BarElementMemory : IExposable
    {
        public virtual bool FixedWidth => true;
        public virtual float Width => 0f;

        public virtual void Clear() { }

        public virtual void ExposeData() { }

        public virtual void LoadMemory() { }

        public virtual void Reset() { }

        public virtual void Update() { }
    }

    public class BlankSpaceMemory : BarElementMemory
    {
        public bool fixedWidth = false;
        public float width = 100f;

        public BlankSpaceMemory()
        {
        }

        public override bool FixedWidth => fixedWidth;
        public override float Width => width;
        public override void Clear()
        { }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref fixedWidth, "fixed");
            Scribe_Values.Look(ref width, "width");
        }

        public override void LoadMemory()
        { }

        public override void Reset()
        {
            fixedWidth = false;
            width = 100f;
        }

        public override void Update()
        { }
    }

    public class EmptyMemory : BarElementMemory // Legacy usseless memory
    {
        public override void Clear()
        { }

        public override void ExposeData()
        { }

        public override void LoadMemory()
        { }

        public override void Reset()
        { }

        public override void Update()
        { }
    }

    public class MainButtonMemory : BarElementMemory
    {
        public string defaultIconPath;
        public string defaultLabel;
        public bool defaultMinimized;
        public bool defaultVisible;
        public string iconPath;
        public string label;
        public bool minimized;
        public bool visible;
        private MainButtonDef _def;
        private string defName;
        private bool initialized = false;
        private bool loaded = false;
        public MainButtonMemory()
        {
            this.initialized = true;
        }

        public MainButtonMemory(MainButtonDef def)
        {
            this._def = def;
            this.defName = def.defName;
        }

        public MainButtonDef Def
        {
            get
            {
                if (_def == null)
                {
                    _def = DefDatabase<MainButtonDef>.GetNamedSilentFail(defName ?? "");
                }
                return _def;
            }
            set
            {
                _def = value;
            }
        }

         // Clear initializer for ExposeData

        public override bool FixedWidth => this.minimized;
        public override void Clear()
        {
            this.label = Def.label;
            this.iconPath = Def.iconPath;
            this.minimized = Def.minimized;
            this.visible = Def.buttonVisible;
            Update();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref label, "label");
            Scribe_Values.Look(ref iconPath, "iconPath");
            Scribe_Values.Look(ref minimized, "minimized");
            Scribe_Values.Look(ref defName, "button");
            Scribe_Values.Look(ref visible, "visible", true);
        }

        public override void LoadMemory()
        {
            if (!loaded)
            {
                this.defaultLabel = Def.label;
                this.defaultIconPath = Def.iconPath;
                this.defaultMinimized = Def.minimized;
                this.defaultVisible = Def.buttonVisible;
                loaded = true;
            }
            if (!initialized)
            {
                this.Clear();
                initialized = true;
                return;
            }
            if (ContentFinder<Texture2D>.Get(this.iconPath ?? "", false) == null) this.iconPath = this.defaultIconPath;
            this.Update();
        }

        public override void Reset()
        {
            this.label = this.defaultLabel;
            this.iconPath = this.defaultIconPath;
            this.minimized = this.defaultMinimized;
            this.visible = this.defaultVisible;
            Update();
        }

        public override void Update()
        {
            Def.minimized = this.minimized;
            Def.buttonVisible = this.visible;
            if (Def.label != this.label)
            {
                Def.label = this.label;
                Def.ClearCachedData();
            }
            if (Def.iconPath != this.iconPath)
            {
                Def.iconPath = this.iconPath;
                HarmonyLib.Traverse.Create(Def).Field("icon").SetValue(null);
            }
        }
    }
}