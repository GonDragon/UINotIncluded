using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    internal static class ExtendedToolbar
    {
        public static float Height => 35f;
        public static float Width => UI.screenWidth;

        public static float interGap = 0;
        public static float padding = 2;
        public static float margin = 3;

        public static void ExtendedToolbarOnGUI(List<Widget.Configs.ElementConfig> elements, Rect inRect)
        {
            if (elements.Count() == 0) return;
            Settings.BarStyle.DoToolbarBackground(inRect);

            float fixedWidth = 0f;
            int elasticElementsAmount = 0;

            foreach (Widget.Configs.ElementConfig element in elements)
            {
                if (!element.Worker.Visible) continue;
                if (!element.Worker.FixedWidth) elasticElementsAmount++;
                else fixedWidth += element.Worker.Width;
            }

            float elasticSpaceAvaible = Width - fixedWidth;
            float elasticElementWidth = elasticSpaceAvaible / elasticElementsAmount;

            float curX = 0;
            foreach (Widget.Configs.ElementConfig element in elements)
            {
                if (!element.Worker.Visible) continue;
                float eWidth = element.Worker.Width;
                if (!element.Worker.FixedWidth) eWidth = elasticElementWidth;

                Text.Anchor = TextAnchor.MiddleCenter;
                Text.Font = Settings.fontSize;
                Text.WordWrap = false;
                element.Worker.OnGUI(new Rect(curX, inRect.y, eWidth, Height));
                Text.WordWrap = true;
                curX += eWidth;
            }
            Text.Font = GameFont.Small;
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