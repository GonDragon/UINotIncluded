using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UINotIncluded.Widget.Configs;

using RimWorld;
using Verse;
using UnityEngine;

namespace UINotIncluded.Widget.Workers
{
    public class Dropdown_Worker : Button_Worker
    {
        public override bool ShowTooltip => false;
        public Dropdown_Worker(ButtonConfig config) : base(config) {        }

        public override void InterfaceTryActivate()
        {
            Windows.DropdownMenu_Window.config = (DropdownMenuConfig)config;
            base.InterfaceTryActivate();
        }

        public override void OnGUI(Rect rect)
        {
            ((DropdownMenuConfig)config).lastX = rect.x;
            ((DropdownMenuConfig)config).lastY = rect.y;
            base.OnGUI(rect);
        }

        public override void OpenConfigWindow()
        {
            Find.WindowStack.Add(new UINotIncluded.Windows.EditDropdown_Window((Configs.DropdownMenuConfig)this.config));
        }

    }
}
