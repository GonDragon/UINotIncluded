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

        private string widthBuffer;

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

            list.Gap();
            list.Label("Fixed width in px:");

            
            int widthValue = (int)config.width;
            list.IntEntry(ref widthValue, ref widthBuffer);

            if (widthValue < 0)
            {
                config.width = 0;
                widthBuffer = "0";
            }
            else if (widthValue > 1500)
            {
                config.width = 1500;
                widthBuffer = "1500";
            }
            else
            {
                config.width = (float)widthValue;
            }

            list.End();
            if (Widgets.ButtonText(new Rect((inRect.width / 2f) - (EditBlankSpace_Window.ButSize.x / 2f), inRect.height - EditBlankSpace_Window.ButSize.y, EditBlankSpace_Window.ButSize.x, EditBlankSpace_Window.ButSize.y), (string)"DoneButton".Translate())) this.Close();
        }
    }
}