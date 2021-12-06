using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;

namespace UINotIncluded.Widget.Configs
{
    public class DropdownMenuConfig : ButtonConfig
    {
        public List<Widget.Configs.ElementConfig> elements = new List<ElementConfig>();
        public float width = 200f;
        public float spacing = 5f;

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
            Scribe_Values.Look(ref width, "width", 200f);
            Scribe_Values.Look(ref spacing, "spacing", 5f);
            Scribe_Collections.Look(ref elements, "elements", LookMode.Deep);
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
