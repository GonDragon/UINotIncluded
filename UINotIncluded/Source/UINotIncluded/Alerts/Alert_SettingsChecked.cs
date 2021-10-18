using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;

namespace UINotIncluded.Alerts
{
    class Alert_SettingsChecked : Alert
    {
        public Alert_SettingsChecked()
        {
            this.defaultLabel = (string)"UINotIncluded.Alerts.SettingsChecked.Label".Translate();
            this.defaultExplanation = (string)"UINotIncluded.Alerts.SettingsChecked.Description".Translate();
            this.defaultPriority = AlertPriority.High;
        }
        public override AlertReport GetReport()
        {
            return (AlertReport)(!Settings.settingsChecked);
        }
    }
}
