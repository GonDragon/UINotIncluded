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
    static class AnimalButtons
    {
        private static readonly float width = 90f;

        public static void AnimalButtonsOnGUI(WidgetRow row, float posX, float posY)
        {
            row.Init(posX, posY, UIDirection.RightThenDown);

            if (row.ButtonText("Animals", fixedWidth: (float)Math.Floor(width / 2))) Find.MainTabsRoot.ToggleTab(DefDatabase<MainButtonDef>.GetNamed("Animals"));
            if (row.ButtonText("Wildlife", fixedWidth: (float)Math.Floor(width / 2))) Find.MainTabsRoot.ToggleTab(DefDatabase<MainButtonDef>.GetNamed("Wildlife"));
        }

    }
}
