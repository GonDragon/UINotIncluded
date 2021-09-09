﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Verse;
using RimWorld;

namespace UINotIncluded.Widget
{
    class RealTimeWidget : ExtendedWidget
    {
        public override float Width => (float)Math.Round(_width + 11.66f * (float)Settings.fontSize);

        private static readonly float _width = 75f;

        public override void OnGUI(Rect rect)
        {
            ExtendedToolbar.DoToolbarBackground(rect);
            Rect space = rect.ContractedBy(ExtendedToolbar.padding);

            Rect iconSpace = DrawIcon(ModTextures.iconWorld, space.x);
            space.x += iconSpace.width;
            space.width -= iconSpace.width;
            WidgetRow row = new WidgetRow(space.x, space.y, UIDirection.RightThenDown,gap: ExtendedToolbar.interGap);

            String label = DateTime.Now.ToString("HH:mm");

            row.Label(label, space.width, null, space.height);
        }
    }
}
