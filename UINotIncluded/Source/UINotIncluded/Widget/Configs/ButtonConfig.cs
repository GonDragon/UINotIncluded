using RimWorld;
using UINotIncluded.Widget.Workers;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget.Configs
{
    public class ButtonConfig : ElementConfig
    {
        private MainButtonDef mainButtonDef;
        public string defName;

        private string _iconPath;
        private string _label;
        public bool minimized;
        public bool hideLabel = false;
        private Button_Worker _worker;
        private string _shortenedLabel;

        private float cachedLabelWidth = -1f;
        private float cachedShortenedLabelWidth = -1f;

        private Texture2D icon;
        public Texture2D Icon => icon;

        public void RefreshIcon()
        {
            if (_iconPath != null) icon = ContentFinder<Texture2D>.Get(this._iconPath);
            else icon = null;
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

        public ButtonConfig()
        { } //Empty constructor to load from ExposeData

        public override void ExposeData()
        {
            Scribe_Values.Look(ref defName, "defName");
            Scribe_Values.Look(ref _iconPath, "iconPath");
            Scribe_Values.Look(ref _label, "label");
            Scribe_Values.Look(ref minimized, "minimized");
            Scribe_Values.Look(ref hideLabel, "hideLabel", false);
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
                RefreshCache();
            }
        }

        public void RefreshCache()
        {
            this._shortenedLabel = _label.Shorten();
            GameFont font = Text.Font;
            Text.Font = GameFont.Small;
            cachedLabelWidth = Text.CalcSize(Label).x;
            cachedShortenedLabelWidth = Text.CalcSize(ShortenedLabel).x;
            Text.Font = font;
        }

        public float LabelWidth => cachedLabelWidth;

        public string ShortenedLabel => _shortenedLabel;

        public float ShortenedLabelWidth => cachedShortenedLabelWidth;

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

        public virtual Def Def
        {
            get
            {
                if (mainButtonDef == null) mainButtonDef = DefDatabase<MainButtonDef>.GetNamedSilentFail(this.defName);
                return mainButtonDef;
            }
        }

        public override void Reset()
        {
            IconPath = ((MainButtonDef)Def).iconPath;
            Label = ((MainButtonDef)Def).label;
            minimized = ((MainButtonDef)Def).minimized;
            hideLabel = false;
            RefreshCache();
        }

        public override bool Equivalent(ElementConfig other)
        {
            if (other.GetType() != typeof(ButtonConfig)) return false;

            return ((ButtonConfig)other).defName == defName;
        }
    }
}