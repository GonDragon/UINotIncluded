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

            CustomGizmoGridDrawer.DrawGizmoGrid((IEnumerable<Designator>)testa,2,ref curX,out Gizmo mousoverGizmo);
            if (mousoverGizmo != null) DrawTooltip((Designator)mousoverGizmo, true);
            CustomGizmoGridDrawer.DrawGizmoGrid((IEnumerable<Designator>)testb, 1, ref curX, out mousoverGizmo);
            if (mousoverGizmo != null) DrawTooltip((Designator)mousoverGizmo, false);
            CustomGizmoGridDrawer.DrawGizmoGrid((IEnumerable<Designator>)testc, 2, ref curX, out mousoverGizmo);
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
