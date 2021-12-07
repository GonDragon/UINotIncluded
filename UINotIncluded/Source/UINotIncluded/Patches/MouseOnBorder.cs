using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using UnityEngine;
using HarmonyLib;
using System.Reflection.Emit;
using RimWorld.Planet;

namespace UINotIncluded.Patches
{
    [HarmonyPatch(typeof(CameraDriver), "CalculateCurInputDollyVect")]
    internal class CameraDriverPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(ILGenerator gen, IEnumerable<CodeInstruction> instructions)
        {
            bool detected = false;
            bool unload = false;
            List<CodeInstruction> buffer = new List<CodeInstruction>();
            OpCode[] target = new OpCode[] { OpCodes.Ldloca_S, OpCodes.Ldflda, OpCodes.Dup, OpCodes.Ldind_R4, OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Ldfld, OpCodes.Add, OpCodes.Stind_R4 };
            int matches = 0;

            int numbermatches = 0;
            bool numberInjected = false;

            var Field_mouseTouchingScreenBottomEdgeStartTime = AccessTools.Field(typeof(CameraDriver), "mouseTouchingScreenBottomEdgeStartTime");

            foreach (var code in instructions)
            {
                if (code.opcode == OpCodes.Ldc_R4 && (float)code.operand == 20f) numbermatches++;
                if (numbermatches == 3 && !numberInjected)
                {
                    code.operand = 6f;
                    numberInjected = true;
                }

                if (!detected && code.opcode == OpCodes.Ldloca_S) detected = true;
                if (detected)
                {
                    buffer.Add(code);

                    for (int i = 0; i < buffer.Count(); i++)
                    {
                        if (buffer[i].opcode != target[i]) unload = true;
                    }

                    if (buffer.Count() == target.Count() && !unload)
                    {
                        matches++;
                        if(matches < 2)
                        {
                            unload = true;
                        } else
                        {

                            //Call the function
                            yield return new CodeInstruction(OpCodes.Ldarg_0);
                            yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(CameraDriver), "config"));
                            yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(CameraMapConfig), "dollyRateScreenEdge")); // this.config.dollyRateScreenEdge

                            yield return new CodeInstruction(OpCodes.Ldloca_S, 9); //Vector2 vector2

                            yield return new CodeInstruction(OpCodes.Ldarg_0); 
                            yield return new CodeInstruction(OpCodes.Ldflda, Field_mouseTouchingScreenBottomEdgeStartTime); // ref this.mouseTouchingScreenBottomEdgeStartTime
                            
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CameraDriverPatch), nameof(MoveIfTimer)));

                            // Set the flag
                            yield return new CodeInstruction(OpCodes.Ldc_I4_1); //
                            yield return new CodeInstruction(OpCodes.Stloc_1); // flag -> true

                            detected = false;
                            buffer.Clear();
                        }                        
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

        public static void MoveIfTimer(float dollyRateScreenEdge,ref Vector2 vector2, ref float mouseTouchingScreenBottomEdgeStartTime)
        {
            if (!Settings.TabsOnTop)
            {
                vector2.y += dollyRateScreenEdge;
                return;
            }
            if (mouseTouchingScreenBottomEdgeStartTime < 0.0)
            {
                mouseTouchingScreenBottomEdgeStartTime = Time.realtimeSinceStartup;
            }
            if (Time.realtimeSinceStartup - mouseTouchingScreenBottomEdgeStartTime >= 0.280000001192093)
            {
                vector2.y += dollyRateScreenEdge;
            }
        }

        
    }

    //[HarmonyPatch(typeof(WorldCameraDriver), "CalculateCurInputDollyVect")]
    //internal class WorldCameraDriverPatch
    //{
    //    private static IEnumerable<CodeInstruction> Transpiler(ILGenerator gen, IEnumerable<CodeInstruction> instructions)
    //    {
    //        bool detected = false;
    //        bool unload = false;
    //        List<CodeInstruction> buffer = new List<CodeInstruction>();
    //        OpCode[] target = new OpCode[] { OpCodes.Ldloca_S, OpCodes.Ldflda, OpCodes.Dup, OpCodes.Ldind_R4, OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Ldfld, OpCodes.Add, OpCodes.Stind_R4 };

    //        UINI.Warning("Arrancamos el rodeo");
    //        foreach (var code in instructions)
    //        {
    //            if (!detected && code.opcode == OpCodes.Ldloca_S) detected = true;
    //            if (detected)
    //            {
    //                buffer.Add(code);

    //                for (int i = 0; i < buffer.Count(); i++)
    //                {
    //                    if (buffer[i].opcode != target[i]) unload = true;
    //                }

    //                if (buffer.Count() == target.Count() && !unload)
    //                {
    //                    UINI.Warning("Encontramos el paquete! A desempacar");
    //                    yield return new CodeInstruction(OpCodes.Ldarg_0);
    //                    yield return new CodeInstruction(OpCodes.Ldarg_0);
    //                    yield return new CodeInstruction(OpCodes.Ldflda, AccessTools.Field(typeof(WorldCameraDriver), "mouseTouchingScreenBottomEdgeStartTime"));
    //                    yield return new CodeInstruction(OpCodes.Ldloca_S,8); // Vector2 Zero
    //                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(WorldCameraDriverPatch), nameof(MoveIfTimer)));
    //                    yield return new CodeInstruction(OpCodes.Stloc_1);
    //                    detected = false;
    //                    buffer.Clear();
    //                }
    //            }
    //            else
    //            {
    //                yield return code;
    //            }

    //            if (unload)
    //            {
    //                foreach (CodeInstruction codeInstruction in buffer)
    //                {
    //                    yield return codeInstruction;
    //                }
    //                buffer.Clear();
    //                detected = false;
    //                unload = false;
    //            }
    //        }
    //    }


    //    internal bool MoveIfTimer(CameraDriver driver, ref double mouseTouchingScreenBottomEdgeStartTime, Vector2 vector2)
    //    {
    //        UINI.Warning("Si, es acá.");
    //        if (mouseTouchingScreenBottomEdgeStartTime < 0.0)
    //            mouseTouchingScreenBottomEdgeStartTime = Time.realtimeSinceStartup;
    //        if (Time.realtimeSinceStartup - mouseTouchingScreenBottomEdgeStartTime >= 0.280000001192093)
    //            vector2.y -= driver.config.dollyRateScreenEdge;
    //        return true;
    //    }
    //}
}
