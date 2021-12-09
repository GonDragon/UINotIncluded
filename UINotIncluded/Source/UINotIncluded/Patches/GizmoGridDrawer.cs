﻿using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace UINotIncluded.Patches
{
    [HarmonyPatch(typeof(GizmoGridDrawer), "DrawGizmoGrid")]
    internal class GizmoGridDrawerPatch
    {
        private static FieldInfo vector2_y = AccessTools.Field(typeof(Vector2), "y");

        private static IEnumerable<CodeInstruction> Transpiler(ILGenerator gen, IEnumerable<CodeInstruction> instructions)
        {
            bool detected = false;
            bool unload = false;
            List<CodeInstruction> buffer = new List<CodeInstruction>();
            OpCode[] target = new OpCode[] { OpCodes.Ldsfld, OpCodes.Ldc_I4_S, OpCodes.Sub, OpCodes.Conv_R4 };

            foreach (var code in instructions)
            {
                if (!detected && code.opcode == OpCodes.Ldsfld) detected = true;
                if (detected)
                {
                    buffer.Add(code);

                    for (int i = 0; i < buffer.Count(); i++)
                    {
                        if (buffer[i].opcode != target[i]) unload = true;
                    }

                    if (buffer.Count() == target.Count() && !unload)
                    {
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GizmoGridDrawerPatch), nameof(PatchedBaseY)));
                        detected = false;
                        buffer.Clear();
                    }
                }
                else
                {
                    yield return code;
                }

                if (unload)
                {
                    foreach (CodeInstruction codeInstruction in buffer)
                    {
                        yield return codeInstruction;
                    }
                    buffer.Clear();
                    detected = false;
                    unload = false;
                }
            }
        }

        private static float PatchedBaseY()
        {
            return Settings.TabsOnBottom ? UI.screenHeight - 33f : UI.screenHeight;
        }
    }
}