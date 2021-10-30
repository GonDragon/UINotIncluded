using RimWorld;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    internal static class ArchitectMenuButton
    {
        public static void ArchitectButtonOnGUI(float posX, float posY, float size)
        {
            Rect inRect = new Rect(posX, posY, size, size);
            Widgets.DrawAtlas(inRect, ModTextures.toolbarBackground);
            if (Widgets.ButtonImage(inRect, ModTextures.arquitectMenuIcon)) Find.MainTabsRoot.ToggleTab(DefDatabase<MainButtonDef>.GetNamed("Architect"));
        }
    }
}