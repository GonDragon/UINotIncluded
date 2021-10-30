using RimWorld;
using Verse;

namespace UINotIncluded.Alerts
{
    internal class Alert_SettingsChecked : Alert
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