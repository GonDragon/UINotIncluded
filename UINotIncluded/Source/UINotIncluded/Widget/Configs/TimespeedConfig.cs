namespace UINotIncluded.Widget.Configs
{
    public class TimespeedConfig : ElementConfig
    {
        private Workers.Timespeed_Worker _worker;
        public override string SettingLabel => "Time-Speed Widget";

        public override WidgetWorker Worker
        {
            get
            {
                if (_worker == null) _worker = new Workers.Timespeed_Worker();
                return _worker;
            }
        }
    }
}