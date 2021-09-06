using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Verse;

namespace UINotIncluded
{
    [StaticConstructorOnStartup]
    public static class ModTextures
    {
        public static readonly Texture2D toolbarBackground = ContentFinder<Texture2D>.Get("GD/UI/ClockBG");
        public static readonly Texture2D toolbarWidgetBackground = ContentFinder<Texture2D>.Get("GD/UI/ClockSCR");

        public static readonly Texture2D arquitectMenuIcon = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/ruler-square-compass");

        public static readonly Texture2D chevronLeft = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/chevron-left");
        public static readonly Texture2D chevronRight = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/chevron-right");
        public static readonly Texture2D chevronUp = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/chevron-up");
        public static readonly Texture2D chevronDown = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/chevron-down");

        public static readonly Texture2D weatherUnknown = ContentFinder<Texture2D>.Get("GD/UI/Icons/Weather/Unknown");
        public static readonly Texture2D seasonSpring = ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/Spring");
        public static readonly Texture2D seasonSummer = ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/Summer");
        public static readonly Texture2D seasonFall = ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/Fall");
        public static readonly Texture2D seasonWinter = ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/Winter");
        public static readonly Texture2D seasonPermaSummer = ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/PermanentSummer");
        public static readonly Texture2D seasonPermaWinter = ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/PermanentWinter");

        public static readonly Texture2D iconAltInspector = ContentFinder<Texture2D>.Get("GD/UI/Icons/Playsettings/alt-info");
        public static readonly Texture2D iconWorld = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/world");

        public static readonly Texture2D pause = ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Pause");
        public static readonly Texture2D pause_outl = ContentFinder<Texture2D>.Get("UI/TimeControls/pause-outline");
        public static readonly Texture2D play = ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Normal");
        public static readonly Texture2D play_outl = ContentFinder<Texture2D>.Get("UI/TimeControls/play-outline");
        public static readonly Texture2D speedOne = ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Fast");
        public static readonly Texture2D speedOne_outl = ContentFinder<Texture2D>.Get("UI/TimeControls/fast-forward-outline");
        public static readonly Texture2D speedTwo = ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Superfast");
        public static readonly Texture2D speedTwo_outl = ContentFinder<Texture2D>.Get("UI/TimeControls/skip-forward-outline");

        private static readonly Dictionary<String, Texture2D> weathers = new Dictionary<String, Texture2D>();
        public static Texture2D WeatherIcon(string iconPath)
        {
            if (weathers.ContainsKey(iconPath)) return weathers[iconPath];

            weathers.Add(iconPath, ContentFinder<Texture2D>.Get(iconPath));
            return weathers[iconPath];
        }
    }
}
