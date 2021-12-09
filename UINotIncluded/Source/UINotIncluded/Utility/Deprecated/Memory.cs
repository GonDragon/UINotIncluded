using System;
using UINotIncluded.Widget.Configs;
using Verse;

namespace UINotIncluded
{
    public class Memory : IExposable
    {
        public virtual void ExposeData()
        { }

        public virtual Widget.Configs.ElementConfig ConvertToConfig() => throw new NotImplementedException();
    }

    public class MainButtonMemory : Memory
    {
        public string iconPath;
        public string label;
        public bool minimized;
        public bool visible;
        private string defName;

        public override ElementConfig ConvertToConfig()
        {
            ButtonConfig config = new ButtonConfig(defName)
            {
                Label = label,
                minimized = minimized,
                IconPath = iconPath
            };
            config.RefreshIcon();
            config.RefreshCache();
            return config;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref label, "label");
            Scribe_Values.Look(ref iconPath, "iconPath");
            Scribe_Values.Look(ref minimized, "minimized");
            Scribe_Values.Look(ref defName, "button");
            Scribe_Values.Look(ref visible, "visible", true);
        }
    }

    public class TimeWidgetMemory : Memory
    {
        public DateFormat dateFormat;
        public RoundHour roundHour;
        public ClockFormat clockFormat;

        public override ElementConfig ConvertToConfig()
        {
            TimeConfig config = new TimeConfig
            {
                dateFormat = dateFormat,
                roundHour = roundHour,
                clockFormat = clockFormat
            };
            return config;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref dateFormat, "dateFormat", DateFormat.MMDDYYYY);
            Scribe_Values.Look(ref roundHour, "roundHour", RoundHour.tenMinute);
            Scribe_Values.Look(ref clockFormat, "clockFormat", ClockFormat.twentyfourHours);
        }
    }

    public class BlankSpaceMemory : Memory
    {
        public bool fixedWidth;
        public float width;

        public override ElementConfig ConvertToConfig()
        {
            BlankSpaceConfig config = new BlankSpaceConfig
            {
                fixedWidth = fixedWidth,
                width = width
            };
            return config;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref fixedWidth, "fixed");
            Scribe_Values.Look(ref width, "width");
        }
    }
}