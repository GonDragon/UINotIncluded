using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    static class ExtendedToolbar
    {
        public static float Height => 35f;
        public static float Width => UI.screenWidth;

        public static float interGap = 0;
        public static float padding = 2;
        public static float margin = 3;

        public static GameFont FontSize => Settings.fontSize;

        public static void ExtendedToolbarOnGUI(List<ToolbarElementWrapper> elements, Rect inRect)
        {
            if (elements.Count() == 0) return;

            //Widgets.DrawAtlas(new Rect(0, 0, Width, Height), ModTextures.toolbarBackground);
            Settings.BarStyle.DoToolbarBackground(inRect);

            float fixedWidth = 0f;
            int elasticElementsAmount = 0;

            foreach(ToolbarElementWrapper element in elements)
            {
                if (!element.Visible) continue;
                if (!element.FixedWidth) elasticElementsAmount++;
                else fixedWidth += element.Width;
            }

            float elasticSpaceAvaible = Width - fixedWidth;
            float elasticElementWidth = elasticSpaceAvaible / elasticElementsAmount;

            float curX = 0;
            foreach(ToolbarElementWrapper element in elements)
            {
                if (!element.Visible) continue;
                float eWidth = element.Width;
                if (eWidth < 0) eWidth = elasticElementWidth;

                Text.Anchor = TextAnchor.MiddleCenter;
                Text.Font = ExtendedToolbar.FontSize;
                element.OnGUI(new Rect(curX, inRect.y, eWidth,Height));
                curX += eWidth;
            }
            Text.Anchor = TextAnchor.UpperLeft;
        }

        public static void DoWidgetBackground(Rect rect)
        {
            Settings.BarStyle.DoWidgetBackground(rect);
        }
    }

    public enum ToolbarPosition
    {
        top,
        bottom
    }
}
