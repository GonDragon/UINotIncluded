using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;
using HarmonyLib;
using System.Reflection.Emit;
using System.Reflection;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(Messages), "MessagesDoGUI")]
    class MessagesDoGUIPatch
    {

        static FieldInfo vector2_y = AccessTools.Field(typeof(Vector2), "y");

        static IEnumerable<CodeInstruction> Transpiler(ILGenerator gen, IEnumerable<CodeInstruction> instructions)
        {
            CodeInstruction prev = instructions.First();
            bool patched = false;

            foreach (var code in instructions)
            {
                yield return code;
                if (!patched)
                {
                    if (prev.opcode == OpCodes.Ldfld && (FieldInfo)prev.operand == vector2_y)
                    {
                        patched = true;
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MessagesDoGUIPatch), nameof(YOffsetAdjustment)));
                        yield return new CodeInstruction(OpCodes.Add);
                    }
                    prev = code;
                }
            }
        }

        static int YOffsetAdjustment()
        {
            return Settings.TabsOnTop ? (int)UIManager.ExtendedBarHeight : 0;
        }
    }

}
