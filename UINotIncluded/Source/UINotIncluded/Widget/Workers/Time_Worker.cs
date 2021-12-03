using RimWorld;
using System;
using System.Text;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget.Workers
{
    public class Time_Worker : WidgetWorker
    {
        readonly Configs.TimeConfig config;

        private const float extra = 50f;
        public override float Width => TimeWidth + DateWidth + extra;
        private float TimeWidth
        {
            get
            {
                if(_timeWidth < 0 || fontCache != Settings.fontSize)
                {
                    GameFont font = Text.Font;
                    Text.Font = Settings.fontSize;
                    _timeWidth = (float)Math.Round(Text.CalcSize("00:00 hs").x);
                    if (fontCache != Settings.fontSize) _dateWidth = -1;
                    fontCache = Settings.fontSize;
                    Text.Font = font;
                }
                return _timeWidth;
            }
        }

        private float DateWidth
        {
            get
            {
                if (Find.CurrentMap == null) return -1;
                if (_dateWidth < 0 || fontCache != Settings.fontSize || _lastFormat != config.dateFormat)
                {
                    GameFont font = Text.Font;
                    Text.Font = Settings.fontSize;
                    string dummyDate = config.dateFormat.GetFormated((long)Find.TickManager.TicksAbs, 0f);
                    _dateWidth = (float)Math.Round(Text.CalcSize(dummyDate).x);
                    if(fontCache != Settings.fontSize) _timeWidth = -1;
                    fontCache = Settings.fontSize;
                    _lastFormat = config.dateFormat;
                    Text.Font = font;
                }
                return _dateWidth;

            }
        }
        private DateFormat _lastFormat;
        private float _dateWidth = -1;
        private float _timeWidth = -1;
        private GameFont fontCache;
        public override void OpenConfigWindow()
        {
            Find.WindowStack.Add(new Windows.EditTimeWidget_Window(config));
        }
        public Time_Worker(Configs.TimeConfig config)
        {
            this.config = config;
        }

        public override bool FixedWidth => true;

        public override void OnGUI(Rect rect)
        {
            this.Margins(ref rect);
            ExtendedToolbar.DoWidgetBackground(rect);
            this.Padding(ref rect);

            Rect space = rect.ContractedBy(ExtendedToolbar.padding);

            Vector2 pos = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
            Season season = GenDate.Season((long)Find.TickManager.TicksAbs, pos);
            Rect iconSpace = DrawIcon(season.GetIconTex(), space.x, space.y, season.LabelCap());
            space.x += iconSpace.width;
            space.width -= iconSpace.width;

            WidgetRow row = new WidgetRow(space.x, space.y, UIDirection.RightThenDown, gap: ExtendedToolbar.interGap);

            float hour = GenDate.HourFloat((long)Find.TickManager.TicksAbs, pos.x);
            int minutes;
            switch (config.roundHour)
            {
                case RoundHour.hour:
                    minutes = 0;
                    break;
                case RoundHour.tenMinute:
                    minutes = (int)Math.Floor((hour - Math.Floor(hour)) * 6) * 10;
                    break;
                case RoundHour.minute:
                    minutes = (int)Math.Floor((hour - Math.Floor(hour)) * 60);
                    break;
                default:
                    throw new NotImplementedException();
                    
            }
            string datestamp = config.dateFormat.GetFormated((long)Find.TickManager.TicksAbs, pos.x);

            float dateWidth = Text.CalcSize(datestamp).x;
            float remainingSpace = space.width - dateWidth - TimeWidth;

            float dateLabelWidth = (float)Math.Floor(dateWidth + remainingSpace / 3);
            float timeLabelWidth = (float)Math.Floor(TimeWidth + remainingSpace * 2 / 3);
            

            Text.WordWrap = false;

            row.Label(datestamp, dateLabelWidth, GetDateDescription(pos, season), space.height);

            string timestamp;
            switch (config.clockFormat)
            {
                case ClockFormat.twelveHours:
                    string meridiam = hour > 12 ? "pm" : "am";
                    hour = hour > 12 ? hour % 12 : hour;
                    hour = (float)Math.Floor(hour);
                    hour = hour == 0 ? 12 : hour;
                    timestamp = string.Format("{0}:{1} {2}",hour.ToString(), minutes.ToString("D2"),meridiam);
                    row.Label(timestamp, timeLabelWidth, height: space.height);
                    break;
                case ClockFormat.twentyfourHours:
                    timestamp = string.Format("{0}:{1} {2}", Math.Floor(hour).ToString(), minutes.ToString("D2"),"hs");
                    row.Label(timestamp, timeLabelWidth, height: space.height);
                    break;
                case ClockFormat.vanilla:
                    DateReadout.DateOnGUI(new Rect(row.FinalX, row.FinalY, timeLabelWidth, space.height));
                    break;
                default:
                    throw new NotImplementedException();
            }
            Text.WordWrap = true;
        }

        private static string GetDateDescription(Vector2 pos, Season season)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 4; ++i)
            {
                Quadrum quadrum = (Quadrum)i;
                stringBuilder.AppendLine(quadrum.Label() + " - " + quadrum.GetSeason(pos.y).LabelCap());
            }
            TaggedString description = "DateReadoutTip".Translate((NamedArgument)GenDate.DaysPassed, (NamedArgument)15, (NamedArgument)season.LabelCap(), (NamedArgument)15, (NamedArgument)GenDate.Quadrum((long)GenTicks.TicksAbs, pos.x).Label(), (NamedArgument)stringBuilder.ToString());
            return description.ToString();
        }
    }
}