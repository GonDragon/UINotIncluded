using Verse;

namespace UINotIncluded.Widget.Configs
{
    internal class BlankSpaceConfig : ElementConfig
    {
        public bool fixedWidth = false;
        public float width = 100f;
        public override bool Configurable => true;

        public override void Reset()
        {
            fixedWidth = false;
            width = 100f;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref fixedWidth, "fixedWidth", false);
            Scribe_Values.Look(ref width, "width", 100f);
        }

        private Workers.BlankSpace_Worker _worker;
        public override string SettingLabel => "Blank Space";

        public override bool Repeatable => true;

        public override WidgetWorker Worker
        {
            get
            {
                if (_worker == null) _worker = new Workers.BlankSpace_Worker(this);
                return _worker;
            }
        }
    }
}