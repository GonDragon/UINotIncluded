using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;
using HarmonyLib;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(Messages), "MessagesDoGUI")]
    class MessagesDoGUIPatch
    {
        public static bool Prefix(ref List<Verse.Message> ___liveMessages)
        {
            Text.Font = GameFont.Small;
            int x = (int)Messages.MessagesTopLeftStandard.x;
            int y = (int)Messages.MessagesTopLeftStandard.y;
            if (UINotIncludedSettings.tabsOnTop) y += (int)UIManager.ExtendedBarHeight;
            if (Current.Game != null && Find.ActiveLesson.ActiveLessonVisible)
                y += (int)Find.ActiveLesson.Current.MessagesYOffset;
            for (int index = ___liveMessages.Count - 1; index >= 0; --index)
            {
                ___liveMessages[index].Draw(x, y);
                y += 26;
            }
            return false;
        }
    }
}
