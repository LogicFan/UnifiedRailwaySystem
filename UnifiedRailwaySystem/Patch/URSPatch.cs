using Harmony;
using System.Reflection;

namespace UnifiedRailwaySystem
{
    public static class URSPatch
    {
        private static HarmonyInstance harmony;

        public static void Patch()
        {
            harmony = HarmonyInstance.Create("com.logic.urs");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void Unpatch()
        {
            harmony.UnpatchAll("com.logic.urs");
        }
    }
}
