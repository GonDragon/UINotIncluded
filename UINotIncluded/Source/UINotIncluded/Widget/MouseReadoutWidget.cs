using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;

namespace UINotIncluded
{
    public static class MouseReadoutWidget
    {
        public static bool AltInspector = false;

        private static readonly float leftSpace = 15f;
        private static readonly float botSpace = UIManager.ExtendedBarHeight + 10f;
        private static readonly float botSpaceTabsOnTop = 15f;
        private static readonly float maxHeight = 600f;
        private static readonly int uniqueID = 121717; //Random generated unique ID.

        public static void DrawReadout(string label)
        {
            label = label.Trim();
            if (MouseReadoutWidget.AltInspector)
            {
                Vector2 mousePos = Event.current.mousePosition;
                Vector2 size = new Vector2(999f, 999f);
                Rect rect = new Rect(mousePos, size);
                TipSignal tip = new TipSignal(label, uniqueID) { delay = 0 };
                TooltipHandler.TipRegion(rect, tip);
            }
            else if (!Settings.altInspectActive)
            {
                Text.Font = GameFont.Small;
                Text.Anchor = TextAnchor.LowerLeft;
                GUI.color = new Color(1f, 1f, 1f, 0.8f);
                float botY = (float)UI.screenHeight - (Settings.tabsOnTop ? botSpaceTabsOnTop : botSpace);
                Widgets.Label(new Rect(leftSpace, botY - maxHeight, 999f, maxHeight), label);
                Text.Anchor = TextAnchor.UpperLeft;
                GUI.color = Color.white;
            }
        }

        public static string SpeedPercentString(float extraPathTicks) => (13f / (extraPathTicks + 13f)).ToStringPercent();
    }
}
