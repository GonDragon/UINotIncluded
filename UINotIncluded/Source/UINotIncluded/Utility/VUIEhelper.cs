using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VUIE;

namespace UINotIncluded.Utility
{
    public class VUIEhelper
    {
        public static float buttonWidth = 160f;
        public readonly object dragDropManager = new DragDropManager<Widget.Configs.ElementConfig>((config, topLeft) => config.Worker.OnGUI(new Rect(topLeft, new Vector2(config.Worker.FixedWidth ? config.Worker.Width : buttonWidth, UIManager.ExtendedBarHeight))));

        public int mouseoverIdx;
        public bool lastEditMode = false;
    }
}
