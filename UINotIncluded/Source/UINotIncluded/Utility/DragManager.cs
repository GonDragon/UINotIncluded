using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace UINotIncluded
{
    public static class DragManager
    {
        public static bool Dragging => _dragged != null;
        private static DragElement? _dragged;
        public static bool Hovering => hoveringOver != null;
        public static DragElement? hoveringOver;

        private static readonly Dictionary<string, List<Designator>> _managed_dragable_lists = new Dictionary<string, List<Designator>>();

        public static void ManageList(string name, List<Designator> list)
        {
            _managed_dragable_lists[name] = list;
        }

        public static void DraggStart(DragElement element)
        {
            _dragged = element;
        }

        public static bool DraggStops()
        {
            return Event.current.rawType == EventType.MouseUp;
        }

        public static DragElement? Dragged => _dragged;

        public static void UseDragged()
        {
            _dragged = null;
        }

        public static void DrawGhost()
        {
            if (Event.current.type != EventType.Repaint || !Dragging || !_managed_dragable_lists.ContainsKey(Dragged?.listname)) return;
            float offset = (float)Math.Floor((float)Dragged?.size.y / 2);
            Vector2 pos = Event.current.mousePosition - new Vector2(offset, offset);
            CustomButtons.DraggableButtonGhost(new Rect(pos, (Vector2)Dragged?.size), _managed_dragable_lists[Dragged?.listname][(int)Dragged?.pos].defaultLabel);
        }

        public static void MoveDragged()
        {
            if (!Dragging || !Hovering) return;
            List<Designator> origin = _managed_dragable_lists[_dragged?.listname];
            List<Designator> destination = _managed_dragable_lists[hoveringOver?.listname];
            
            bool sameList = _dragged?.listname == hoveringOver?.listname;
            int removePos = sameList ? ((int)hoveringOver?.pos > (int)_dragged?.pos ? (int)_dragged?.pos : (int)_dragged?.pos+1) : (int)_dragged?.pos;

            Designator temp = origin[(int)_dragged?.pos];
            destination.Insert((int)hoveringOver?.pos, temp);
            origin.RemoveAt(removePos);
        }
    }

    public struct DragElement
    {
        public int pos;
        public string listname;
        public Vector2 size;
    }
}