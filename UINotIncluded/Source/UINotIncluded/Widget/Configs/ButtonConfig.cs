using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UINotIncluded.Widget.Workers;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget.Configs
{
    public class ButtonConfig : ElementConfig
    {
        MainButtonDef mainButtonDef;
        public string defName;

        private string _iconPath;
        private string _label;
        public bool minimized;
        private Button_Worker _worker;
        private string _shortenedLabel;

        private float cachedLabelWidth = -1f;
        private float cachedShortenedLabelWidth = -1f;

        private Texture2D icon;
        public Texture2D Icon
        {
            get
            {
                if (icon == null && _iconPath != null) icon = ContentFinder<Texture2D>.Get(this._iconPath);
                return icon;
            }
        }

        public bool RefreshIcon()
        {
            icon = null;
            return this.Icon != null;
        }

        public ButtonConfig(string defName)
        {
            this.defName = defName;
            minimized = false;
        }

        public ButtonConfig(MainButtonDef def)
        {
            this.defName = def.defName;
            mainButtonDef = def;
            Label = def.label;
            IconPath = def.iconPath;
            minimized = def.minimized;
        }

        public ButtonConfig() { } //Empty constructor to load from ExposeData

        public override void ExposeData()
        {
            Scribe_Values.Look(ref defName, "defName");
            Scribe_Values.Look(ref _iconPath, "iconPath");
            Scribe_Values.Look(ref _label, "label");
            Scribe_Values.Look(ref minimized, "minimized");
        }

        public string IconPath
        {
            get
            {
                return _iconPath;
            }

            set
            {
                _iconPath = value;
                RefreshIcon();
            }
        }
        public string Label
        {
            get
            {
                return _label;
            }

            set
            {
                _label = value.CapitalizeFirst();
                _shortenedLabel = null;
                cachedLabelWidth = 0;
                cachedShortenedLabelWidth = 0;
            }
        }

        public float LabelWidth
        {
            get
            {
                if (cachedLabelWidth < 0f)
                {
                    GameFont font = Text.Font;
                    Text.Font = GameFont.Small;
                    cachedShortenedLabelWidth = Text.CalcSize(Label).x;
                    Text.Font = font;
                }
                return cachedLabelWidth;
            }
        }

        public string ShortenedLabel
        {
            get
            {
                if (this._shortenedLabel == null) this._shortenedLabel = Label.Shorten();
                return ShortenedLabel;
            }
        }

        public float ShortenedLabelWidth
        {
            get
            {
                if(cachedShortenedLabelWidth < 0f)
                {
                    GameFont font = Text.Font;
                    Text.Font = GameFont.Small;
                    cachedShortenedLabelWidth = Text.CalcSize(ShortenedLabel).x;
                    Text.Font = font;
                }
                return cachedShortenedLabelWidth;
            }
        }

        public override bool Configurable => true;
        public override string SettingLabel => defName;

        public override WidgetWorker Worker
        {
            get
            {
                if (_worker == null) _worker = new Workers.Button_Worker(this);
                return _worker;
            }
        }

        public Def Def
        {
            get
            {
                if (mainButtonDef == null) mainButtonDef = DefDatabase<MainButtonDef>.GetNamed(this.defName);
                return mainButtonDef;
            }
        }

        public override void Reset()
        {
            IconPath = ((MainButtonDef)Def).iconPath;
            Label = ((MainButtonDef)Def).label;
            minimized = ((MainButtonDef)Def).minimized;
        }
    }
}
