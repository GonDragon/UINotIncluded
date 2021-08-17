using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    class ExtendedToolbar
    {
        public float width;
        public float height = 35f;
        public float x;
        public float y;
        public WidgetRow row = new WidgetRow();
        public ExtendedToolbar(float x, float y, float width)
        {
            this.x = x;
            this.y = y;
            this.width = width;
        }

        public void ExtendedToolbarOnGUI()
        {
            Rect rect = new Rect(this.x, this.y, this.width, this.height);
            GUI.DrawTexture(rect, SolidColorMaterials.NewSolidColorTexture(new ColorInt(111, 111, 111, (int)byte.MaxValue).ToColor));
            Widgets.DrawAtlas(new Rect(this.x + 1f, this.y + 1f, this.width - 2f, this.height - 2f), ContentFinder<Texture2D>.Get("GD/UI/ClockBG"));

            float rowHeight = this.height - 8f;
            row.Init(this.x + 6f, this.y + 5f, UIDirection.RightThenDown);
            Weather.DoWeatherGUI(row, rowHeight);

            /*
            float groupHeight = this.height - 8f;
            GUI.BeginGroup(new Rect(this.x + 4f, this.y + 4f, this.width - 8f, groupHeight));

            float groupX = 0;
            float groupY = 0;            
            size = Weather.DoWeatherGUI(groupX, groupY, groupHeight);
            groupX += size + 4f;
            GUI.EndGroup();
            */

        }

        public static void DoToolbarBackground(Rect rect)
        {
            Rect rect1 = new Rect(rect.x, rect.y-2, rect.width, rect.height+2);
            Widgets.DrawAtlas(rect1, ContentFinder<Texture2D>.Get("GD/UI/ClockSCR"));
        }
    }
}
