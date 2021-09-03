﻿using System;
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
            Widgets.DrawAtlas(inRect, ModTextures.toolbarBackground);
            if (Widgets.ButtonImage(inRect, ModTextures.arquitectMenuIcon)) Find.MainTabsRoot.ToggleTab(DefDatabase<MainButtonDef>.GetNamed("Architect"));
        }
    }
}