using ColossalFramework.UI;
using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace UnifiedRailwaySystem.PatchTrainTrackAI
{
    /// <summary>
    /// Elimiate the <c>ToolBase.ToolErrors.CannotCrossTrack</c> between Train Track and Tram Track.
    /// Ref: UnifiedRailwaySystem.PatchRoadAI.URSCheckBuildPosition
    /// </summary>
    [HarmonyPatch(typeof(TrainTrackAI))]
    [HarmonyPatch("CheckBuildPosition")]
    public static class URSCheckBuildPosition
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            var postfix = new CodeInstruction[]
            {
                // toolErrors &= ~ToolBase.ToolErrors.CannotCrossTrack;
                new CodeInstruction(OpCodes.Ldc_I4, -8388609),
                new CodeInstruction(OpCodes.Conv_I8),
                new CodeInstruction(OpCodes.And),
                new CodeInstruction(OpCodes.Stloc_0),
                new CodeInstruction(OpCodes.Ldloc_0)
            };

            // check if the assembly is as expected.
            if (codes[221].opcode == OpCodes.Ret)
            {
                // add postfix just before return statement.
                codes.InsertRange(221, postfix);
            }
            else
            {
                ExceptionPanel panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
                panel.SetMessage("Harmony Trainpiler Error", "Error on TrainTrackAI.CheckBuildPosition.", true);
            }

            // string s = "";
            // for (int i = 0; i < codes.Count; ++i)
            // {
            //     s += "TrainTrackAI, codes[" + i + "]: " + codes[i] + "\n";
            // }
            // Debug.Log(s);

            return codes.AsEnumerable();
        }
    }
}
