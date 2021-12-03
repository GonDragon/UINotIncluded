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
        private readonly Widget.Configs.ButtonConfig config;

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
            string label = (string)this.config.Label;
            float labelWidth = this.config.LabelWidth;
            if (labelWidth > rect.width - 2.0)
            {
                label = this.config.ShortenedLabel;
                labelWidth = this.config.ShortenedLabelWidth;
            }
            if (def.Worker.Disabled)
            {
                Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
            }
            else
            {
                bool flag = labelWidth > 0.8 * rect.width;
                Rect labelSpace = new Rect(rect);
                label = config.minimized ? "" : label;
                float leftMarginText = flag ? 2f : rect.width * 0.15f;
                double buttonBarPercent = def.Worker.ButtonBarPercent;
                SoundDef mouseoverCategory = SoundDefOf.Mouseover_Category;
                Color? labelColor = new Color?();

                if(this.config.Icon != null)
                {
                    Vector2 iconPos = rect.center;
                    float halfIconSize = 16f;
                    iconPos -= new Vector2(halfIconSize, halfIconSize);
                    if (label != "") iconPos.x = rect.x + rect.width * 0.15f;
                    float xcorrection = 0f;
                    if (Mouse.IsOver(rect))
                    {
                        iconPos += new Vector2(2f, -2f);
                        xcorrection = -2f;
                    }
                    
                    Rect iconSpace = new Rect(iconPos.x, iconPos.y, IconSize, IconSize);
                    float diff = iconSpace.xMax - labelSpace.x;
                    leftMarginText = diff + halfIconSize + xcorrection;
                    DrawButtonTextSubtle(rect, label, mouseoverCategory, (float)buttonBarPercent, (float)leftMarginText, labelColor);
                    GUI.DrawTexture(iconSpace, (Texture)this.config.Icon);                    
                } else
                {
                    DrawButtonTextSubtle(rect, label, mouseoverCategory, (float)buttonBarPercent, (float)leftMarginText, labelColor);
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
                if (Widgets.ButtonInvisible(rect,false)) InterfaceTryActivate();
            }
        }

        private void DrawButtonTextSubtle(
            Rect rect,
            string label,
            SoundDef mouseoverSound,
            float barPercent = 0.0f,
            float textLeftMargin = -1f,
            Color? labelColor = null)
        {
            bool OverButton = Mouse.IsOver(rect);
            if (OverButton)
            {
                GUI.color = GenUI.MouseoverColor;
                MouseoverSounds.DoRegion(rect, mouseoverSound);
            }
            Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
            GUI.color = Color.white;
            if (barPercent > 1.0 / 1000.0)
                Widgets.FillableBar(rect.ContractedBy(1f), barPercent, ModTextures.ButtonBarTex, (Texture2D)null, false);
            Rect rect2 = new Rect(rect);
            if (textLeftMargin < 0.0)
                textLeftMargin = rect.width * 0.15f;
            rect2.x += textLeftMargin;
            if (OverButton)
            {
                rect2.x += 2f;
                rect2.y -= 2f;
            }
            Verse.Text.Anchor = TextAnchor.MiddleLeft;
            Verse.Text.WordWrap = false;
            Verse.Text.Font = GameFont.Small;
            GUI.color = labelColor ?? Color.white;
            Widgets.Label(rect2, label);
            Verse.Text.Anchor = TextAnchor.UpperLeft;
            Verse.Text.WordWrap = true;
            GUI.color = Color.white;
        }
    }
}