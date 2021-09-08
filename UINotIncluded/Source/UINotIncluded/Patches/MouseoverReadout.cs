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
        [HarmonyPrefix]
        [HarmonyPatch(nameof(MouseoverReadout.MouseoverReadoutOnGUI))]
        static bool MouseoverReadoutOnGUIPrefix(ref TerrainDef ___cachedTerrain, ref string ___cachedTerrainString, string[] ___glowStrings)
        {
            if (Event.current.type != EventType.Repaint || (!MouseReadoutWidget.AltInspector && Find.MainTabsRoot.OpenTab != null)) return false;

            string readout = "";

            IntVec3 c = UI.MouseCell();
            if (!c.InBounds(Find.CurrentMap))
                return false;
            if (c.Fogged(Find.CurrentMap))
            {
                MouseReadoutWidget.DrawReadout("Undiscovered".Translate());
            }
            else
            {
                readout = "\n" + ___glowStrings[Mathf.RoundToInt(Find.CurrentMap.glowGrid.GameGlowAt(c) * 100f)] + readout;
                TerrainDef terrain = c.GetTerrain(Find.CurrentMap);
                if (terrain != ___cachedTerrain)
                {
                    string str = (double)terrain.fertility > 0.0001 ? " " + "FertShort".TranslateSimple() + " " + terrain.fertility.ToStringPercent() : "";
                    ___cachedTerrainString = (string)(terrain.LabelCap + (terrain.passability != Traversability.Impassable ? " (" + "WalkSpeed".Translate((NamedArgument)MouseReadoutWidget.SpeedPercentString((float)terrain.pathCost )) + str + ")" : (TaggedString)(string)null));
                    ___cachedTerrain = terrain;
                }
                string cachedTerrainString = ___cachedTerrainString;
                readout = "\n" + cachedTerrainString + readout;
                Zone zone = c.GetZone(Find.CurrentMap);
                if (zone != null)
                {
                    readout = "\n" + zone.label + readout;
                }
                float depth = Find.CurrentMap.snowGrid.GetDepth(c);
                if ((double)depth > 0.0299999993294477)
                {
                    SnowCategory snowCategory = SnowUtility.GetSnowCategory(depth);
                    string snowLabel = (string)(SnowUtility.GetDescription(snowCategory) + " (" + "WalkSpeed".Translate((NamedArgument)MouseReadoutWidget.SpeedPercentString((float)SnowUtility.MovementTicksAddOn(snowCategory))) + ")");
                    readout = "\n" + snowLabel + readout;
                }                
                /* START Temperature String Inject*/
                readout = "\n" + MouseoverReadoutHelper.TemperatureString() + readout;
                /* END */
                List<Thing> thingList = c.GetThingList(Find.CurrentMap);
                for (int index = 0; index < thingList.Count; ++index)
                {
                    Thing thing = thingList[index];
                    if (thing.def.category != ThingCategory.Mote)
                    {
                        readout = "\n" + thing.LabelMouseover + readout;
                    }
                }
                RoofDef roof = c.GetRoof(Find.CurrentMap);
                if (roof != null)
                {
                    readout = "\n" + roof.LabelCap + readout;
                }
                MouseReadoutWidget.DrawReadout(readout);
            }
            return false;
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
