namespace UINotIncluded.Widget.Configs
{
    internal class TimeIrlConfig : ElementConfig
    {
        private Workers.TimeIrl_Worker _worker;
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