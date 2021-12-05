using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UINotIncluded.Windows
{
    public class EditDropdown_Window : EditMainButton_Window
    {
        public EditDropdown_Window(Widget.Configs.DropdownMenuConfig config) : base(config)
        {

        }

        public override Vector2 InitialSize => new Vector2(700f, 600f);

        public override void DoWindowContents(Rect rect)
        {
            Rect firstHalf = new Rect(rect.x, rect.y, Mathf.Round(rect.width / 2f), rect.height);
            Rect secondHalf = new Rect(firstHalf.xMax, rect.y, rect.width - firstHalf.width, rect.height);

            base.DoWindowContents(secondHalf);
        }
    }
}
