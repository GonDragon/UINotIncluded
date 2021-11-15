using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;
using System.Reflection;

namespace UINotIncluded
{
    [HarmonyPatch(typeof(GlobalControls), "GlobalControlsOnGUI")]
    internal class GlobalControl_TranspilerPatch
    {
        private static readonly MethodInfo utility_DoDate = AccessTools.Method(typeof(GlobalControlsUtility), "DoDate");
        private static readonly MethodInfo globalControls_Temperaturestring = AccessTools.Method(typeof(GlobalControls), "TemperatureString");

        private static IEnumerable<CodeInstruction> Transpiler(ILGenerator gen, IEnumerable<CodeInstruction> instructions)
        {
            CodeInstruction prev = instructions.First();
            bool patchWeater = false;
            bool patchWeater_finish = false;
            bool patchedTemp = false;

            foreach (var code in instructions)
            {
                if (!patchWeater)
                {
                    if (prev.opcode == OpCodes.Call && (MethodInfo)prev.operand == utility_DoDate)
                    {
                        patchWeater = true;

                        yield return new CodeInstruction(OpCodes.Ldloc_0);
                        yield return new CodeInstruction(OpCodes.Ldloca_S, 1);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GlobalControl_TranspilerPatch), nameof(DoWeather)));
                        continue;
                    }
                    yield return code;
                    prev = code;
                } else if (!patchWeater_finish)
                {
                    if (prev.opcode == OpCodes.Newobj)
                    {
                        yield return code;
                        patchWeater_finish = true;
                    }
                    prev = code;
                } else if (!patchedTemp)
                {
                    if (prev.opcode == OpCodes.Call && (MethodInfo)prev.operand == globalControls_Temperaturestring)
                    {
                        patchedTemp = true;

                        yield return new CodeInstruction(OpCodes.Ldloc_0);
                        yield return new CodeInstruction(OpCodes.Ldloca_S, 1);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GlobalControl_TranspilerPatch), nameof(DoTemperature)));
                        continue;
                    }
                    yield return code;
                    prev = code;
                } else
                {
                    yield return code;
                }
            }
        }

        private static void DoWeather(float x, ref float y)
        {
            if (!Settings.vanillaWeather) return;
            y -= 26f;
            Rect rect = new Rect(x - 22f, y,230f,26f);
            Find.CurrentMap.weatherManager.DoWeatherGUI(rect);
        }

        private static void DoTemperature(string label, float x, ref float y)
        {
            if (!Settings.vanillaWeather) return;
            y -= 26f;
            Rect rect = new Rect(x - 100f, y, 293f, 26f);
            Widgets.Label(rect, label);
        }



    }

    //[HarmonyPatch(typeof(GlobalControls))]
    //internal class GlobalControlsPatches
    //{
    //    [HarmonyPrefix]
    //    [HarmonyPatch("TemperatureString"), HarmonyPriority(Priority.High)]
    //    public static bool TemperatureString_Patch()
    //    {
    //        return Settings.vanillaWeather;
    //    }
    //}

    [HarmonyPatch(typeof(GlobalControlsUtility))]
    internal class GlobalControlsUtilityPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoPlaySettings)), HarmonyPriority(Priority.Low)]
        private static bool DoPlaySettingsPatch(WidgetRow rowVisibility, bool worldView, ref float curBaseY)
        {
            if (Settings.TabsOnBottom) curBaseY -= UIManager.ExtendedBarHeight;
            if (Settings.useDesignatorBar && !Settings.designationsOnLeft && !worldView) curBaseY -= 88f;
            if (Settings.togglersOnTop) curBaseY += 35;
            float borderGap = 4f;
            float initialY = Settings.togglersOnTop ? (Settings.TabsOnTop ? UIManager.ExtendedBarHeight + borderGap : borderGap) : curBaseY;
            rowVisibility.Init((float)UI.screenWidth - borderGap, initialY, Settings.togglersOnTop ? UIDirection.LeftThenDown : UIDirection.LeftThenUp, Settings.togglersOnTop ? 250f : 180f);
            Find.PlaySettings.DoPlaySettingsGlobalControls(rowVisibility, worldView);
            if (!Settings.togglersOnTop) curBaseY = rowVisibility.FinalY;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoTimespeedControls)), HarmonyPriority(Priority.Low)]
        private static bool DoTimespeedControls_Patch(ref float curBaseY)
        {
            if (Settings.vanillaControlSpeed) return true;
            curBaseY += 4f;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoDate)), HarmonyPriority(Priority.Low)]
        private static bool DoDate_Patch()
        {
            return Settings.vanillaDate;
        }

        //[HarmonyPostfix]
        //[HarmonyPatch(nameof(GlobalControlsUtility.DoDate))]
        //private static void DoDate_PostifxPatch(ref float curBaseY)
        //{
        //    if (!Settings.vanillaWeather) curBaseY += 50;
        //}

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GlobalControlsUtility.DoRealtimeClock)), HarmonyPriority(Priority.Low)]
        private static bool DoRealtimeClock_Patch(ref float curBaseY)
        {
            if (Settings.vanillaRealtime) return true;
            curBaseY += 4f;
            return false;
        }
    }

    //[HarmonyPatch(typeof(WeatherManager))]
    //internal class WeatherManagerPatches
    //{
    //    [HarmonyPrefix]
    //    [HarmonyPatch(nameof(WeatherManager.DoWeatherGUI)), HarmonyPriority(Priority.High)]
    //    private static bool DoWeatherGUI_PrefixPatch(ref Rect rect)
    //    {
    //        rect.width = Settings.vanillaWeather ? rect.width : 0;
    //        return Settings.vanillaWeather;
    //    }
    //}

    [HarmonyPatch(typeof(PlaySettings), "DoPlaySettingsGlobalControls")]
    internal class DoPlaySettingsGlobalControlsPatch
    {
        public static void Postfix(WidgetRow row, bool worldView)
        {
            if (worldView)
            {
            }
            else
            {
                row.ToggleableIcon(ref UIManager.toggleAltInspector, ModTextures.iconAltInspector, "UINotIncluded.Playsetting.altinspect.tooltip".Translate(), SoundDefOf.Mouseover_ButtonToggle);
            }
        }
    }
}