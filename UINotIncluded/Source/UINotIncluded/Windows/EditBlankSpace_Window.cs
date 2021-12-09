using System;
using UINotIncluded.Widget.Configs;
using UnityEngine;
using Verse;

namespace UINotIncluded.Windows
{
    internal class EditBlankSpace_Window : Window
    {
        private readonly Widget.Configs.BlankSpaceConfig config;
        private static readonly Vector2 ButSize = new Vector2(150f, 38f);

        public EditBlankSpace_Window(BlankSpaceConfig config)
        {
            this.config = config;
        }

        public override Vector2 InitialSize => new Vector2(400f, 400f);

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard list = new Listing_Standard();
            list.Begin(inRect);
            list.CheckboxLabeled("Fixed Width", ref config.fixedWidth);
            list.Label(string.Format("Width ({0}px)", Math.Round(config.width).ToString()));
            config.width = list.Slider(config.width, 0f, 1000f);
            list.End();
            if (Widgets.ButtonText(new Rect((inRect.width / 2f) - (EditBlankSpace_Window.ButSize.x / 2f), inRect.height - EditBlankSpace_Window.ButSize.y, EditBlankSpace_Window.ButSize.x, EditBlankSpace_Window.ButSize.y), (string)"DoneButton".Translate())) this.Close();
        }
    }
}