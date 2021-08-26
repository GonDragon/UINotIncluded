using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace UINotIncluded.Widget
{
    static class ArchitectMenuButton
    {

        public static void ArchitectButtonOnGUI(float posX, float posY, float size)
        {
            Rect rect = new Rect(posX, posY, size, size);
            Rect inRect = rect.ContractedBy(1f);

            GUI.DrawTexture(rect, SolidColorMaterials.NewSolidColorTexture(new ColorInt(111, 111, 111, (int)byte.MaxValue).ToColor));
            Widgets.DrawAtlas(inRect, ContentFinder<Texture2D>.Get("GD/UI/ClockBG"));
            if (Widgets.ButtonImage(inRect, ContentFinder<Texture2D>.Get("GD/UI/Icons/Others/ruler-square-compass"))) Find.MainTabsRoot.ToggleTab(DefDatabase<MainButtonDef>.GetNamed("Architect"));
        }
    }
}
