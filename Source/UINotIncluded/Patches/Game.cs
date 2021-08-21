using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(Game), nameof(Game.UpdatePlay))]
    public class Patch_DirectHotkeys
    {
        static void Prefix()
        {
            if (!UINotIncludedSettings.altInspectActive) { MouseReadoutWidget.AltInspector = false; return; }
            if (!(Event.current.type == EventType.KeyDown || Event.current.type == EventType.KeyUp)) return;
            if (Event.current.keyCode != KeyCode.LeftAlt) return;
            MouseReadoutWidget.AltInspector = Event.current.type == EventType.KeyDown;
            Event.current.Use();
        }
    }
}
