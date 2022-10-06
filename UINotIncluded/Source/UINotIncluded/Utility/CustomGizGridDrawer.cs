using System;
using System.Collections.Generic;
using System.Linq;
using UINotIncluded;
using UnityEngine;

namespace Verse
{
    public static class CustomGizmoGridDrawer
    {
        public static Dictionary<KeyCode, Gizmo> drawnHotKeys = new Dictionary<KeyCode, Gizmo>();
        public static Func<Gizmo, bool> customActivator;
        private static float heightDrawn;
        private static int heightDrawnFrame;
        public static readonly Vector2 GizmoSpacing = new Vector2(5f, 5f);
        private static readonly float defaultSize = 80f;
        private static readonly List<List<Gizmo>> gizmoGroups = new List<List<Gizmo>>();
        private static readonly List<Gizmo> firstGizmos = new List<Gizmo>();
        private static readonly List<Gizmo> tmpAllGizmos = new List<Gizmo>();
        private static readonly Func<Gizmo, Gizmo, int> SortByOrder = (Func<Gizmo, Gizmo, int>)((lhs, rhs) => lhs.Order.CompareTo(rhs.Order));

        public static float HeightDrawnRecently => Time.frameCount > CustomGizmoGridDrawer.heightDrawnFrame + 2 ? 0.0f : CustomGizmoGridDrawer.heightDrawn;

        public static void DrawGizmoGrid(
          IEnumerable<Gizmo> gizmos,
          int rows,
          float startX,
          out Gizmo mouseoverGizmo,
          bool leftoversOnRight = false,
          Func<Gizmo, bool> customActivatorFunc = null,
          Func<Gizmo, bool> highlightFunc = null,
          Func<Gizmo, bool> lowlightFunc = null)
        {
            if (Event.current.type == EventType.Layout)
            {
                mouseoverGizmo = (Gizmo)null;
            }
            else
            {
                CustomGizmoGridDrawer.tmpAllGizmos.Clear();
                CustomGizmoGridDrawer.tmpAllGizmos.AddRange(gizmos);
                CustomGizmoGridDrawer.tmpAllGizmos.SortStable<Gizmo>(CustomGizmoGridDrawer.SortByOrder);
                CustomGizmoGridDrawer.gizmoGroups.Clear();

                float buttonSize = (defaultSize / (float)rows) - (GizmoSpacing.y * ((float)rows - 1f) / rows);
                int rowLength = (int)Math.Ceiling((float)gizmos.Count() / rows);
                bool fullRows = gizmos.Count() % rows == 0;

                float curX = startX;
                float startY = (UI.screenHeight - 15f) - buttonSize * rows - (GizmoSpacing.y * (rows - 1));

                int[] startYrows = new int[rows];
                bool simpleButtons = buttonSize < 50;

                if (Settings.TabsOnBottom) startY -= UIManager.ExtendedBarHeight;

                for (int i = 0; i < rows; i++)
                {
                    startYrows[i] = (int)Math.Floor(startY + i * (buttonSize + GizmoSpacing.y));
                }

                for (int index1 = 0; index1 < CustomGizmoGridDrawer.tmpAllGizmos.Count; ++index1)
                {
                    Gizmo tmpAllGizmo = CustomGizmoGridDrawer.tmpAllGizmos[index1];
                    bool flag = false;
                    for (int index2 = 0; index2 < CustomGizmoGridDrawer.gizmoGroups.Count; ++index2)
                    {
                        if (CustomGizmoGridDrawer.gizmoGroups[index2][0].GroupsWith(tmpAllGizmo))
                        {
                            flag = true;
                            CustomGizmoGridDrawer.gizmoGroups[index2].Add(tmpAllGizmo);
                            CustomGizmoGridDrawer.gizmoGroups[index2][0].MergeWith(tmpAllGizmo);
                            break;
                        }
                    }
                    if (!flag)
                    {
                        List<Gizmo> gizmoList = SimplePool<List<Gizmo>>.Get();
                        gizmoList.Add(tmpAllGizmo);
                        CustomGizmoGridDrawer.gizmoGroups.Add(gizmoList);
                    }
                }
                CustomGizmoGridDrawer.firstGizmos.Clear();

                for (int index3 = 0; index3 < CustomGizmoGridDrawer.gizmoGroups.Count; ++index3)
                {
                    List<Gizmo> gizmoGroup = CustomGizmoGridDrawer.gizmoGroups[index3];
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
                    if (gizmo != null) CustomGizmoGridDrawer.firstGizmos.Add(gizmo);
                }
                CustomGizmoGridDrawer.customActivator = customActivatorFunc;
                Text.Font = GameFont.Tiny;

                mouseoverGizmo = (Gizmo)null;
                Gizmo interactedGiz = (Gizmo)null;
                Event interactedEvent = (Event)null;
                Gizmo floatMenuGiz = (Gizmo)null;

                int curRow = 1;
                for (int index = 0; index < CustomGizmoGridDrawer.firstGizmos.Count; ++index)
                {
                    Gizmo firstGizmo = CustomGizmoGridDrawer.firstGizmos[index];
                    if (firstGizmo.Visible)
                    {
                        Rect buttonSpace = new Rect(curX, startYrows[curRow - 1], buttonSize, buttonSize);

                        CustomGizmoGridDrawer.heightDrawnFrame = Time.frameCount;
                        CustomGizmoGridDrawer.heightDrawn = (float)UI.screenHeight - startYrows[curRow - 1];
                        GizmoResult result = ((Designator)firstGizmo).DoCustomGuizmoOnGUI(buttonSpace, new GizmoRenderParms()
                        {
                            highLight = highlightFunc != null && highlightFunc(firstGizmo),
                            lowLight = lowlightFunc != null && lowlightFunc(firstGizmo)
                        },
                        simpleButtons);
                        ProcessGizmoState(firstGizmo, result, ref mouseoverGizmo);
                        GenUI.AbsorbClicksInRect(buttonSpace);

                        if ((index + 1) % rowLength == 0)
                        {
                            curX = startX;
                            curRow++;
                            if (curRow == rows && !leftoversOnRight && !fullRows) { curX += buttonSize + GizmoSpacing.x; } // TODO: Dirty fix. Only works with 2 rows. Make it work on n rows.
                        }
                        else
                        {
                            curX += buttonSize + GizmoSpacing.x;
                        }
                    }
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
                for (int index = 0; index < CustomGizmoGridDrawer.gizmoGroups.Count; ++index)
                {
                    CustomGizmoGridDrawer.gizmoGroups[index].Clear();
                    SimplePool<List<Gizmo>>.Return(CustomGizmoGridDrawer.gizmoGroups[index]);
                }
                CustomGizmoGridDrawer.gizmoGroups.Clear();
                CustomGizmoGridDrawer.firstGizmos.Clear();
                CustomGizmoGridDrawer.tmpAllGizmos.Clear();

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
                for (int index = 0; index < CustomGizmoGridDrawer.gizmoGroups.Count; ++index)
                {
                    if (CustomGizmoGridDrawer.gizmoGroups[index].Contains(toMatch))
                        return CustomGizmoGridDrawer.gizmoGroups[index];
                }
                return (List<Gizmo>)null;
            }
        }

        public static void Clean()
        {
            GizmoGridDrawer.drawnHotKeys.Clear();
            drawnHotKeys.Clear();
        }

        public static float CalculateWidth(IEnumerable<Gizmo> gizmos, int rows)
        {
            float amountGizmos = gizmos.Count();
            float buttonSize = (defaultSize / (float)rows) - (GizmoSpacing.y * ((float)rows - 1f) / rows);
            float rowLength = (float)Math.Ceiling(amountGizmos / rows);

            return (rowLength * buttonSize) + GizmoSpacing.y * (rowLength);
        }
    }
}