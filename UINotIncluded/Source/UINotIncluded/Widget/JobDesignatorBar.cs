using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    public class JobDesignatorBar
    {
        private static readonly float posX = (float)UI.screenWidth;

        public static List<Designator>[] Jobs => DesignatorManager.GetDesignationConfigs();

        private readonly int mainRows = 1;
        private readonly int leftRows = 2;
        private readonly int rigthRows = 2;

        public void JobDesignatorBarOnGUI()
        {
            if (Find.DesignatorManager.SelectedDesignator != null)
                Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0.0f, (float)((double)(UI.screenHeight - 35) - (double)((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270.0));

            float rigthWidth = CustomGizmoGridDrawer.CalculateWidth(Jobs[(int)DesignationConfig.right], rigthRows);
            float mainWidth = CustomGizmoGridDrawer.CalculateWidth(Jobs[(int)DesignationConfig.main], mainRows);
            float leftWidth = CustomGizmoGridDrawer.CalculateWidth(Jobs[(int)DesignationConfig.left], leftRows);

            float curX = Settings.designationsOnLeft ? 15f + rigthWidth + mainWidth + leftWidth : posX;
            curX -= (rigthWidth + mainWidth);

            CustomGizmoGridDrawer.DrawGizmoGrid((IEnumerable<Designator>)Jobs[(int)DesignationConfig.main], mainRows, curX, out Gizmo mousoverGizmo);
            if (mousoverGizmo != null) DrawTooltip((Designator)mousoverGizmo, false);

            curX -= leftWidth;
            CustomGizmoGridDrawer.DrawGizmoGrid((IEnumerable<Designator>)Jobs[(int)DesignationConfig.left], leftRows, curX, out mousoverGizmo);
            if (mousoverGizmo != null) DrawTooltip((Designator)mousoverGizmo, true);

            curX += mainWidth + leftWidth;
            CustomGizmoGridDrawer.DrawGizmoGrid((IEnumerable<Designator>)Jobs[(int)DesignationConfig.right], rigthRows, curX, out mousoverGizmo, true);
            if (mousoverGizmo != null) DrawTooltip((Designator)mousoverGizmo, true);
            CustomGizmoGridDrawer.Clean();
        }

        public static void DrawTooltip(Designator instance, bool detailed)
        {
            KeyCode k = instance.hotKey == null ? KeyCode.None : instance.hotKey.MainKey;
            String tipText = "";

            if (k != KeyCode.None && CustomGizmoGridDrawer.drawnHotKeys.ContainsKey(k) && CustomGizmoGridDrawer.drawnHotKeys[k] != instance) k = KeyCode.None;

            if (detailed)
            {
                String hotkeyText = String.Format("Hotkey: {0}\n", k.ToString());
                tipText += String.Format("<b>{0}</b>\n{1}\n", instance.Label, hotkeyText);
            }
            tipText += instance.Desc;

            Vector2 mousePos = Event.current.mousePosition;
            Vector2 size = new Vector2(999f, 999f);
            Rect rect = new Rect(mousePos, size);
            TipSignal tip = new TipSignal(tipText, 24637);
            TooltipHandler.TipRegion(rect, tip);
        }
    }
}