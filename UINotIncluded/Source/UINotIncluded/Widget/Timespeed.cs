using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;
using UINotIncluded.Utility;

namespace UINotIncluded.Widget
{
    public class Timespeed : ExtendedWidget
    {
        public override float MinimunWidth => 100f;

        public override float MaximunWidth => 150f;

        public override void OnGUI(Rect rect)
        {
            ExtendedToolbar.DoToolbarBackground(rect);
            Rect space = rect.ContractedBy(ExtendedToolbar.padding);
            space.x += 14;
            space.y += 1;
            TimeControls.DoTimeControlsGUI(space);
        }

        private static class CustomTimeControls
        {
            public static readonly Vector2 TimeButSize = TimeControls.TimeButSize;
            private static readonly TimeSpeed[] CachedTimeSpeedValues = (TimeSpeed[])Enum.GetValues(typeof(TimeSpeed));

            private static void PlaySoundOf(TimeSpeed speed)
            {
                SoundDef soundDef = (SoundDef)null;
                switch (speed)
                {
                    case TimeSpeed.Paused:
                        soundDef = SoundDefOf.Clock_Stop;
                        break;
                    case TimeSpeed.Normal:
                        soundDef = SoundDefOf.Clock_Normal;
                        break;
                    case TimeSpeed.Fast:
                        soundDef = SoundDefOf.Clock_Fast;
                        break;
                    case TimeSpeed.Superfast:
                        soundDef = SoundDefOf.Clock_Superfast;
                        break;
                    case TimeSpeed.Ultrafast:
                        soundDef = SoundDefOf.Clock_Superfast;
                        break;
                }
                if (soundDef == null)
                    return;
                soundDef.PlayOneShotOnCamera();
            }

            public static void DoTimeControlsGUI(Rect timerRect)
            {
                TickManager tickManager = Find.TickManager;
                GUI.BeginGroup(timerRect);
                Rect rect = new Rect(0.0f, 0.0f, TimeControls.TimeButSize.x, TimeControls.TimeButSize.y);
                for (int index = 0; index < CustomTimeControls.CachedTimeSpeedValues.Length; ++index)
                {
                    TimeSpeed cachedTimeSpeedValue = CustomTimeControls.CachedTimeSpeedValues[index];
                    if (cachedTimeSpeedValue != TimeSpeed.Ultrafast)
                    {
                        bool selected = tickManager.CurTimeSpeed == cachedTimeSpeedValue;
                        if (Widgets.ButtonImage(rect, cachedTimeSpeedValue.GetTexture(selected)))
                        {
                            if (cachedTimeSpeedValue == TimeSpeed.Paused)
                            {
                                tickManager.TogglePaused();
                                PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Pause, KnowledgeAmount.SpecificInteraction);
                            }
                            else
                            {
                                tickManager.CurTimeSpeed = cachedTimeSpeedValue;
                                PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
                            }
                            CustomTimeControls.PlaySoundOf(tickManager.CurTimeSpeed);
                        }
                        rect.x += rect.width;
                    }
                }
                if (Find.TickManager.slower.ForcedNormalSpeed)
                    Widgets.DrawLineHorizontal(rect.width * 2f, rect.height / 2f, rect.width * 2f);
                GUI.EndGroup();
                GenUI.AbsorbClicksInRect(timerRect);
                UIHighlighter.HighlightOpportunity(timerRect, nameof(TimeControls));
                if (Event.current.type != EventType.KeyDown)
                    return;
                if (KeyBindingDefOf.TogglePause.KeyDownEvent)
                {
                    Find.TickManager.TogglePaused();
                    CustomTimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
                    PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Pause, KnowledgeAmount.SpecificInteraction);
                    Event.current.Use();
                }
                if (!Find.WindowStack.WindowsForcePause)
                {
                    if (KeyBindingDefOf.TimeSpeed_Normal.KeyDownEvent)
                    {
                        Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
                        CustomTimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
                        PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
                        Event.current.Use();
                    }
                    if (KeyBindingDefOf.TimeSpeed_Fast.KeyDownEvent)
                    {
                        Find.TickManager.CurTimeSpeed = TimeSpeed.Fast;
                        CustomTimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
                        PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
                        Event.current.Use();
                    }
                    if (KeyBindingDefOf.TimeSpeed_Superfast.KeyDownEvent)
                    {
                        Find.TickManager.CurTimeSpeed = TimeSpeed.Superfast;
                        CustomTimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
                        PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
                        Event.current.Use();
                    }
                }
                if (!Prefs.DevMode)
                    return;
                if (KeyBindingDefOf.TimeSpeed_Ultrafast.KeyDownEvent)
                {
                    Find.TickManager.CurTimeSpeed = TimeSpeed.Ultrafast;
                    CustomTimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
                    Event.current.Use();
                }
                if (!KeyBindingDefOf.Dev_TickOnce.KeyDownEvent || tickManager.CurTimeSpeed != TimeSpeed.Paused)
                    return;
                tickManager.DoSingleTick();
                SoundDefOf.Clock_Stop.PlayOneShotOnCamera();
            }
        }

    }


}
