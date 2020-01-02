﻿using ColossalFramework.UI;
using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace UnifiedRailwaySystem.URSRoadAI
{
    /** Elimiate the ToolBase.ToolErrors.CannotCrossTrack.
     * Ref: UnifiedRailwaySystem.URSTrainTrackAI.URSCheckBuildPosition
     */
    [HarmonyPatch(typeof(RoadAI))]
    [HarmonyPatch("CheckBuildPosition")]
    public static class URSCheckBuildPosition
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            var postfix = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldc_I4, -8388609),
                new CodeInstruction(OpCodes.Conv_I8),
                new CodeInstruction(OpCodes.And),
                new CodeInstruction(OpCodes.Stloc_0),
                new CodeInstruction(OpCodes.Ldloc_0)
            };

            // check if the assembly is as expected
            if (codes[241].opcode == OpCodes.Ret)
            {
                // add postfix just before return statement.
                codes.InsertRange(241, postfix);
            }
            else
            {
                ExceptionPanel panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
                panel.SetMessage("Harmony Trainpiler Error", "Error on URSRoadAI.cs, line 39", true);
            }

            // string s = "";
            // for (int i = 0; i < codes.Count; ++i)
            // {
            //     s += "RoadAI, codes[" + i + "]: " + codes[i] + "\n";
            // }
            // Debug.Log(s);

            return codes.AsEnumerable();
        }
    }
}