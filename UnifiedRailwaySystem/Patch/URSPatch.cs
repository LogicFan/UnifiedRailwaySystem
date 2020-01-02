using Harmony;
using System.Reflection;
using UnityEngine;

namespace UnifiedRailwaySystem
{
    /// <summary>
    /// A class to control all Harmony patches.
    /// </summary>
    public static class URSPatch
    {
        private static HarmonyInstance harmony;

        /// <summary>
        /// Patch all harmony patches.
        /// </summary>
        public static void Patch()
        {
            Debug.Log("URSPatch.Patch");

            harmony = HarmonyInstance.Create("com.logic.urs");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Unpatch all harmony patches.
        /// </summary>
        public static void Unpatch()
        {
            Debug.Log("URSPatch.Unpatch");
            
            harmony.UnpatchAll("com.logic.urs");
        }
    }
}
