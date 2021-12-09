using Verse;

namespace UINotIncluded.Widget
{
    public class ExtendedWidgetDef : Def
    {
        public float minWidth = 0f;
        public bool multipleInstances = false;

        public override TaggedString LabelCap
        {
            get
            {
                if (this.label == null) this.label = defName;

                return base.LabelCap + " (widget)";
            }
        }
    }
}