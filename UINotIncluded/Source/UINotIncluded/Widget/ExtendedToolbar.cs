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
        public static float Width
        {
            get
            {
                float sum = 0;
                foreach(ExtendedWidget widget in widgetList)
                {
                    sum += widget.Width;
                }
                return sum;
            }
        }

        public static float interGap = 0;
        public static float padding = 2;
        public static float margin = 5;


        private static readonly List<ExtendedWidget> widgetList = new List<ExtendedWidget>();

        private static readonly ExtendedWidget weather = new Weather();
        private static readonly ExtendedWidget realtime = new RealTimeWidget();
        private static readonly ExtendedWidget datetime = new TimeWidget();
        private static readonly ExtendedWidget timespeed = new Timespeed();

        public static GameFont FontSize => UINotIncludedSettings.fontSize;

        public static void ExtendedToolbarOnGUI(float x, float y)
        {
            widgetList.Clear();

            widgetList.Add(weather);
            if (Prefs.ShowRealtimeClock) widgetList.Add(realtime);
            widgetList.Add(datetime);
            widgetList.Add(timespeed);

            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = FontSize;

            Widgets.DrawAtlas(new Rect(x, y, Width, Height), ModTextures.toolbarBackground);
            ExtendedToolbar.DrawBar(new Rect(x+2, y+2, Width-2, Height-4), widgetList);

            Text.Anchor = TextAnchor.UpperLeft;

        }

        public static void DrawBar(Rect rect, List<ExtendedWidget> widgets)
        {
            int n_widgets = widgets.Count();
            int[] minSizes = new int[n_widgets];
            int totalMinSize = 0;

            for(int i=0; i < n_widgets; i++)
            {
                minSizes[i] = (int)Math.Ceiling(widgets[i].Width);
                totalMinSize += minSizes[i];
            }


            float[] actualSizes = new float[n_widgets];
            float occupiedSpace = 0;
            float totalAvaibleWidth = rect.width - (margin * (n_widgets + 1));
            for (int j=0; j < (n_widgets - 1); j++)
            {
                actualSizes[j] = (float)Math.Ceiling((((float)minSizes[j]) / totalMinSize) * totalAvaibleWidth);
                occupiedSpace += actualSizes[j];
            }
            actualSizes[n_widgets - 1] = totalAvaibleWidth - occupiedSpace;

            GUI.BeginGroup(rect);
            float curX = margin;
            int cur = 0;

            try
            {
                foreach (ExtendedWidget widget in widgets)
                {
                    widget.OnGUI(new Rect(curX, 0, actualSizes[cur], rect.height));
                    curX += actualSizes[cur] + margin;
                    cur++;
                }
            }
            catch (Exception e)
            {
                UINotIncludedStatic.ErrorOnce(String.Format("Error catched on the widget {0} named {1}. The error was: {2}.\nStack trace:\n{3}",cur.ToString(),widgets[cur].GetType().ToString(),e.ToString(),e.StackTrace),"Rendering Widgets");
            }
            finally
            {
                GUI.EndGroup();
            }

            
            
        }

        public static void DoToolbarBackground(Rect rect)
        {
            Widgets.DrawAtlas(rect, ModTextures.toolbarWidgetBackground);
        }
    }
}
