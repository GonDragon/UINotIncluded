using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;

namespace UINotIncluded.Widget
{
    static class DragList
    {
        static private Dictionary<string, int> lists = new Dictionary<string, int>();
        static private List<String> elements;

        public static void DoList(string name, Rect inRect)
        {
            if(elements == null)
            {
                elements = Settings.GetDesignationList(DesignationConfig.hidden).ListFullCopy();
                elements.AddRange(Settings.GetDesignationList(DesignationConfig.left));
                elements.AddRange(Settings.GetDesignationList(DesignationConfig.main));
                elements.AddRange(Settings.GetDesignationList(DesignationConfig.right));
            }

            if (!lists.ContainsKey(name))
            {
                lists[name] = DragAndDropWidget.NewGroup();
            
            }

            int dragndropID = lists[name];


            Widgets.DrawMenuSection(inRect);
            GUI.BeginGroup(inRect);
            Rect widgetSpace = new Rect(0f, 0f, inRect.width, 26f);
            DragContext context = DragContext.Instance;
            context.ManageDraggableList(dragndropID, elements);
            DragAndDropWidget.DropArea(dragndropID,new Rect(new Vector2(0f,0f),inRect.size),context.OnDrop(),context);

            int n = 0;
            foreach (String element in elements)
            {
                DragElement current = new DragElement { name = element, pos = n, dropID = dragndropID, space = widgetSpace };

                Rect spaceBefore = new Rect(widgetSpace.x, widgetSpace.y - 13f, widgetSpace.width, widgetSpace.height);
                Rect spaceAfter = new Rect(widgetSpace.x,widgetSpace.y + 13f,widgetSpace.width, widgetSpace.height);
                if (DragAndDropWidget.Draggable(dragndropID, widgetSpace, current, onStartDragging: context.OnDragStart(current)))
                {
                    Vector2 mousePos = Event.current.mousePosition;
                    Vector2 ghostPos = new Vector2(mousePos.x,mousePos.y-13f);
                    Widgets.Label(new Rect(ghostPos, widgetSpace.size),current.name);

                    if(Mouse.IsOver(widgetSpace) || Mouse.IsOver(spaceAfter))
                    {
                        Widgets.Label(widgetSpace, current.name);
                        context.current = current;
                    } else
                    {
                        Color temp = GUI.color;
                        GUI.color = new Color(1f, 1f, 1f, 0.5f);
                        Widgets.Label(widgetSpace, current.name);
                        GUI.color = temp;
                    }
                }
                else
                {
                    Widgets.Label(widgetSpace, current.name);

                    if (DragAndDropWidget.Dragging)
                    {
                        DragElement dragged = (DragElement)context.DragElement;

                        if (current.pos < dragged.pos && Mouse.IsOver(spaceBefore))
                        {
                            Widgets.DrawLineHorizontal(widgetSpace.x, widgetSpace.y, widgetSpace.width / 2);
                            context.current = current;
                        }
                        else if (current.pos > dragged.pos && Mouse.IsOver(spaceAfter)) 
                        {
                            Widgets.DrawLineHorizontal(widgetSpace.x, widgetSpace.y + 26f, widgetSpace.width / 2);
                            context.current = current;
                        }
                    }
                }
                widgetSpace.y += 26f;
                n++;
            }
            GUI.EndGroup();

        }

        private struct DragElement
        {
            public string name;
            public int pos;
            public int dropID;
            public Rect space;
        }

        private sealed class DragContext
        {
            private static DragContext instance = null;
            private static readonly object padlock = new object();
            DragContext()
            {
            }

            public static DragContext Instance
            {
                get
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DragContext();
                        }
                        return instance;
                    }
                }
            }

            public DragElement? current;
            private readonly Dictionary<int,List<string>> _managed_dragable_lists = new Dictionary<int, List<string>>();

            private DragElement? _dragElement;
            public DragElement? DragElement
            {
                get
                {
                    try
                    {
                        return (DragElement)DragAndDropWidget.CurrentlyDraggedDraggable();
                    }
                    catch
                    {
                        return _dragElement;
                    }
                }

                set
                {
                    _dragElement = value;
                }
            }

            public void ManageDraggableList(int DragID, List<string> list)
            {
                _managed_dragable_lists[DragID] = list;
            }

            public Action OnDragStart(DragElement element)
            {
                return () => this.DragElement = element;
            }

            public Action<object> OnDrop()
            {
                return (object element) => {
                    UINI.Log("Im Here");
                    if (this.current == null) return;
                    DragElement draggable = (DragElement)element;
                    DragElement current = (DragElement)this.current;
                    UINI.Log(String.Format("Element position: {1} -> Move position: {0}.", current.pos, ((DragElement)element).pos));

                    try
                    {
                        String temp = (String)(_managed_dragable_lists[draggable.dropID][draggable.pos]);
                        _managed_dragable_lists[draggable.dropID].RemoveAt(draggable.pos);
                        _managed_dragable_lists[current.dropID].Insert(current.pos,temp);
                    }
                    catch (KeyNotFoundException e)
                    {
                        UINI.Error(String.Format("Atempted drop on/from unmanaged list.\n{0}",e.StackTrace));
                    }

                    this.DragElement = null;
                };
            }
        }
    }
}
