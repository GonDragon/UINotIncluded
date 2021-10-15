using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace UINotIncluded
{
    public static class DragMemory
    {
        public static bool Dragging => _dragged != null;
        public static DragElement? _dragged;
        public static bool Hovering => hoveringOver != null;
        public static DragElement? hoveringOver;

        public static DragElement? Dragged => _dragged;
    }
    public class DragManager<T>
    {
        public Action OnUpdate;
        public Func<T, string> GetLabel;
        private readonly Action<T> _OnClick;

        private readonly Dictionary<string, List<T>> _managed_dragable_lists = new Dictionary<string, List<T>>();

        public DragManager(Action OnUpdate, Func<T, string> GetLabel)
        {
            this.OnUpdate = OnUpdate;
            this.GetLabel = GetLabel;
            this._OnClick = (T element) => { };
        }

        public DragManager(Action OnUpdate, Func<T, string> GetLabel, Action<T> OnClick)
        {
            this.OnUpdate = OnUpdate;
            this.GetLabel = GetLabel;
            this._OnClick = OnClick;

        }

        public void ManageList(string name, List<T> list)
        {
            _managed_dragable_lists[name] = list;
        }

        public void Update()
        {
            DrawGhost();
            if (DraggStops())
            {
                if (DragMemory.Dragging)
                {
                    MoveDragged();
                    DragMemory._dragged = null;
                    OnUpdate();
                }
            }
        }

        public void DraggStart(DragElement element)
        {
            DragMemory._dragged = element;
        }

        public bool DraggStops()
        {
            return Event.current.rawType == EventType.MouseUp;
        }

        public void OnClick(DragElement element)
        {
            this._OnClick(_managed_dragable_lists[element.listname][element.pos]);
        }

        public void DrawGhost()
        {
            if (Event.current.type != EventType.Repaint || !DragMemory.Dragging || !_managed_dragable_lists.ContainsKey(DragMemory.Dragged?.listname)) return;
            float offset = (float)Math.Floor((float)DragMemory.Dragged?.size.y / 2);
            Vector2 pos = Event.current.mousePosition - new Vector2(offset, offset);
            CustomButtons.DraggableButtonGhost(new Rect(pos, (Vector2)DragMemory.Dragged?.size), GetLabel(_managed_dragable_lists[DragMemory.Dragged?.listname][(int)DragMemory.Dragged?.pos]));
        }

        public void MoveDragged()
        {
            if (!DragMemory.Dragging || !DragMemory.Hovering) return;
            List<T> origin = _managed_dragable_lists[DragMemory._dragged?.listname];
            List<T> destination = _managed_dragable_lists[DragMemory.hoveringOver?.listname];
            
            bool sameList = DragMemory._dragged?.listname == DragMemory.hoveringOver?.listname;
            int removePos = sameList ? ((int)DragMemory.hoveringOver?.pos > (int)DragMemory._dragged?.pos ? (int)DragMemory._dragged?.pos : (int)DragMemory._dragged?.pos+1) : (int)DragMemory._dragged?.pos;

            T temp = origin[(int)DragMemory._dragged?.pos];
            destination.Insert((int)DragMemory.hoveringOver?.pos, temp);
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