using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace UINotIncluded.Widget.Configs
{
    public class TimeConfig : ElementConfig
    {
        public DateFormat dateFormat = DateFormat.ddmmmYYYY;
        public RoundHour roundHour = RoundHour.tenMinute;
        public ClockFormat clockFormat = ClockFormat.twentyfourHours;
        Workers.Time_Worker _worker;
        public override string SettingLabel => "Time Widget";
        public override bool Configurable => true;
        public override void ExposeData()
        {
            Scribe_Values.Look(ref dateFormat, "dateFormat", DateFormat.ddmmmYYYY);
            Scribe_Values.Look(ref roundHour, "roundHour", RoundHour.tenMinute);
            Scribe_Values.Look(ref clockFormat, "clockFormat", ClockFormat.twentyfourHours);
        }

        public override bool Equals(object obj)
        {
            return obj is TimeConfig config;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override WidgetWorker Worker
        {
            get
            {
                if (_worker == null) _worker = new Workers.Time_Worker(this);
                return _worker;
            }
        }
    }
}
