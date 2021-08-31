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
    public class TimeWidget : ExtendedWidget
    {
        public override float MinimunWidth => 150f;

        public override float MaximunWidth => 200f;

        public override void OnGUI(Rect rect)
        {
            ExtendedToolbar.DoToolbarBackground(rect);
            
            Rect space = rect.ContractedBy(ExtendedToolbar.padding);

            Vector2 pos = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
            Season season = GenDate.Season((long)Find.TickManager.TicksAbs, pos);
            Rect iconSpace = DrawIcon(season.GetIconTex(), space.x, season.LabelCap());
            space.x += iconSpace.width;
            space.width -= iconSpace.width;

            WidgetRow row = new WidgetRow(space.x, space.y,UIDirection.RightThenDown, space.width,ExtendedToolbar.interGap);
            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Tiny;

            float startX = row.FinalX;


            float hour = GenDate.HourFloat((long)Find.TickManager.TicksAbs, pos.x);
            int minutes = (int)Math.Floor((hour - Math.Floor(hour)) * 6) * 10;
            string timestamp = Math.Floor(hour).ToString() + ":" + minutes.ToString("D2") + " hs";
            string datestamp = UINotIncludedSettings.dateFormat.GetFormated((long)Find.TickManager.TicksAbs, pos.x);

            float dateWidth = Text.CalcSize(datestamp).x;
            float timeWidth = Text.CalcSize(timestamp).x;
            float remainingSpace = space.width - dateWidth - timeWidth - iconSpace.width;

            float dateLabelWidth = (float)Math.Floor(dateWidth + remainingSpace / 2);
            float timeLabelWidth = (float)Math.Floor(timeWidth + remainingSpace / 2);

            UINotIncludedStatic.Warning(String.Format("timestamp: {0}",timestamp));

            row.Label(datestamp, dateLabelWidth, GetDateDescription(pos, season), space.height);
            row.Label(timestamp, timeLabelWidth, height: space.height);
            Text.Anchor = TextAnchor.UpperLeft;
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
