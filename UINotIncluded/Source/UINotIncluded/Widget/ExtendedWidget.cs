using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using UnityEngine;

namespace UINotIncluded.Widget
{
    public abstract class ExtendedWidget
    {
        public abstract float MinimunWidth { get; }
        public abstract float MaximunWidth { get; }

        public float iconSize = 24f;

        public abstract void OnGUI(Rect rect);

        public Rect DrawIcon(Texture2D icon, float curX, string tooltip = null)
        {
            Rect rect = new Rect(curX, 4f, iconSize, iconSize);
            GUI.DrawTexture(rect, icon);
            if (!tooltip.NullOrEmpty())
                TooltipHandler.TipRegion(rect, (TipSignal)tooltip);
            return rect;
        }
    }
}
