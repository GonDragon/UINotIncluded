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
            Rect scrollviewInRect = new Rect(0, 0, scrollviewRect.width - 20f, buttonspace_heigth * elements.Count());

            Widgets.BeginScrollView(scrollviewRect, ref scroll.pos, scrollviewInRect);

            Rect widgetSpace = new Rect(0f, 0f, inRect.width, buttonspace_heigth);
            DragManager.ManageList(name, elements);

            int n = 0;
            foreach (object element in elements)
            {
                Rect innerSpace = widgetSpace.ContractedBy(contraction);
                DragElement current = new DragElement { pos = n, listname = name, size = innerSpace.size };

                if (CustomButtons.DraggableButton(innerSpace, getLabel(element)) == Widgets.DraggableResult.Dragged) DragManager.DraggStart(current);

                //Rect spaceBefore = new Rect(innerSpace.x, widgetSpace.y - 13f, innerSpace.width, widgetSpace.height);
                //Rect spaceAfter = new Rect(innerSpace.x,widgetSpace.y + 13f, innerSpace.width, widgetSpace.height);
                //else if (DragAndDropWidget.Dragging)
                //{
                //    if (Mouse.IsOver(spaceBefore))
                //    {
                //        Widgets.DrawLineHorizontal(innerSpace.x, widgetSpace.y, widgetSpace.width);
                //        context.current = current;
                //    }
                //    else if (Mouse.IsOver(spaceAfter))
                //    {
                //        Widgets.DrawLineHorizontal(innerSpace.x, widgetSpace.y + buttonspace_heigth, widgetSpace.width);
                //        context.current = current;
                //    }
                //}

                widgetSpace.y += buttonspace_heigth;
                n++;
            }

            Widgets.EndScrollView();
        }
    }

    public struct DragElement
    {
        internal int pos;
        internal string listname;
        internal Vector2 size;
    }
}