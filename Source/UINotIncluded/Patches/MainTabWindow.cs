using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace UINotIncluded
{
    public static class MainTabWindowPatchHelper
    {

        public static void CenterRectOnScreen(ref Rect rect)
        {
            float newX = (float)Math.Floor(UI.screenWidth / 2f) - (float)Math.Floor(rect.width / 2f);
            float newY = (float)Math.Floor(UI.screenHeight / 2f) - (float)Math.Floor(rect.height / 2f);
          
            rect.x = newX;
            rect.y = newY;
        }
    }



    [HarmonyPatch(typeof(Window), "SetInitialSizeAndPosition")]
    public class MainTabWindowPatch
    {
        public static void Postfix(ref Rect ___windowRect)
        {
            MainTabWindowPatchHelper.CenterRectOnScreen(ref ___windowRect);
        }
    }

    //[HarmonyPatch(typeof(MainTabWindow_Assign))]
    //public class MainTabWindow_AssignPatch
    //{

    //}

    //[HarmonyPatch(typeof(MainTabWindow_Factions))]
    //public class MainTabWindow_FactionsPatch
    //{

    //}

    //[HarmonyPatch(typeof(MainTabWindow_History))]
    //public class MainTabWindow_HistoryPatch
    //{

    //}

    //[HarmonyPatch(typeof(MainTabWindow_Ideos))]
    //public class MainTabWindow_IdeosPatch
    //{

    //}

    //[HarmonyPatch(typeof(MainTabWindow_Menu))]
    //public class MainTabWindow_MenuPatch
    //{

    //}

    //[HarmonyPatch(typeof(MainTabWindow_Quests))]
    //public class MainTabWindow_QuestsPatch
    //{

    //}

    //[HarmonyPatch(typeof(MainTabWindow_Research))]
    //public class MainTabWindow_ResearchPatch
    //{

    //}

    //[HarmonyPatch(typeof(MainTabWindow_Schedule))]
    //public class MainTabWindow_SchedulePatch
    //{

    //}

    //[HarmonyPatch(typeof(MainTabWindow_Wildlife))]
    //public class MainTabWindow_WildlifePatch
    //{

    //}

    //[HarmonyPatch(typeof(MainTabWindow_Work))]
    //public class MainTabWindow_WorkPatch
    //{

    //}
}
