using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UINotIncluded.Widget.Workers;
using Verse;

namespace UINotIncluded.Widget.Configs
{
    public class ButtonConfig : ElementConfig
    {
        MainButtonDef mainButtonDef;

        public string _iconPath;
        public string _label;
        public bool? _minimized;
        private Button_Worker _worker;

        public ButtonConfig(string defName)
        {
            this.defName = defName;
        }

        public ButtonConfig(MainButtonDef def)
        {
            this.defName = def.defName;
            mainButtonDef = def;
        }

        public ButtonConfig() { } //Empty constructor to load from ExposeData

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref _iconPath, "iconPath");
            Scribe_Values.Look(ref _label, "label");
            Scribe_Values.Look(ref _minimized, "minimized");
        }

        public string IconPath
        {
            get
            {
                if (_iconPath == null) return ((MainButtonDef)Def).iconPath;
                return _iconPath;
            }
        }
        public string Label
        {
            get
            {
                if (_label == null) return ((MainButtonDef)Def).label;
                return _label;
            }
        }
        public bool Minimized
        {
            get
            {
                if (_minimized == null) return ((MainButtonDef)Def).minimized;
                return (bool)_minimized;
            }
        }

        public override bool Configurable => true;

        public override WidgetWorker Worker
        {
            get
            {
                if (_worker == null) _worker = new Workers.Button_Worker(this);
                return _worker;
            }
        }

        public override Def Def
        {
            get
            {
                if (mainButtonDef == null) mainButtonDef = DefDatabase<MainButtonDef>.GetNamed(this.defName);
                return mainButtonDef;
            }
        }
    }
}
