using System;
using Harmony;
using UnityEngine;
using ColossalFramework;
using System.Reflection;

// currently not used

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
                if (info.name.Contains("Metro Track"))
                {
                    // info = PrefabCollection<NetInfo>.FindLoaded("URS Metro Track");
                    // Debug.Log("NetManager.CreateSegament, info: " + info);
                    // info = PrefabCollection<NetInfo>.FindLoaded("Train Track Tunnel");

                }
                return true;
            }
        }   
    }
}
