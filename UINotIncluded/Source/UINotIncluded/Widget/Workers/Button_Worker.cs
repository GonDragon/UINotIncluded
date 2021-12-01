using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace UINotIncluded.Widget.Workers
{
    internal class Button_Worker : WidgetWorker
    {
        public MainButtonDef def;
        private const float CompactModeMargin = 2f;
        private const float IconSize = 32f;
        private Widget.Configs.ButtonConfig config;

        private static readonly Texture2D ButtonBarTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(78, 109, 129, 130).ToColor);

        public Button_Worker(Widget.Configs.ButtonConfig config)
        {
            this.config = config;
            this.def = (MainButtonDef)config.Def;
        }

        public override bool FixedWidth => config.Minimized;

        public override float Width => 60f;

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
            switch (Event.current.type)
            {
                case EventType.Repaint:
                    OnRepaint(rect);
                    break;

                default:
                    OnInteraction(rect);
                    break;
            }
        }

        private void OnRepaint(Rect rect)
        {
            Text.Font = GameFont.Small;
            string str = (string)this.def.LabelCap;
            float num1 = this.def.LabelCapWidth;
            if ((double)num1 > (double)rect.width - 2.0)
            {
                str = this.def.ShortenedLabelCap;
                num1 = this.def.ShortenedLabelCapWidth;
            }
            if (def.Worker.Disabled)
            {
                Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
            }
            else
            {
                bool flag = (double)num1 > 0.850000023841858 * (double)rect.width - 1.0;
                Rect rect1 = rect;
                string label = this.def.Icon == null ? str : "";
                float num2 = flag ? 2f : -1f;
                double buttonBarPercent = def.Worker.ButtonBarPercent;
                double num3 = (double)num2;
                SoundDef mouseoverCategory = SoundDefOf.Mouseover_Category;
                Vector2 functionalSizeOffset = new Vector2();
                Color? labelColor = new Color?();
                DrawButtonTextSubtle(rect1, label, mouseoverCategory, (float)buttonBarPercent, (float)num3, functionalSizeOffset, labelColor);
                if(this.def.Icon != null)
                {
                    Vector2 center = rect.center;
                    float num4 = 16f;
                    if (Mouse.IsOver(rect))
                        center += new Vector2(2f, -2f);
                    GUI.DrawTexture(new Rect(center.x - num4, center.y - num4, IconSize, IconSize), (Texture)this.def.Icon);
                }
                if (Find.MainTabsRoot.OpenTab != this.def && !Find.WindowStack.NonImmediateDialogWindowOpen)
                    UIHighlighter.HighlightOpportunity(rect, this.def.cachedHighlightTagClosed);
                if (this.def.description.NullOrEmpty())
                    return;
                TooltipHandler.TipRegion(rect, (TipSignal)(this.def.LabelCap + "\n\n" + this.def.description));
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

        private static void DrawButtonTextSubtle(
            Rect rect,
            string label,
            SoundDef mouseoverSound,
            float barPercent = 0.0f,
            float textLeftMargin = -1f,
            Vector2 functionalSizeOffset = default(Vector2),
            Color? labelColor = null,
            bool highlight = false)
        {
            Rect offsetRect = rect;
            offsetRect.width += functionalSizeOffset.x;
            offsetRect.height += functionalSizeOffset.y;
            bool flag = false;
            if (Mouse.IsOver(offsetRect))
            {
                flag = true;
                GUI.color = GenUI.MouseoverColor;
            }
            MouseoverSounds.DoRegion(offsetRect, mouseoverSound);
            Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
            if (highlight)
            {
                GUI.color = Color.grey;
                Widgets.DrawBox(rect, 2);
            }
            GUI.color = Color.white;
            if ((double)barPercent > 1.0 / 1000.0)
                Widgets.FillableBar(rect.ContractedBy(1f), barPercent, ButtonBarTex, (Texture2D)null, false);
            Rect rect2 = new Rect(rect);
            if ((double)textLeftMargin < 0.0)
                textLeftMargin = rect.width * 0.15f;
            rect2.x += textLeftMargin;
            if (flag)
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