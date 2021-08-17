using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using RimWorld;
using UnityEngine;
using HarmonyLib;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(WeatherManager), "DoWeatherGUI")]
    class WeatherManagerOnGUIPatch
    {
        static void Prefix(ref Rect rect)
        {
            rect = new Rect(UIManager.Instance.nextPosition, 0,230f,UIManager.Instance.ClockHeight);
        }

        static void Postfix()
        {

        }
    }
}
