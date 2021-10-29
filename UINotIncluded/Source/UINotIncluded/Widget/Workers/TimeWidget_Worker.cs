using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Verse;
using RimWorld;
using RimWorld.Planet;

namespace UINotIncluded.Widget
{
    public class TimeWidget_Worker : WidgetWorker
    {
        public override float GetWidth()
        {
            return (float)Math.Round(def.minWidth + 35 * (float)Settings.fontSize);
        }

        public override bool WidgetVisible
        {
            get => Find.CurrentMap != null;
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

            WidgetRow row = new WidgetRow(space.x, space.y,UIDirection.RightThenDown, gap: ExtendedToolbar.interGap);

            float hour = GenDate.HourFloat((long)Find.TickManager.TicksAbs, pos.x);
            int minutes = (int)Math.Floor((hour - Math.Floor(hour)) * 6) * 10;
            string timestamp = Math.Floor(hour).ToString() + ":" + minutes.ToString("D2") + " hs";
            string datestamp = Settings.dateFormat.GetFormated((long)Find.TickManager.TicksAbs, pos.x);

            float dateWidth = Text.CalcSize(datestamp).x;
            float timeWidth = Text.CalcSize(timestamp).x;
            float remainingSpace = space.width - dateWidth - timeWidth;

            float dateLabelWidth = (float)Math.Floor(dateWidth + remainingSpace / 3);
            float timeLabelWidth = (float)Math.Floor(timeWidth + remainingSpace * 2 / 3);

            row.Label(datestamp, dateLabelWidth, GetDateDescription(pos, season), space.height);
            row.Label(timestamp, timeLabelWidth, height: space.height);
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
