using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Verse;

namespace UINotIncluded
{
    public static class CustomButtons
    {
        public static Widgets.DraggableResult DraggableButton(Rect space, String label)
        {

            Texture2D texture = Mouse.IsOver(space) ? ModTextures.buttonDraggableMouseover : ModTextures.buttonDraggable;

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.DrawAtlas(space, texture);
            Widgets.Label(space, label);
            Text.Anchor = TextAnchor.UpperLeft;

            return Widgets.ButtonInvisibleDraggable(space); ;
        }
    }

    public enum ButtonArrowAction
    {
        none,
        up,
        left,
        right,
        down
    }
}
