using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UINotIncluded.Widget.Workers;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget.Configs
{
    public class WeatherConfig : ElementConfig
    {
        Workers.Weather_Worker _worker;
        public override string SettingLabel => "Weather Widget";

        public override WidgetWorker Worker
        {
            get
            {
                if (_worker == null) _worker = new Workers.Weather_Worker(this);
                return _worker;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is WeatherConfig config;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
