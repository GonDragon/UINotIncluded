using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    class Clock
    {
        public float width;
        public float height = 35f;
        public float x;
        public float y;
        public Clock(float x, float y, float width)
        {
            this.x = x;
            this.y = y;
            this.width = width;
        }

        public void ClockOnGUI()
        {
            Text.Font = GameFont.Small;
            this.DrawBody();
        }

        private void DrawBody()
        {
            Rect rect = new Rect(this.x, this.y, this.width, this.height);
            GUI.DrawTexture(rect, SolidColorMaterials.NewSolidColorTexture(new ColorInt(111, 111, 111, (int)byte.MaxValue).ToColor));
            Widgets.DrawAtlas(new Rect(1f, 1f, this.width - 2f, this.height - 2f), ContentFinder<Texture2D>.Get("GD/UI/ClockBG"));
            Widgets.DrawAtlas(new Rect(5f, 3f, this.width - 10f, this.height - 6f), ContentFinder<Texture2D>.Get("GD/UI/ClockSCR"));
        }
    }
}
