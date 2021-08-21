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
            if (Event.current.type != EventType.Repaint || Find.MainTabsRoot.OpenTab != null) return false;

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
                GlobalControls GlobConInstance = Find.MapUI.globalControls;
                MethodInfo TemperatureString = AccessTools.Method(typeof(GlobalControls),"TemperatureString");
                readout = "\n" + TemperatureString.Invoke(GlobConInstance,null) + readout;
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
}
