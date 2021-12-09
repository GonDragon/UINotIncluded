using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace UINotIncluded.Windows
{
    internal class EditTimeWidget_Window : Window
    {
        public Widget.Configs.TimeConfig config;

        private static readonly Vector2 ButSize = new Vector2(150f, 38f);

        public EditTimeWidget_Window(Widget.Configs.TimeConfig config)
        {
            this.config = config;
        }

        public override Vector2 InitialSize => new Vector2(400f, 400f);

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard list = new Listing_Standard();
            list.Begin(inRect);

            if (list.ButtonTextLabeled("UINotIncluded.Setting.dateFormat".Translate(), config.dateFormat.ToStringHuman()))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                foreach (DateFormat dateFormat in Enum.GetValues(typeof(DateFormat)))
                {
                    DateFormat localFormat = dateFormat;
                    options.Add(new FloatMenuOption(localFormat.ToStringHuman(), (Action)(() => config.dateFormat = dateFormat)));
                }
                Find.WindowStack.Add((Window)new FloatMenu(options));
            }

            if (list.ButtonTextLabeled("UINotIncluded.Setting.roundHour".Translate(), config.roundHour.ToString()))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                foreach (RoundHour roundHour in Enum.GetValues(typeof(RoundHour)))
                {
                    options.Add(new FloatMenuOption(roundHour.ToString(), (Action)(() => config.roundHour = roundHour)));
                }
                Find.WindowStack.Add((Window)new FloatMenu(options));
            }

            if (list.ButtonTextLabeled("UINotIncluded.Setting.clockFormat".Translate(), config.clockFormat.ToString()))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                foreach (ClockFormat clockFormat in Enum.GetValues(typeof(ClockFormat)))
                {
                    options.Add(new FloatMenuOption(clockFormat.ToString(), (Action)(() => config.clockFormat = clockFormat)));
                }
                Find.WindowStack.Add((Window)new FloatMenu(options));
            }

            list.End();
            if (Widgets.ButtonText(new Rect((inRect.width / 2f) - (EditTimeWidget_Window.ButSize.x / 2f), inRect.height - EditTimeWidget_Window.ButSize.y, EditTimeWidget_Window.ButSize.x, EditTimeWidget_Window.ButSize.y), (string)"DoneButton".Translate())) this.Close();
        }
    }
}