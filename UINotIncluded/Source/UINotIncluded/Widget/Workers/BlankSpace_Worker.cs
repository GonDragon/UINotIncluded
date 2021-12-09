using UINotIncluded.Widget.Configs;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget.Workers
{
    internal class BlankSpace_Worker : WidgetWorker
    {
        private Configs.BlankSpaceConfig config;

        public BlankSpace_Worker(BlankSpaceConfig config)
        {
            this.config = config;
        }

        public override void OpenConfigWindow()
        {
            Find.WindowStack.Add(new Windows.EditBlankSpace_Window(config));
        }

        public override bool FixedWidth => config.fixedWidth;

        public override float Width => config.width;

        public override void OnGUI(Rect rect)
        { }
    }
}