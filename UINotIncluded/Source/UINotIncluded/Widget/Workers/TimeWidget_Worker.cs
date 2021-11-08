using RimWorld;
using System;
using System.Text;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    public class TimeWidget_Worker : WidgetWorker
    {
        private float TimeWidth
        {
            get
            {
                if(Settings.fontSize != lastFont)
                {
                    _timeWidth = Text.CalcSize("00:00 hs").x;
                    lastFont = Settings.fontSize;
                }
                return _timeWidth;
            }
        }
        private float _timeWidth;
        private GameFont lastFont;

        public override BarElementMemory CreateMemory => new TimeWidgetMemory();

        public override float GetWidth(BarElementMemory memory)
        {
            float difference;
            switch(Settings.fontSize)
            {
                case GameFont.Small:
                    difference = 35;
                    break;
                case GameFont.Medium:
                    difference = 75;
                    break;
                default:
                    difference = 0;
                    break;
            }
            return (float)Math.Round(def.minWidth + difference);
        }

        public override bool WidgetVisible
        {
            get => Find.CurrentMap != null;
        }

        public override Action ConfigAction(BarElementMemory memory)
        {
            return () => Find.WindowStack.Add(new UINotIncluded.Windows.EditTimeWidget_Window((TimeWidgetMemory) memory));
        }

        public override void OnGUI(Rect rect, BarElementMemory memory)
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
            switch (((TimeWidgetMemory)memory).roundHour)
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
            string datestamp = ((TimeWidgetMemory)memory).dateFormat.GetFormated((long)Find.TickManager.TicksAbs, pos.x);

            float dateWidth = Text.CalcSize(datestamp).x;
            float remainingSpace = space.width - dateWidth - TimeWidth;

            float dateLabelWidth = (float)Math.Floor(dateWidth + remainingSpace / 3);
            float timeLabelWidth = (float)Math.Floor(TimeWidth + remainingSpace * 2 / 3);
            

            Text.WordWrap = false;

            row.Label(datestamp, dateLabelWidth, GetDateDescription(pos, season), space.height);

            string timestamp;
            switch (((TimeWidgetMemory)memory).clockFormat)
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