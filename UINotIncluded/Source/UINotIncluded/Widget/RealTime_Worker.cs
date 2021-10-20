using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Verse;
using RimWorld;

namespace UINotIncluded.Widget
{
    class RealTime_Worker : WidgetWorker
    {
        public override bool WidgetVisible { get => Prefs.ShowRealtimeClock; }
        public override float GetWidth()
        {
            return (float)Math.Round(def.minWidth + 11.66f * (float)Settings.fontSize);
        }

        public override void OnGUI(Rect rect)
        {
            this.Margins(ref rect);
            ExtendedToolbar.DoWidgetBackground(rect);
            this.Padding(ref rect);

            Rect iconSpace = DrawIcon(ModTextures.iconWorld, rect.x, rect.y);
            rect.x += iconSpace.width;
            rect.width -= iconSpace.width;
            WidgetRow row = new WidgetRow(rect.x, rect.y, UIDirection.RightThenDown,gap: ExtendedToolbar.interGap);

            String label = DateTime.Now.ToString("HH:mm");

            row.Label(label, rect.width, null, rect.height);
        }
    }
}
