using ColossalFramework.UI;
using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace UnifiedRailwaySystem.URSTrainTrackBaseAI
{
    /** Elimiate the ToolBase.ToolErrors.ObjectCollision between TrainTrack
     * and Road Bridge.
     * Ref: UnifiedRailwaySystem.URSRoadBridgeAI.URSCanConnectTo
     */
    [HarmonyPatch(typeof(TrainTrackBaseAI))]
    [HarmonyPatch("CanConnectTo")]
    public static class URSCanConnectTo
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            // check if the assembly is as expected
            if (codes[4].opcode == OpCodes.Call && codes[114].opcode == OpCodes.Ret)
            {
                // remove 5 - 113 (inclusive) line of IL code
                codes.RemoveRange(5, 109);
            }
            else
            {
                ExceptionPanel panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
                panel.SetMessage("Harmony Trainpiler Error", "Error on URSTrainTrackBaseAI.cs, line 31", true);
            }

            // string s = "";
            // for (int i = 0; i < codes.Count; ++i)
            // {
            //     s += "TrainTrackBaseAI, codes[" + i + "]: " + codes[i] + "\n";
            // }
            // Debug.Log(s);
            return codes.AsEnumerable();
        }
    }
}
