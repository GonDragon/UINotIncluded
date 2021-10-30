using System;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    public abstract class WidgetWorker
    {
        public ExtendedWidgetDef def;
        public float iconSize = 24f;
        public virtual BarElementMemory CreateMemory => new EmptyMemory();

        public virtual bool WidgetVisible => true;

        public virtual Action ConfigAction(BarElementMemory memory)
        {
            return null;
        }

        public Rect DrawIcon(Texture2D icon, float curX, float curY, string tooltip = null)
        {
            Rect rect = new Rect(curX, curY, iconSize, iconSize);
            GUI.DrawTexture(rect, icon);
            if (!tooltip.NullOrEmpty())
                TooltipHandler.TipRegion(rect, (TipSignal)tooltip);
            return rect;
        }

        public virtual bool FixedWidth(BarElementMemory memory)
        {
            return true;
        }

        public virtual float GetWidth()
        {
            return def.minWidth;
        }

        public virtual float GetWidth(BarElementMemory memory)
        {
            return GetWidth();
        }

        public virtual string LabelCap(BarElementMemory memory)
        {
            return def.LabelCap;
        }

        public virtual void Margins(ref Rect rect)
        {
            rect = rect.ContractedBy(ExtendedToolbar.margin);
        }

        public abstract void OnGUI(Rect rect, BarElementMemory memory);
        public virtual void Padding(ref Rect rect)
        {
            rect = rect.ContractedBy(ExtendedToolbar.padding);
        }
    }
}