using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    internal static class CustomLists
    {
        private static readonly float buttonspace_heigth = 40f;
        private static readonly float contraction = 4f;

        public static void Draggable(string name, Rect inRect, List<Designator> elements, Func<object, string> getLabel)
        {
            Widgets.DrawMenuSection(inRect);

            ScrollInstance scroll = ScrollManager.GetInstance(name.GetHashCode());
            Rect scrollviewRect = new Rect(inRect);
            Rect scrollviewInRect = new Rect(0, 0, scrollviewRect.width - 20f, buttonspace_heigth * elements.Count() + contraction * 2);

            Widgets.BeginScrollView(scrollviewRect, ref scroll.pos, scrollviewInRect);

            Rect widgetSpace = new Rect(0f, contraction, inRect.width - contraction, buttonspace_heigth);
            DragManager.ManageList(name, elements);

            int n = 0;
            foreach (object element in elements)
            {
                Rect innerSpace = widgetSpace.ContractedBy(contraction);
                DragElement current = new DragElement { pos = n, listname = name, size = innerSpace.size };
                bool shouldDrawButton = true;

                Rect spaceBefore = new Rect(innerSpace.x, widgetSpace.y - 13f, innerSpace.width, widgetSpace.height);
                Rect spaceAfter = new Rect(innerSpace.x, widgetSpace.y + 13f, innerSpace.width, widgetSpace.height);


                if (DragManager.Dragging)
                {
                    if (DragManager.Dragged?.listname == name && DragManager.Dragged?.pos == n) shouldDrawButton = false;
                    bool beforeDragged = DragManager.Dragged?.pos > n;
                    if (Mouse.IsOver(spaceBefore))
                    {
                        Widgets.DrawLineHorizontal(innerSpace.x, widgetSpace.y, widgetSpace.width);
                        DragManager.hoveringOver = current;
                    }
                    else if (Mouse.IsOver(spaceAfter))
                    {
                        Widgets.DrawLineHorizontal(innerSpace.x, widgetSpace.y + buttonspace_heigth, widgetSpace.width);
                        DragManager.hoveringOver = new DragElement { pos = n + 1, listname = name, size = innerSpace.size };
                    }
                }

                if (shouldDrawButton && CustomButtons.DraggableButton(innerSpace, getLabel(element)) == Widgets.DraggableResult.Dragged) DragManager.DraggStart(current);

                widgetSpace.y += buttonspace_heigth;
                n++;
            }

            Widgets.EndScrollView();
        }
    }
}