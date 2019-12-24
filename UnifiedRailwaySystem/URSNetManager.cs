using System;
using Harmony;
using UnityEngine;

namespace UnifiedRailwaySystem
{
    namespace URSNetManager
    {
        [HarmonyPatch(typeof(NetManager))]
        [HarmonyPatch("CreateSegment")]
        public class URSCreateSegament
        {
            public static bool Prefix(ref NetInfo info)
            {
                Debug.Log("NetManager.CreateSegament, info: " + info);
                return true;
            }
        }
    }
}
