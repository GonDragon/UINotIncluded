using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace UINotIncluded.Widget.Workers
{
    public class Button_Worker : WidgetWorker
    {
        public MainButtonDef def;
        private const float CompactModeMargin = 2f;
        private const float IconSize = 32f;
        private const float HalfIconSize = 16f;
        public readonly Widget.Configs.ButtonConfig config;

        public Button_Worker(Widget.Configs.ButtonConfig config)
        {
            this.config = config;
            this.def = (MainButtonDef)config.Def;
            config.RefreshCache();
            config.RefreshIcon();
        }

        public override bool FixedWidth => config.minimized;

        public override float Width => 60f;

        public override void OpenConfigWindow()
        {
            Find.WindowStack.Add(new Windows.EditMainButton_Window(config));
        }

        public virtual void InterfaceTryActivate()
        {
            if (TutorSystem.TutorialMode && this.def.canBeTutorDenied && Find.MainTabsRoot.OpenTab != this.def && !TutorSystem.AllowAction((EventPack)("MainTab-" + this.def.defName + "-Open")))
                return;
            if (this.def.closesWorldView && Find.TilePicker.Active && !Find.TilePicker.AllowEscape)
                Messages.Message((string)"MessagePlayerMustSelectTile".Translate(), MessageTypeDefOf.RejectInput, false);
            else
                def.Worker.Activate();
        }

        public override void OnGUI(Rect rect)
        {
#if DEBUG
            string key = "DrawButton " + this.def.defName;
            Analyzer.Profiling.Profiler profiler = Analyzer.Profiling.ProfileController.Start(key);
#endif
            switch (Event.current.type)
            {
                case EventType.Repaint:
                    OnRepaint(rect);
                    break;

                default:
                    OnInteraction(rect);
                    break;
            }
#if DEBUG
            Analyzer.Profiling.ProfileController.Stop(key);
#endif
        }

        private void OnRepaint(Rect rect)
        {
            Text.Font = GameFont.Small;
            string label = config.hideLabel ? "" : (string)this.config.Label;
            float labelWidth = this.config.LabelWidth;
            bool overButton = Mouse.IsOver(rect);
            float leftMargin = rect.width * 0.1f;
            bool drawIcon = this.config.Icon != null;
            float maxWidth = drawIcon ? rect.width - leftMargin - IconSize - 8f : rect.width - leftMargin - 4f;

            if (labelWidth > maxWidth)
            {
                label = this.config.ShortenedLabel;
                labelWidth = this.config.ShortenedLabelWidth;
                if (labelWidth > maxWidth) label = "";
            }

            if (def.Worker.Disabled)
            {
                Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
            }
            else
            {
                label = config.minimized ? "" : label;
                double buttonBarPercent = def.Worker.ButtonBarPercent;

                if (drawIcon)
                {
                    Vector2 iconPos = rect.center;
                    iconPos -= new Vector2(HalfIconSize, HalfIconSize);
                    if (label != "") iconPos.x = rect.x + leftMargin;
                    if (overButton)
                    {
                        iconPos += new Vector2(2f, -2f);
                    }

                    Rect iconSpace = new Rect(iconPos.x, iconPos.y, IconSize, IconSize);
                    leftMargin += IconSize + 4f;
                    DrawButtonTextSubtle(rect, label, SoundDefOf.Mouseover_Category, overButton, (float)buttonBarPercent, (float)leftMargin);
                    GUI.DrawTexture(iconSpace, (Texture)this.config.Icon);
                }
                else
                {
                    DrawButtonTextSubtle(rect, label, SoundDefOf.Mouseover_Category, overButton, (float)buttonBarPercent, (float)leftMargin);
                }

                if (Find.MainTabsRoot.OpenTab != this.def && !Find.WindowStack.NonImmediateDialogWindowOpen)
                    UIHighlighter.HighlightOpportunity(rect, this.def.cachedHighlightTagClosed);
                if (this.def.description.NullOrEmpty())
                    return;
                TooltipHandler.TipRegion(rect, (TipSignal)(this.config.Label + "\n\n" + this.def.description));
            }
        }

        private void OnInteraction(Rect rect)
        {
            if (!Mouse.IsOver(rect)) return;
            if (def.Worker.Disabled)
            {
                if (Event.current.type == EventType.MouseDown)
                    return;
                Event.current.Use();
            }
            else
            {
                if (Widgets.ButtonInvisible(rect, false)) InterfaceTryActivate();
            }
        }

        private void DrawButtonTextSubtle(
            Rect rect,
            string label,
            SoundDef mouseoverSound,
            bool OverButton = false,
            float barPercent = 0.0f,
            float textLeftMargin = 0f)
        {
            if (OverButton) GUI.color = GenUI.MouseoverColor;
            MouseoverSounds.DoRegion(rect, mouseoverSound);
            Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
            GUI.color = Color.white;
            if (barPercent > 1.0 / 1000.0)
                Widgets.FillableBar(rect.ContractedBy(1f), barPercent, ModTextures.ButtonBarTex, (Texture2D)null, false);
            Rect labelSpace = new Rect(rect);
            labelSpace.x += textLeftMargin;
            if (OverButton)
            {
                labelSpace.x += 2f;
                labelSpace.y -= 2f;
            }
            Verse.Text.Anchor = TextAnchor.MiddleLeft;
            Verse.Text.WordWrap = false;
            Verse.Text.Font = GameFont.Small;
            Widgets.Label(labelSpace, label);
            Verse.Text.Anchor = TextAnchor.UpperLeft;
            Verse.Text.WordWrap = true;
        }
    }
}