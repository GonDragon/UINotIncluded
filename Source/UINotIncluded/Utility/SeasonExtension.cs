using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Verse;

namespace RimWorld
{
    public static class SeasonExtension
    {
        public static Texture2D GetIconTex(this Season season)
        {
            switch (season)
            {
                case Season.Undefined:
            return ContentFinder<Texture2D>.Get("GD/UI/Icons/Weather/Unknown");
                case Season.Spring:
            return ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/Spring");
                case Season.Summer:
            return ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/Summer");
                case Season.Fall:
            return ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/Fall");
                case Season.Winter:
            return ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/Winter");
                case Season.PermanentSummer:
            return ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/PermanentSummer");
                case Season.PermanentWinter:
            return ContentFinder<Texture2D>.Get("GD/UI/Icons/Season/PermanentWinter");
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
