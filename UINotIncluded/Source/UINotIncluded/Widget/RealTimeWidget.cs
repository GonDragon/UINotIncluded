using System;
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
        public override float MinimunWidth => 75f;

        public override float MaximunWidth => 100f;

        public override void OnGUI(Rect rect)
        {
            ExtendedToolbar.DoToolbarBackground(rect);
            Rect space = rect.ContractedBy(ExtendedToolbar.padding);
            WidgetRow row = new WidgetRow(space.x, space.y, UIDirection.RightThenDown, space.width, ExtendedToolbar.interGap);

            Rect iconSpace = DrawIcon(ModTextures.iconWorld, space.x);
            space.x += iconSpace.width;
            space.width -= iconSpace.width;

            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Tiny;
            String label = DateTime.Now.ToString("HH:mm");

            row.Label(label, space.width - iconSpace.width, null, space.height);
            Text.Anchor = TextAnchor.UpperLeft;
        }
    }
}
