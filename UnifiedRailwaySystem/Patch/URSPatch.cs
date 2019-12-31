using Harmony;
using System.Reflection;
using UnityEngine;

namespace UnifiedRailwaySystem
{
    public static class URSPatch
    {
        private static HarmonyInstance harmony;

        public static void Patch()
        {
            Debug.Log("URSPatch.Patch");
            harmony = HarmonyInstance.Create("com.logic.urs");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void Unpatch()
        {
            Debug.Log("URSPatch.Unpatch");
            harmony.UnpatchAll("com.logic.urs");
        }
    }
}
