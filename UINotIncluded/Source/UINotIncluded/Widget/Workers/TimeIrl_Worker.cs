using System;

using UnityEngine;
using Verse;

namespace UINotIncluded.Widget.Workers
{
    internal class TimeIrl_Worker : WidgetWorker
    {
        public override bool Visible { get => Prefs.ShowRealtimeClock; }

        public override bool FixedWidth => true;

        private const float extra = 30f;
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
            this.Margins(ref rect);
            ExtendedToolbar.DoWidgetBackground(rect);
            this.Padding(ref rect);

            Rect iconSpace = DrawIcon(ModTextures.iconWorld, rect.x, rect.y);
            rect.x += iconSpace.width;
            rect.width -= iconSpace.width;
            WidgetRow row = new WidgetRow(rect.x, rect.y, UIDirection.RightThenDown, gap: ExtendedToolbar.interGap);

            String label = DateTime.Now.ToString("HH:mm");

            row.Label(label, rect.width, null, rect.height);
        }
    }
}