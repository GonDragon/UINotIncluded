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
    public static class DragManager
    {
        public static bool Dragging => _dragged != null;
        private static object _dragged;


        private static readonly Dictionary<string, List<Designator>> _managed_dragable_lists = new Dictionary<string, List<Designator>>();

        public static void ManageList(string name, List<Designator> list)
        {
            _managed_dragable_lists[name] = list;
        }

        public static void DraggStart(object element)
        {
            _dragged = element;
        }

        public static bool DraggStops()
        {
            if (Event.current.isMouse && Event.current.type == EventType.MouseUp)
            {
                Event.current.Use();
                return true;
            }
            return false;
        }

        public static object Dragged => _dragged;

        public static void UseDragged()
        {
            _dragged = null;
        }


        public static void DrawGhost()
        {
        }
    }
}
