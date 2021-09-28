using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;
using System.Reflection;

namespace UINotIncluded
{
    public class AltInspectorManager
    {
        private static readonly int uniqueID = 606502519; //Random generated unique ID.

        private readonly Dictionary<string, string> tileInfo = new Dictionary<string, string>();
        private readonly List<string> things = new List<string>();

        private readonly Vector2 cleanSize = new Vector2(10f, 10f);
        private readonly float padding = 5f;

        private TerrainDef cachedTerrain;
        private string cachedTerrainLabel;
        private string cachedTerrainWalkspeedString;
        private int cachedTerrainWalkspeedInt;
        private string cachedTerrainFertility;
        private string[] glowStrings;

        private static string indoorsUnroofedStringCached;
        private static int indoorsUnroofedStringCachedRoofCount = -1;
        private static string cachedTemperatureString;
        private static string cachedTemperatureStringForLabel;
        private static float cachedTemperatureStringForTemperature;
        private static TemperatureDisplayMode cachedTemperatureDisplayMode;

        public AltInspectorManager() => MakePermaCache();
        private void MakePermaCache()
        {
            this.glowStrings = new string[101];
            for (int index = 0; index <= 100; ++index)
                this.glowStrings[index] = GlowGrid.PsychGlowAtGlow((float)index / 100f).GetLabel() + " (" + ((float)index / 100f).ToStringPercent() + ")";
        }

        private bool ShowDrawReadout()
        {
            if (Event.current.type != EventType.Repaint) return false;
            return (UIManager.toggleAltInspector || Event.current.alt);
        }

        public void AltInspectorOnGUI()
        {
            if (ShowDrawReadout())
            {
                GenerateTileInfoAndThings();
                if (tileInfo.Count() == 0) return;

                if (Settings.legacyAltInspector)
                {
                    TipSignal tooltip = new TipSignal(TileInfoAsString(), uniqueID) { delay = 0f };
                    TooltipHandler.TipRegion(new Rect(Event.current.mousePosition, cleanSize), tooltip);
                    return;
                }
                Rect windowRect = GetWindowRect();
                Find.WindowStack.ImmediateWindow(AltInspectorManager.uniqueID, windowRect, WindowLayer.Super, () => {
                    DrawTileInfo(windowRect);
                });
                TooltipHandler.ClearTooltipsFrom(new Rect(Event.current.mousePosition, cleanSize));
            }
        }

        private void GenerateTileInfoAndThings()
        {
            tileInfo.Clear();
            things.Clear();
            IntVec3 currentCell = UI.MouseCell();
            if (!currentCell.InBounds(Find.CurrentMap)) return;
            if (currentCell.Fogged(Find.CurrentMap))
            {
                tileInfo["Terrain"] = "Undiscovered".Translate();
            } else
            {
                TerrainDef terrain = currentCell.GetTerrain(Find.CurrentMap);
                if (terrain != cachedTerrain)
                {
                    cachedTerrainLabel = terrain.LabelCap;
                    cachedTerrainWalkspeedInt = terrain.pathCost;
                    cachedTerrainWalkspeedString = terrain.passability != Traversability.Impassable ? SpeedPercentString((float)terrain.pathCost) : (string)"Impassable".Translate();
                    cachedTerrainFertility = terrain.fertility.ToStringPercent();
                    cachedTerrain = terrain;
                }
                tileInfo["Terrain"] = cachedTerrainLabel;
                tileInfo["UINotIncluded.AltInspector.WalkSpeed"] = cachedTerrainWalkspeedString;
                float snowDepth = Find.CurrentMap.snowGrid.GetDepth(currentCell);
                if ((double)snowDepth > 0.0299999993294477)
                {
                    SnowCategory snowCategory = SnowUtility.GetSnowCategory(snowDepth);
                    int snowMovementTicks = SnowUtility.MovementTicksAddOn(snowCategory);
                    if(snowMovementTicks > cachedTerrainWalkspeedInt) tileInfo["UINotIncluded.AltInspector.WalkSpeed"] = this.SpeedPercentString((float)snowMovementTicks);
                    tileInfo["UINotIncluded.AltInspector.TopLayer"] = SnowUtility.GetDescription(snowCategory);
                }

                tileInfo["Temperature"] = TemperatureString(currentCell);

                tileInfo["UINotIncluded.AltInspector.Fertility"] = cachedTerrainFertility;

                tileInfo["UINotIncluded.AltInspector.Brightness"] = glowStrings[Mathf.RoundToInt(Find.CurrentMap.glowGrid.GameGlowAt(currentCell) * 100f)];
                

                Zone zone = currentCell.GetZone(Find.CurrentMap);
                if (zone != null) tileInfo["Zone"] = zone.label;

                RoofDef roof = currentCell.GetRoof(Find.CurrentMap);
                if (roof != null)
                {
                    tileInfo["Roof"] = roof.LabelCap;
                }

                List<Thing> thingList = currentCell.GetThingList(Find.CurrentMap);
                for (int index = 0; index < thingList.Count; ++index)
                {
                    Thing thing = thingList[index];
                    if (thing.def.category != ThingCategory.Mote)
                    {
                        things.Add(thing.LabelMouseover);
                    }
                }
            }
        }

        private Rect GetWindowRect()
        {
            int nRows = tileInfo.Count() + things.Count();
            Rect rect = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 250f, 25f * nRows + padding*2);

            rect.x += 26f;
            rect.y += 26f;
            if ((double)rect.xMax > (double)UI.screenWidth)
                rect.x -= rect.width + 52f;
            if ((double)rect.yMax > (double)UI.screenHeight)
                rect.y -= rect.height + 52f;
            return rect;
        }

        public void DrawTileInfo(Rect windowRect)
        {
            float curY = padding;
            Rect inRect = new Rect(padding,curY,windowRect.width - padding*2, 100f);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;
            Text.WordWrap = false;

            int n = 0;
            foreach(string infoLabel in tileInfo.Keys)
            {
                Rect row = new Rect(inRect.x, curY, inRect.width, 23f);
                if (n % 2 == 1)
                    Widgets.DrawLightHighlight(row);

                Rect labelSize = new Rect(row.x, curY, 100f, 23f);
                Widgets.Label(labelSize, infoLabel.Translate());

                Rect valueSize = new Rect(labelSize.xMax, curY, 150f, 23f);
                Widgets.Label(valueSize, tileInfo[infoLabel]);
                curY += 25f;
                n++;
            }
            foreach(string thingLabel in things)
            {
                Rect row = new Rect(inRect.x, curY, inRect.width, 23f);
                if (n % 2 == 1)
                    Widgets.DrawLightHighlight(row);
                Widgets.Label(row, thingLabel);
                curY += 25f;
                n++;
            }
            Text.WordWrap = true;
        }

        public string TileInfoAsString()
        {
            string singleLabel = "";
            singleLabel += tileInfo["Terrain"];

            if (tileInfo.Count() == 1) return singleLabel;

            if(tileInfo.ContainsKey("UINotIncluded.AltInspector.WalkSpeed") || tileInfo.ContainsKey("UINotIncluded.AltInspector.Fertility"))
            {
                singleLabel += " (";
                if (tileInfo.ContainsKey("UINotIncluded.AltInspector.WalkSpeed")) singleLabel += String.Format("{0}, ", "WalkSpeed".Translate((NamedArgument)tileInfo["UINotIncluded.AltInspector.WalkSpeed"]));
                if (tileInfo.ContainsKey("UINotIncluded.AltInspector.Fertility")) singleLabel += "FertShort".TranslateSimple() + tileInfo["UINotIncluded.AltInspector.Fertility"];
                singleLabel += ")";
            }

            singleLabel += String.Format("\n{0}\n{1}\n", tileInfo["Temperature"],tileInfo["UINotIncluded.AltInspector.Brightness"]);

            if (tileInfo.ContainsKey("UINotIncluded.AltInspector.TopLayer")) singleLabel += String.Format("{0}\n",tileInfo["UINotIncluded.AltInspector.TopLayer"]);
            if (tileInfo.ContainsKey("Zone")) singleLabel += String.Format("{0}\n", tileInfo["Zone"]);
            if (tileInfo.ContainsKey("Roof")) singleLabel += String.Format("{0}\n", tileInfo["Roof"]);

            foreach (string thingLabel in things)
            {
                singleLabel += String.Format("{0}\n", thingLabel);
            }

            return singleLabel.Trim();
        }

        private static string TemperatureString(IntVec3 cell)
        {
            Room room1 = cell.GetRoom(Find.CurrentMap);
            if (room1 == null)
            {
                for (int index = 0; index < 9; ++index)
                {
                    IntVec3 intVec3_2 = cell + GenAdj.AdjacentCellsAndInside[index];
                    if (intVec3_2.InBounds(Find.CurrentMap))
                    {
                        Room room2 = intVec3_2.GetRoom(Find.CurrentMap);
                        if (room2 != null && (!room2.PsychologicallyOutdoors && !room2.UsesOutdoorTemperature || !room2.PsychologicallyOutdoors && (room1 == null || room1.PsychologicallyOutdoors) || room2.PsychologicallyOutdoors && room1 == null))
                        {
                            cell = intVec3_2;
                            room1 = room2;
                        }
                    }
                }
            }
            if (room1 == null && cell.InBounds(Find.CurrentMap))
            {
                Building edifice = cell.GetEdifice(Find.CurrentMap);
                if (edifice != null)
                {
                    CellRect cellRect = edifice.OccupiedRect();
                    cellRect = cellRect.ExpandedBy(1);
                    foreach (IntVec3 clipInside in cellRect.ClipInsideMap(Find.CurrentMap))
                    {
                        room1 = clipInside.GetRoom(Find.CurrentMap);
                        if (room1 != null && !room1.PsychologicallyOutdoors)
                        {
                            cell = clipInside;
                            break;
                        }
                    }
                }
            }
            string str;
            if (cell.InBounds(Find.CurrentMap) && !cell.Fogged(Find.CurrentMap) && room1 != null && !room1.PsychologicallyOutdoors)
            {
                if (room1.OpenRoofCount == 0)
                {
                    str = (string)"Indoors".Translate();
                }
                else
                {
                    if (AltInspectorManager.indoorsUnroofedStringCachedRoofCount != room1.OpenRoofCount)
                    {
                        AltInspectorManager.indoorsUnroofedStringCached = (string)("IndoorsUnroofed".Translate() + " (" + room1.OpenRoofCount.ToStringCached() + ")");
                        AltInspectorManager.indoorsUnroofedStringCachedRoofCount = room1.OpenRoofCount;
                    }
                    str = AltInspectorManager.indoorsUnroofedStringCached;
                }
            }
            else
                str = (string)"Outdoors".Translate();
            float num1 = room1 == null || cell.Fogged(Find.CurrentMap) ? Find.CurrentMap.mapTemperature.OutdoorTemp : room1.Temperature;
            int num2 = Mathf.RoundToInt(GenTemperature.CelsiusTo(AltInspectorManager.cachedTemperatureStringForTemperature, Prefs.TemperatureMode));
            int num3 = Mathf.RoundToInt(GenTemperature.CelsiusTo(num1, Prefs.TemperatureMode));
            if (AltInspectorManager.cachedTemperatureStringForLabel != str || num2 != num3 || AltInspectorManager.cachedTemperatureDisplayMode != Prefs.TemperatureMode)
            {
                AltInspectorManager.cachedTemperatureStringForLabel = str;
                AltInspectorManager.cachedTemperatureStringForTemperature = num1;
                AltInspectorManager.cachedTemperatureString = str + " " + num1.ToStringTemperature("F0");
                AltInspectorManager.cachedTemperatureDisplayMode = Prefs.TemperatureMode;
            }
            return AltInspectorManager.cachedTemperatureString;
        }

        public string SpeedPercentString(float extraPathTicks) => (13f / (extraPathTicks + 13f)).ToStringPercent();
    }
}
