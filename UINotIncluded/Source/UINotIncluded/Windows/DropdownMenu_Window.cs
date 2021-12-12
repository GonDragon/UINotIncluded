using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;

namespace UINotIncluded.Windows
{
    public class DropdownMenu_Window : MainTabWindow
    {
        public static Widget.Configs.DropdownMenuConfig config;

        public override Vector2 RequestedTabSize
        {
            get
            {
                float x = config.matchLabelSize ? config.lastWidth : config.width;
                float n_elements = config.elements.Count();

                float y = UIManager.ExtendedBarHeight * n_elements + config.spacing * (n_elements + 1);

                return new Vector2(x, y);
            }
        }

        protected override void SetInitialSizeAndPosition()
        {
            base.SetInitialSizeAndPosition();

            if (config.lastY > UI.screenHeight / 2)
                this.windowRect.y = (float)(UI.screenHeight - UIManager.ExtendedBarHeight) - this.windowRect.height;
            else
                this.windowRect.y = config.lastY + UIManager.ExtendedBarHeight;

            if (config.lastX + this.windowRect.width > UI.screenWidth)
                this.windowRect.x = UI.screenWidth - this.windowRect.width;
            else
                this.windowRect.x = config.lastX;
        }

        protected override float Margin => config.spacing;

        public DropdownMenu_Window()
        {
            this.layer = WindowLayer.Super;
        }

        public override void DoWindowContents(Rect rect)
        {
            GUI.BeginGroup(rect);
            float curY = 0.0f;
            Text.Font = GameFont.Small;
            foreach (Widget.Configs.ButtonConfig config in DropdownMenu_Window.config.elements)
            {
                curY += DrawElement(config, new Vector2(0.0f, curY), rect.width) + DropdownMenu_Window.config.spacing;
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