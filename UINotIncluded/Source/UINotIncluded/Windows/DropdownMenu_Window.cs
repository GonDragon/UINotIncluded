using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace UINotIncluded.Windows
{
    public class DropdownMenu_Window : MainTabWindow
    {
        public float interSpace = 2f;
        public override Vector2 RequestedTabSize => new Vector2(450f, 390f);

        public override MainTabWindowAnchor Anchor => MainTabWindowAnchor.Right;

        public DropdownMenu_Window()
        {
            this.layer = WindowLayer.Super;
        }


        public override void ExtraOnGUI()
        {
            base.ExtraOnGUI();
            VersionControl.DrawInfoInCorner();
        }

        public override void DoWindowContents(Rect rect)
        {
            GUI.BeginGroup(rect);
            float curY = 0.0f;
            Text.Font = GameFont.Small;
            foreach (Widget.Configs.ButtonConfig config in Widget.WidgetManager.AllAvailableMainTabButtons())
            {
                curY += DrawElement(config, new Vector2(0.0f, curY), rect.width) + interSpace;
            }                

            GUI.EndGroup();
        }

        private float DrawElement(Widget.Configs.ElementConfig config, Vector2 pos, float width)
        {
            Rect rect = new Rect(pos.x, pos.y, width, UIManager.ExtendedBarHeight);
            config.Worker.OnGUI(rect);
            return UIManager.ExtendedBarHeight;
        }
    }
}
