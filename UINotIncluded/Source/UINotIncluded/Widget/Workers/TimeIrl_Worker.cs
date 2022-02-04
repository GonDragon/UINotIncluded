using System;

using UnityEngine;
using Verse;

namespace UINotIncluded.Widget.Workers
{
    internal class TimeIrl_Worker : WidgetWorker
    {
        public override bool Visible { get => Prefs.ShowRealtimeClock; }

        public override bool FixedWidth => true;

        private float extra => this.IconSize + 6f;
        private float cacheWidth = -1f;
        private GameFont fontCache;
        public override float Width
        {
            get
            {
                if (cacheWidth < 0 || fontCache != Settings.fontSize)
                {
                    GameFont font = Text.Font;
                    Text.Font = Settings.fontSize;
                    cacheWidth = (float)Math.Round(Text.CalcSize("00:00 hs").x);
                    fontCache = Settings.fontSize;
                    Text.Font = font;
                }
                return cacheWidth + extra;
            }
        }

        public override void OnGUI(Rect rect)
        {
            Rect innerRect = new Rect(rect);
            this.Margins(ref innerRect);
            ExtendedToolbar.DoWidgetBackground(innerRect);
            this.Padding(ref innerRect);

            Rect iconSpace = DrawIcon(ModTextures.iconWorld, innerRect.x, innerRect.y);
            innerRect.x += iconSpace.width;
            innerRect.width -= iconSpace.width;
            WidgetRow row = new WidgetRow(innerRect.x, rect.y, UIDirection.RightThenDown, gap: ExtendedToolbar.interGap);

            String label = DateTime.Now.ToString("HH:mm");

            Text.Anchor = TextAnchor.MiddleLeft;
            row.Label(label, innerRect.width, null, rect.height);
            Text.Anchor = TextAnchor.UpperLeft;
        }
    }
}