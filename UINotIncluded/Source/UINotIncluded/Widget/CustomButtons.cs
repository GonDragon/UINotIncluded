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
        private static readonly float arrowWidth = 20f;
        public static ButtonArrowAction ButtonLabelWithArrows(Rect space, String label)
        {
            float labelWidth = space.width - (arrowWidth * 3);
            float halfbuttonHeight = (float)Math.Floor(space.height / 2);
            Widgets.ButtonText(new Rect(space.x + arrowWidth, space.y, labelWidth, space.height), label);

            ButtonArrowAction action = ButtonArrowAction.none;

            action = Widgets.ButtonImageWithBG(new Rect(space.x, space.y, arrowWidth, space.height), ModTextures.chevronLeft) ? ButtonArrowAction.left : action;
            action = Widgets.ButtonImageWithBG(new Rect(space.x + labelWidth + 2*arrowWidth, space.y, arrowWidth, space.height), ModTextures.chevronRight) ? ButtonArrowAction.right : action;

            action = Widgets.ButtonImageWithBG(new Rect(space.x + labelWidth + arrowWidth, space.y, arrowWidth, halfbuttonHeight), ModTextures.chevronUp) ? ButtonArrowAction.up : action;
            action = Widgets.ButtonImageWithBG(new Rect(space.x + labelWidth + arrowWidth, space.y + halfbuttonHeight, arrowWidth, space.height - halfbuttonHeight), ModTextures.chevronDown) ? ButtonArrowAction.down : action;

            return action;
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
