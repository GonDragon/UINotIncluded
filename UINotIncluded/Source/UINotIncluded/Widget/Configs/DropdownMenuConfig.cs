using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;

namespace UINotIncluded.Widget.Configs
{
    public class DropdownMenuConfig : ButtonConfig
    {
        private Workers.Dropdown_Worker _worker;
        public DropdownMenuConfig() : base(new MainButtonDef()
        {
            defName = "UINI_NMB" + DateTime.Now.ToFileTime(),
            label = "",
            description = "Custom dropdown window.",
            tabWindowClass = typeof(Windows.DropdownMenu_Window)
        })
        {            
            Reset();
        }

        public override string SettingLabel => "Dropdown " + this.Label;

        public override void ExposeData()
        {
            base.ExposeData();
        }

        public override WidgetWorker Worker
        {
            get
            {
                if (_worker == null) _worker = new Workers.Dropdown_Worker(this);
                return _worker;
            }
        }
    }
}
