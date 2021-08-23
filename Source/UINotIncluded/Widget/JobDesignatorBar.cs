using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;

namespace UINotIncluded.Widget
{
    public class JobDesignatorBar
    {
        private static readonly float posX = (float)UI.screenWidth - 15f;

        public static List<Designator> Jobs;
        public JobDesignatorBar()
        {
            Jobs = DefDatabase<DesignationCategoryDef>.GetNamed("Orders").AllResolvedDesignators;
        }
        public void JobDesignatorBarOnGUI()
        {
            float curX = posX;

            if (Find.DesignatorManager.SelectedDesignator != null)
                Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0.0f, (float)((double)(UI.screenHeight - 35) - (double)((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270.0));

            List<Designator> testa = new List<Designator>();
            List<Designator> testb = new List<Designator>();
            List<Designator> testc = new List<Designator>();

            for(int i = 0;i < Jobs.Count(); i++)
            {
                testa.Add(Jobs[i]);
                if(i < 5) testb.Add(Jobs[i]);
                if (i < 4) testc.Add(Jobs[i]);
            }

            JobsDesignationGridDrawer.DrawJobsGrid((IEnumerable<Designator>)testa, ref curX, (float)UI.screenHeight - 60, 3,true);
            JobsDesignationGridDrawer.DrawJobsGrid((IEnumerable<Designator>)testb, ref curX, (float)UI.screenHeight - 60, 1, true);
            JobsDesignationGridDrawer.DrawJobsGrid((IEnumerable<Designator>)testc, ref curX, (float)UI.screenHeight - 60, 2, false);
            JobsDesignationGridDrawer.DrawTooltip();
        }
    }

    public static class JobsDesignationGridDrawer
    {
        [TweakValue("A. Grid SpacingX", 0, 20)]
        public static int spacingX = 5;

        [TweakValue("A. Grid SpacingY", 0, 20)]
        public static int spacingY = 5;

        [TweakValue("A. Grid Height", 10, 200)]
        public static int gridHeight = 80;

        private static readonly List<Designator> tempJobs = new List<Designator>();

        private static Event interactedEvent;
        private static Designator interactedJob;
        private static Designator floatMenuGiz;
        private static Designator mouseOverGiz;
        private static bool drawTooltip = false;
        private static bool detailed = false;
        public static void DrawJobsGrid(
          IEnumerable<Designator> gizmos,
          ref float leftX,
          float botomY,
          int rows = 1,
          bool leftoversOnRigth = false)
        {
            if (Event.current.type == EventType.Layout)
            {
                return;
            }
            else
            {
                tempJobs.Clear();
                tempJobs.AddRange(gizmos);

                float buttonSize = (gridHeight / (float)rows) - (spacingY * ((float)rows - 1f) / rows);
                float startY = botomY - buttonSize * rows - (spacingY * (rows - 1));
                int[] startYrows = new int[rows];
                bool simpleButtons = buttonSize < 50;

                for (int i = 0; i < rows; i++)
                {
                    startYrows[i] = (int)Math.Floor(startY + i * (buttonSize + spacingY));
                }

                int curRow = 1;
                int curJob = 0;
                if (leftoversOnRigth)
                {
                    int leftovers = gizmos.Count() % rows;
                    if(leftovers > 0)
                    {
                        for (int remaining = leftovers; remaining > 0; remaining--)
                        {
                            Rect buttonSpace = new Rect(leftX - buttonSize, startYrows[curRow - 1], buttonSize, buttonSize);
                            GizmoResult result = tempJobs[remaining].DoCustomGuizmoOnGUI(buttonSpace, new GizmoRenderParms(), simpleButtons);
                            if (result.State == GizmoState.Mouseover)
                            {
                                ProcessDesignatorState(tempJobs[remaining], result, simpleButtons);
                            };
                            curRow++;
                            curJob++;
                        }
                        curRow = 1;
                        leftX -= buttonSize + spacingX;
                    }                    
                }

                for(int i = curJob;i < tempJobs.Count;i++)
                {
                    Rect buttonSpace = new Rect(leftX-buttonSize,startYrows[curRow-1],buttonSize,buttonSize);
                    GizmoResult result = tempJobs[i].DoCustomGuizmoOnGUI(buttonSpace, new GizmoRenderParms(),simpleButtons);
                    if (result.State == GizmoState.Mouseover)
                    {
                        ProcessDesignatorState(tempJobs[i], result,simpleButtons);
                    };
                    curRow++;
                    if (curRow > rows)
                    {
                        curRow = 1;
                        leftX -= buttonSize + spacingX;
                    }
                }
            }
            if (interactedJob != null)
            {
                interactedJob.ProcessInput(interactedEvent);
                interactedJob = (Designator)null;
                Event.current.Use();
            }
            else if (floatMenuGiz != null)
            {
                List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
                foreach (FloatMenuOption clickFloatMenuOption in floatMenuGiz.RightClickFloatMenuOptions)
                    floatMenuOptionList.Add(clickFloatMenuOption);
                Event.current.Use();
                if (floatMenuOptionList.Any<FloatMenuOption>()) Find.WindowStack.Add((Window)new FloatMenu(floatMenuOptionList));
                floatMenuGiz = (Designator)null;
            }


            void ProcessDesignatorState(Designator job, GizmoResult result, bool simpleButtons)
            {
                if (result.State == GizmoState.Interacted || result.State == GizmoState.OpenedFloatMenu && job.RightClickFloatMenuOptions.FirstOrDefault<FloatMenuOption>() == null)
                {
                    interactedEvent = result.InteractEvent;
                    interactedJob = job;
                }
                else if (result.State == GizmoState.OpenedFloatMenu)
                    floatMenuGiz = job;
                if (result.State < GizmoState.Mouseover)
                    return;
                mouseOverGiz = job;
                drawTooltip = true;
                detailed = simpleButtons;
            }
        }

        public static void DrawTooltip()
        {
            if(drawTooltip) { drawTooltip = false; mouseOverGiz.DrawTooltip(detailed); }
        }
    }
}
