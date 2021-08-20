using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using UnityEngine;

namespace UINotIncluded.Utility
{
    public static class TimeSpeedExtension
    {
        public static Texture2D GetTexture(this TimeSpeed timespeed, bool selected)
        {
            switch (timespeed)
            {
                case TimeSpeed.Paused:
                    return ContentFinder<Texture2D>.Get(selected ? "GD/UI/Icons/Others/pause" : "GD/UI/Icons/Others/pause-outline");
                case TimeSpeed.Normal:
                    return ContentFinder<Texture2D>.Get(selected ? "GD/UI/Icons/Others/play" : "GD/UI/Icons/Others/play-outline");
                case TimeSpeed.Fast:
                    return ContentFinder<Texture2D>.Get(selected ? "GD/UI/Icons/Others/fast-forward" : "GD/UI/Icons/Others/fast-forward-outline");
                case TimeSpeed.Superfast:
                    return ContentFinder<Texture2D>.Get(selected ? "GD/UI/Icons/Others/skip-forward" : "GD/UI/Icons/Others/skip-forward-outline");
                case TimeSpeed.Ultrafast:
                    return ContentFinder<Texture2D>.Get(selected ? "GD/UI/Icons/Others/skip-forward" : "GD/UI/Icons/Others/skip-forward-outline");

                default:
                    throw new NotImplementedException();

            }
        }
    }
}
