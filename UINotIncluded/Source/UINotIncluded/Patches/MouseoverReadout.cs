using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using HarmonyLib;
using UnityEngine;
using RimWorld;
using System.Reflection.Emit;
using System.Reflection;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(MouseoverReadout))]
    class MouseoverReadoutPatches
    {
        [HarmonyPatch(nameof(MouseoverReadout.MouseoverReadoutOnGUI))]
        static bool Prefix(out bool __state)
        {
            if (!Settings.vanillaReadout)
            {
                __state = false;
                return false;
            }
            __state = true;

            float deltaH = Settings.tabsOnTop ? 35 : 0;

            GUI.BeginGroup(new Rect(0f,0f + deltaH, UI.screenWidth, UI.screenHeight - deltaH));
            return true;
        }

        [HarmonyPatch(nameof(MouseoverReadout.MouseoverReadoutOnGUI))]
        static void Postfix(bool __state)
        {
            if(__state)
            {
                GUI.EndGroup();
            }
        }
    }

    public static class MouseoverReadoutHelper
    {
        private static string indoorsUnroofedStringCached;
        private static int indoorsUnroofedStringCachedRoofCount = -1;
        private static string cachedTemperatureString;
        private static string cachedTemperatureStringForLabel;
        private static float cachedTemperatureStringForTemperature;
        private static TemperatureDisplayMode cachedTemperatureDisplayMode;

        public static string TemperatureString()
        {
            IntVec3 intVec3_1 = UI.MouseCell();
            IntVec3 c = intVec3_1;
            Room room1 = intVec3_1.GetRoom(Find.CurrentMap);
            if (room1 == null)
            {
                for (int index = 0; index < 9; ++index)
                {
                    IntVec3 intVec3_2 = intVec3_1 + GenAdj.AdjacentCellsAndInside[index];
                    if (intVec3_2.InBounds(Find.CurrentMap))
                    {
                        Room room2 = intVec3_2.GetRoom(Find.CurrentMap);
                        if (room2 != null && (!room2.PsychologicallyOutdoors && !room2.UsesOutdoorTemperature || !room2.PsychologicallyOutdoors && (room1 == null || room1.PsychologicallyOutdoors) || room2.PsychologicallyOutdoors && room1 == null))
                        {
                            c = intVec3_2;
                            room1 = room2;
                        }
                    }
                }
            }
            if (room1 == null && intVec3_1.InBounds(Find.CurrentMap))
            {
                Building edifice = intVec3_1.GetEdifice(Find.CurrentMap);
                if (edifice != null)
                {
                    CellRect cellRect = edifice.OccupiedRect();
                    cellRect = cellRect.ExpandedBy(1);
                    foreach (IntVec3 clipInside in cellRect.ClipInsideMap(Find.CurrentMap))
                    {
                        room1 = clipInside.GetRoom(Find.CurrentMap);
                        if (room1 != null && !room1.PsychologicallyOutdoors)
                        {
                            c = clipInside;
                            break;
                        }
                    }
                }
            }
            string str;
            if (c.InBounds(Find.CurrentMap) && !c.Fogged(Find.CurrentMap) && room1 != null && !room1.PsychologicallyOutdoors)
            {
                if (room1.OpenRoofCount == 0)
                {
                    str = (string)"Indoors".Translate();
                }
                else
                {
                    if (MouseoverReadoutHelper.indoorsUnroofedStringCachedRoofCount != room1.OpenRoofCount)
                    {
                        MouseoverReadoutHelper.indoorsUnroofedStringCached = (string)("IndoorsUnroofed".Translate() + " (" + room1.OpenRoofCount.ToStringCached() + ")");
                        MouseoverReadoutHelper.indoorsUnroofedStringCachedRoofCount = room1.OpenRoofCount;
                    }
                    str = MouseoverReadoutHelper.indoorsUnroofedStringCached;
                }
            }
            else
                str = (string)"Outdoors".Translate();
            float num1 = room1 == null || c.Fogged(Find.CurrentMap) ? Find.CurrentMap.mapTemperature.OutdoorTemp : room1.Temperature;
            int num2 = Mathf.RoundToInt(GenTemperature.CelsiusTo(MouseoverReadoutHelper.cachedTemperatureStringForTemperature, Prefs.TemperatureMode));
            int num3 = Mathf.RoundToInt(GenTemperature.CelsiusTo(num1, Prefs.TemperatureMode));
            if (MouseoverReadoutHelper.cachedTemperatureStringForLabel != str || num2 != num3 || MouseoverReadoutHelper.cachedTemperatureDisplayMode != Prefs.TemperatureMode)
            {
                MouseoverReadoutHelper.cachedTemperatureStringForLabel = str;
                MouseoverReadoutHelper.cachedTemperatureStringForTemperature = num1;
                MouseoverReadoutHelper.cachedTemperatureString = str + " " + num1.ToStringTemperature("F0");
                MouseoverReadoutHelper.cachedTemperatureDisplayMode = Prefs.TemperatureMode;
            }
            return MouseoverReadoutHelper.cachedTemperatureString;
        }
    }
}
