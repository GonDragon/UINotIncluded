using RimWorld;
using System;
using Verse;

namespace UINotIncluded.Widget
{
    internal static class AnimalButtons
    {
        private static readonly float width = 90f;

        public static void AnimalButtonsOnGUI(WidgetRow row, float posX, float posY)
        {
            row.Init(posX, posY, UIDirection.RightThenDown);

            Text.Font = GameFont.Tiny;
            if (row.ButtonText("Animals", fixedWidth: (float)Math.Floor(width / 2))) Find.MainTabsRoot.ToggleTab(DefDatabase<MainButtonDef>.GetNamed("Animals"));
            if (row.ButtonText("Wildlife", fixedWidth: (float)Math.Floor(width / 2))) Find.MainTabsRoot.ToggleTab(DefDatabase<MainButtonDef>.GetNamed("Wildlife"));
        }
    }
}