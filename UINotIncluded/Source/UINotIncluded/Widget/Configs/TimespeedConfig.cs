using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UINotIncluded.Widget.Configs
{
    public class TimespeedConfig : ElementConfig
    {
        Workers.Timespeed_Worker _worker;
        public override string SettingLabel => "Time-Speed Widget";

        public override WidgetWorker Worker
        {
            get
            {
                if (_worker == null) _worker = new Workers.Timespeed_Worker();
                return _worker;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is TimespeedConfig config;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
