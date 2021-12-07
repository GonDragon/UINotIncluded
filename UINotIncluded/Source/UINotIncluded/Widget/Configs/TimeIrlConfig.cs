using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UINotIncluded.Widget.Configs
{
    class TimeIrlConfig : ElementConfig
    {
        Workers.TimeIrl_Worker _worker;
        public override string SettingLabel => "IRL-Time Widget";

        public override WidgetWorker Worker
        {
            get
            {
                if (_worker == null) _worker = new Workers.TimeIrl_Worker();
                return _worker;
            }
        }
    }
}
