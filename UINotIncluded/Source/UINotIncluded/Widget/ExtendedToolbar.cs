using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    internal static class ExtendedToolbar
    {
        public static float Height => Settings.barsHeight;
        public static float Width => UI.screenWidth;

        public static float interGap = 0;
        public static float padding = 2;
        public static float margin = 3;

        public static void ExtendedToolbarOnGUI(List<Widget.Configs.ElementConfig> elements, Rect inRect)
        {
#if DEBUG
            string key = "Draw Toolbar";
            Analyzer.Profiling.Profiler profiler = Analyzer.Profiling.ProfileController.Start(key);
#endif

            if (elements.Count() == 0) return;
            Settings.BarStyle.DoToolbarBackground(inRect);

            float fixedWidth = 0f;
            int elasticElementsAmount = 0;

            foreach (Widget.Configs.ElementConfig element in elements)
            {
                if (!element.Worker.Visible()) continue;
                if (!element.Worker.FixedWidth) elasticElementsAmount++;
                else fixedWidth += element.Worker.Width;
            }

            float elasticSpaceAvaible = Width - fixedWidth;
            float elasticElementWidth = elasticSpaceAvaible / elasticElementsAmount;

            float curX = 0;
            foreach (Widget.Configs.ElementConfig element in elements)
            {
                if (!element.Worker.Visible()) continue;
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
#if DEBUG
            Analyzer.Profiling.ProfileController.Stop(key);
#endif
        }

        public static void DoWidgetBackground(Rect rect)
        {
            Settings.BarStyle.DoWidgetBackground(rect);
        }

        public static void VUIE_ExtendedToolbarOnGUI(List<Widget.Configs.ElementConfig> elements, Rect inRect, Utility.VUIEhelper vuie)
        {
#if DEBUG
            string key = "Draw Toolbar";
            Analyzer.Profiling.Profiler profiler = Analyzer.Profiling.ProfileController.Start(key);
#endif

            VUIE.DragDropManager<Widget.Configs.ElementConfig> manager = (VUIE.DragDropManager<Widget.Configs.ElementConfig>)vuie.dragDropManager;

            if (elements.Count() == 0) return;
            Settings.BarStyle.DoToolbarBackground(inRect);

            float fixedWidth = 0f;
            int elasticElementsAmount = 0;

            foreach (Widget.Configs.ElementConfig element in elements)
            {
                if (!element.Worker.Visible()) continue;
                if (!element.Worker.FixedWidth) elasticElementsAmount++;
                else fixedWidth += element.Worker.Width;
            }

            if(Mouse.IsOver(inRect) && manager.DraggingNow)
            {
                if(!manager.Dragging.Worker.FixedWidth) elasticElementsAmount++;
                else fixedWidth += manager.Dragging.Worker.Width;
            }

            float elasticSpaceAvaible = Width - fixedWidth;
            float elasticElementWidth = elasticSpaceAvaible / elasticElementsAmount;

            if (Mouse.IsOver(inRect))
            {
                Utility.VUIEhelper.buttonWidth = elasticElementWidth;
            }

            float curX = 0;
            foreach (Widget.Configs.ElementConfig element in elements.ListFullCopy())
            {
                if (!element.Worker.Visible()) continue;
                float eWidth = element.Worker.Width;
                if (!element.Worker.FixedWidth) eWidth = elasticElementWidth;
                Rect elementRect = new Rect(curX, inRect.y, eWidth, Height);

                if (Mouse.IsOver(elementRect) && manager.DraggingNow)
                {
                    vuie.mouseoverIdx = elements.IndexOf(element);
                    elementRect.x += manager.Dragging.Worker.FixedWidth ? manager.Dragging.Worker.Width : elasticElementWidth;
                    curX += manager.Dragging.Worker.FixedWidth ? manager.Dragging.Worker.Width : elasticElementWidth;
                }

                if (Mouse.IsOver(elementRect) && Input.GetMouseButtonDown(1))
                {
                    if(element.Configurable)
                    {
                        element.Worker.OpenConfigWindow();
                    }                    
                    Event.current.Use();
                }

                Text.Anchor = TextAnchor.MiddleCenter;
                Text.Font = Settings.fontSize;
                Text.WordWrap = false;
                element.Worker.OnGUI(elementRect);
                Text.WordWrap = true;

                if (manager.TryStartDrag(element, elementRect)) elements.Remove(element);

                curX += eWidth;
            }

            manager.DropLocation(inRect, null, element =>
            {
                elements.Insert(vuie.mouseoverIdx, element);
                return true;
            });

            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
            

#if DEBUG
            Analyzer.Profiling.ProfileController.Stop(key);
#endif
        }
    }

    public enum ToolbarPosition
    {
        top,
        bottom
    }
}