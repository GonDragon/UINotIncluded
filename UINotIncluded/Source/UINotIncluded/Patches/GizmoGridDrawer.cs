using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using UnityEngine;
using Verse;

namespace UINotIncluded.Patches
{
    [HarmonyPatch(typeof(GizmoGridDrawer), "DrawGizmoGrid")]
    public class GizmoGridDrawerPatch
    {

        public static bool Prefix(
            IEnumerable<Gizmo> gizmos,
            float startX,
            out Gizmo mouseoverGizmo,
            Func<Gizmo, bool> customActivatorFunc,
            Func<Gizmo, bool> highlightFunc,
            Func<Gizmo, bool> lowlightFunc,
            float ___heightDrawn,
            int ___heightDrawnFrame,
            List<List<Gizmo>> ___gizmoGroups,
            List<Gizmo> ___firstGizmos,
            List<Command> ___shrinkableCommands,
            List<Gizmo> ___tmpAllGizmos,
            Func<Gizmo, Gizmo, int> ___SortByOrder
            )
        {
            if (Event.current.type == EventType.Layout)
            {
                mouseoverGizmo = (Gizmo)null;
            }
            else
            {
                float baseY = Settings.TabsOnBottom ? UI.screenHeight - 33 : UI.screenHeight; // ONLY reason for this patch
                ___tmpAllGizmos.Clear();
                ___tmpAllGizmos.AddRange(gizmos);
                ___tmpAllGizmos.SortStable<Gizmo>(___SortByOrder);
                ___gizmoGroups.Clear();
                for (int index1 = 0; index1 < ___tmpAllGizmos.Count; ++index1)
                {
                    Gizmo tmpAllGizmo = ___tmpAllGizmos[index1];
                    bool flag = false;
                    for (int index2 = 0; index2 < ___gizmoGroups.Count; ++index2)
                    {
                        if (___gizmoGroups[index2][0].GroupsWith(tmpAllGizmo))
                        {
                            flag = true;
                            ___gizmoGroups[index2].Add(tmpAllGizmo);
                            ___gizmoGroups[index2][0].MergeWith(tmpAllGizmo);
                            break;
                        }
                    }
                    if (!flag)
                    {
                        List<Gizmo> gizmoList = SimplePool<List<Gizmo>>.Get();
                        gizmoList.Add(tmpAllGizmo);
                        ___gizmoGroups.Add(gizmoList);
                    }
                }
                ___firstGizmos.Clear();
                ___shrinkableCommands.Clear();
                float num1 = (float)(UI.screenWidth - 147);
                Vector2 vector2 = new Vector2(startX, (float)((double)baseY - (double)GizmoGridDrawer.GizmoSpacing.y - 75.0));
                float maxWidth = num1 - startX;
                int num2 = 0;
                for (int index3 = 0; index3 < ___gizmoGroups.Count; ++index3)
                {
                    List<Gizmo> gizmoGroup = ___gizmoGroups[index3];
                    Gizmo gizmo = (Gizmo)null;
                    for (int index4 = 0; index4 < gizmoGroup.Count; ++index4)
                    {
                        if (!gizmoGroup[index4].disabled)
                        {
                            gizmo = gizmoGroup[index4];
                            break;
                        }
                    }
                    if (gizmo == null)
                        gizmo = gizmoGroup.FirstOrDefault<Gizmo>();
                    else if (gizmo is Command_Toggle commandToggle23)
                    {
                        if (!commandToggle23.activateIfAmbiguous && !commandToggle23.isActive())
                        {
                            for (int index5 = 0; index5 < gizmoGroup.Count; ++index5)
                            {
                                if (gizmoGroup[index5] is Command_Toggle commandToggle24 && !commandToggle24.disabled && commandToggle24.isActive())
                                {
                                    gizmo = gizmoGroup[index5];
                                    break;
                                }
                            }
                        }
                        if (commandToggle23.activateIfAmbiguous && commandToggle23.isActive())
                        {
                            for (int index6 = 0; index6 < gizmoGroup.Count; ++index6)
                            {
                                if (gizmoGroup[index6] is Command_Toggle commandToggle25 && !commandToggle25.disabled && !commandToggle25.isActive())
                                {
                                    gizmo = gizmoGroup[index6];
                                    break;
                                }
                            }
                        }
                    }
                    if (gizmo != null)
                    {
                        if (gizmo is Command command6 && command6.shrinkable && command6.Visible)
                            ___shrinkableCommands.Add(command6);
                        if ((double)vector2.x + (double)gizmo.GetWidth(maxWidth) > (double)num1)
                        {
                            vector2.x = startX;
                            vector2.y -= 75f + GizmoGridDrawer.GizmoSpacing.x;
                            ++num2;
                        }
                        vector2.x += gizmo.GetWidth(maxWidth) + GizmoGridDrawer.GizmoSpacing.x;
                        ___firstGizmos.Add(gizmo);
                    }
                }
                if (num2 > 1 && ___shrinkableCommands.Count > 1)
                {
                    for (int index = 0; index < ___shrinkableCommands.Count; ++index)
                        ___firstGizmos.Remove((Gizmo)___shrinkableCommands[index]);
                }
                else
                    ___shrinkableCommands.Clear();
                GizmoGridDrawer.drawnHotKeys.Clear();
                GizmoGridDrawer.customActivator = customActivatorFunc;
                Text.Font = GameFont.Tiny;
                Vector2 topLeft1 = new Vector2(startX, (float)((double)baseY - (double)GizmoGridDrawer.GizmoSpacing.y - 75.0));
                mouseoverGizmo = (Gizmo)null;
                Gizmo interactedGiz = (Gizmo)null;
                Event interactedEvent = (Event)null;
                Gizmo floatMenuGiz = (Gizmo)null;
                for (int index = 0; index < ___firstGizmos.Count; ++index)
                {
                    Gizmo firstGizmo = ___firstGizmos[index];
                    if (firstGizmo.Visible)
                    {
                        if ((double)topLeft1.x + (double)firstGizmo.GetWidth(maxWidth) > (double)num1)
                        {
                            topLeft1.x = startX;
                            topLeft1.y -= 75f + GizmoGridDrawer.GizmoSpacing.x;
                        }
                        ___heightDrawnFrame = Time.frameCount;
                        ___heightDrawn = (float)UI.screenHeight - topLeft1.y;
                        GizmoResult result = firstGizmo.GizmoOnGUI(topLeft1, maxWidth, new GizmoRenderParms()
                        {
                            highLight = highlightFunc != null && highlightFunc(firstGizmo),
                            lowLight = lowlightFunc != null && lowlightFunc(firstGizmo)
                        });
                        ProcessGizmoState(firstGizmo, result, ref mouseoverGizmo);
                        GenUI.AbsorbClicksInRect(new Rect(topLeft1.x, topLeft1.y, firstGizmo.GetWidth(maxWidth), 75f + GizmoGridDrawer.GizmoSpacing.y).ContractedBy(-12f));
                        topLeft1.x += firstGizmo.GetWidth(maxWidth) + GizmoGridDrawer.GizmoSpacing.x;
                    }
                }
                float num3 = topLeft1.x;
                int num4 = 0;
                for (int index = 0; index < ___shrinkableCommands.Count; ++index)
                {
                    Command shrinkableCommand = ___shrinkableCommands[index];
                    float getShrunkSize = shrinkableCommand.GetShrunkSize;
                    if ((double)topLeft1.x + (double)getShrunkSize > (double)num1)
                    {
                        ++num4;
                        if (num4 > 1)
                            num3 = startX;
                        topLeft1.x = num3;
                        topLeft1.y -= getShrunkSize + 3f;
                    }
                    Vector2 topLeft2 = topLeft1;
                    topLeft2.y += getShrunkSize + 3f;
                    ___heightDrawnFrame = Time.frameCount;
                    ___heightDrawn = Mathf.Min(___heightDrawn, (float)UI.screenHeight - topLeft2.y);
                    GizmoResult result = shrinkableCommand.GizmoOnGUIShrunk(topLeft2, getShrunkSize, new GizmoRenderParms()
                    {
                        highLight = highlightFunc != null && highlightFunc((Gizmo)shrinkableCommand),
                        lowLight = lowlightFunc != null && lowlightFunc((Gizmo)shrinkableCommand)
                    });
                    ProcessGizmoState((Gizmo)shrinkableCommand, result, ref mouseoverGizmo);
                    GenUI.AbsorbClicksInRect(new Rect(topLeft2.x, topLeft2.y, getShrunkSize, getShrunkSize + 3f).ExpandedBy(3f));
                    topLeft1.x += getShrunkSize + 3f;
                }
                if (interactedGiz != null)
                {
                    List<Gizmo> matchingGroup = FindMatchingGroup(interactedGiz);
                    for (int index = 0; index < matchingGroup.Count; ++index)
                    {
                        Gizmo other = matchingGroup[index];
                        if (other != interactedGiz && !other.disabled && interactedGiz.InheritInteractionsFrom(other))
                            other.ProcessInput(interactedEvent);
                    }
                    interactedGiz.ProcessInput(interactedEvent);
                    Event.current.Use();
                }
                else if (floatMenuGiz != null)
                {
                    List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
                    foreach (FloatMenuOption clickFloatMenuOption in floatMenuGiz.RightClickFloatMenuOptions)
                        floatMenuOptionList.Add(clickFloatMenuOption);
                    List<Gizmo> matchingGroup = FindMatchingGroup(floatMenuGiz);
                    for (int index7 = 0; index7 < matchingGroup.Count; ++index7)
                    {
                        Gizmo other = matchingGroup[index7];
                        if (other != floatMenuGiz && !other.disabled && floatMenuGiz.InheritFloatMenuInteractionsFrom(other))
                        {
                            foreach (FloatMenuOption clickFloatMenuOption in other.RightClickFloatMenuOptions)
                            {
                                FloatMenuOption floatMenuOption = (FloatMenuOption)null;
                                for (int index8 = 0; index8 < floatMenuOptionList.Count; ++index8)
                                {
                                    if (floatMenuOptionList[index8].Label == clickFloatMenuOption.Label)
                                    {
                                        floatMenuOption = floatMenuOptionList[index8];
                                        break;
                                    }
                                }
                                if (floatMenuOption == null)
                                    floatMenuOptionList.Add(clickFloatMenuOption);
                                else if (!clickFloatMenuOption.Disabled)
                                {
                                    if (!floatMenuOption.Disabled)
                                    {
                                        Action prevAction = floatMenuOption.action;
                                        Action localOptionAction = clickFloatMenuOption.action;
                                        floatMenuOption.action = (Action)(() =>
                                        {
                                            prevAction();
                                            localOptionAction();
                                        });
                                    }
                                    else if (floatMenuOption.Disabled)
                                        floatMenuOptionList[floatMenuOptionList.IndexOf(floatMenuOption)] = clickFloatMenuOption;
                                }
                            }
                        }
                    }
                    Event.current.Use();
                    if (floatMenuOptionList.Any<FloatMenuOption>())
                        Find.WindowStack.Add((Window)new FloatMenu(floatMenuOptionList));
                }
                for (int index = 0; index < ___gizmoGroups.Count; ++index)
                {
                    ___gizmoGroups[index].Clear();
                    SimplePool<List<Gizmo>>.Return(___gizmoGroups[index]);
                }
                ___gizmoGroups.Clear();
                ___firstGizmos.Clear();
                ___tmpAllGizmos.Clear();

                void ProcessGizmoState(Gizmo giz, GizmoResult result, ref Gizmo mouseoverGiz)
                {
                    if (result.State == GizmoState.Interacted || result.State == GizmoState.OpenedFloatMenu && giz.RightClickFloatMenuOptions.FirstOrDefault<FloatMenuOption>() == null)
                    {
                        interactedEvent = result.InteractEvent;
                        interactedGiz = giz;
                    }
                    else if (result.State == GizmoState.OpenedFloatMenu)
                        floatMenuGiz = giz;
                    if (result.State < GizmoState.Mouseover)
                        return;
                    mouseoverGiz = giz;
                }
            }

            List<Gizmo> FindMatchingGroup(Gizmo toMatch)
            {
                for (int index = 0; index < ___gizmoGroups.Count; ++index)
                {
                    if (___gizmoGroups[index].Contains(toMatch))
                        return ___gizmoGroups[index];
                }
                return (List<Gizmo>)null;
            }

            return false;
        }
    }
}
