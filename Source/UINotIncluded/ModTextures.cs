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
        public static Texture2D toolbarBackground = ContentFinder<Texture2D>.Get("GD/UI/ClockBG");
        public static Texture2D toolbarWidgetBackground = ContentFinder<Texture2D>.Get("GD/UI/ClockSCR");

        public static Texture2D arquitectMenuIcon = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/ruler-square-compass");

        public static Texture2D chevronLeft = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/chevron-left");
        public static Texture2D chevronRight = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/chevron-right");
        public static Texture2D chevronUp = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/chevron-up");
        public static Texture2D chevronDown = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/chevron-down");

        public static Texture2D weatherUnknown = ContentFinder<Texture2D>.Get("GD/UI/Icons/Weather/Unknown");
        public static Texture2D seasonSpring = ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/Spring");
        public static Texture2D seasonSummer = ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/Summer");
        public static Texture2D seasonFall = ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/Fall");
        public static Texture2D seasonWinter = ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/Winter");
        public static Texture2D seasonPermaSummer = ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/PermanentSummer");
        public static Texture2D seasonPermaWinter = ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/PermanentWinter");

        public static Texture2D iconAltInspector = ContentFinder<Texture2D>.Get("GD/UI/Icons/Playsettings/alt-info");
        public static Texture2D iconWorld = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/world");

        public static Texture2D pause = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/pause");
        public static Texture2D pause_outl = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/pause-outline");
        public static Texture2D play = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/play");
        public static Texture2D play_outl = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/play-outline");
        public static Texture2D speedOne = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/fast-forward");
        public static Texture2D speedOne_outl = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/fast-forward-outline");
        public static Texture2D speedTwo = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/skip-forward");
        public static Texture2D speedTwo_outl = ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/skip-forward-outline");

        private static readonly Dictionary<String, Texture2D> weathers = new Dictionary<String, Texture2D>();
        public static Texture2D WeatherIcon(string iconPath)
        {
            if (weathers.ContainsKey(iconPath)) return weathers[iconPath];

            weathers.Add(iconPath, ContentFinder<Texture2D>.Get(iconPath));
            return weathers[iconPath];
        }
    }
}
