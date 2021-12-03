using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    public abstract class WidgetWorker
    {
        public float iconSize = 24f;

        public virtual bool Visible => true;
        public virtual bool FixedWidth => false;
        public virtual float Width => 100f;

        public Rect DrawIcon(Texture2D icon, float curX, float curY, string tooltip = null)
        {
            Rect rect = new Rect(curX, curY, iconSize, iconSize);
            GUI.DrawTexture(rect, icon);
            if (!tooltip.NullOrEmpty())
                TooltipHandler.TipRegion(rect, (TipSignal)tooltip);
            return rect;
        }

        public virtual void Margins(ref Rect rect)
        {
            rect = rect.ContractedBy(ExtendedToolbar.margin);
        }

        public abstract void OnGUI(Rect rect);

        public virtual void OpenConfigWindow() { }
        public virtual void Padding(ref Rect rect)
        {
            rect = rect.ContractedBy(ExtendedToolbar.padding);
        }
    }
}