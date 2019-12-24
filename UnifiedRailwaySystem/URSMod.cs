using ICities;
using UnityEngine;
using Harmony;
using System.Reflection;

namespace UnifiedRailwaySystem
{
    public class URSMod : IUserMod, ILoadingExtension
    {
        HarmonyInstance harmony;

        #region IUserMod
        public string Name
        {
            get { return "Unified Railway System"; }
        }

        public string Description
        {
            get { return "Let Train and Metro use the same railway."; }
        }
        #endregion

        #region ILoadingExtension
        public void OnCreated(ILoading loading) { }

        public void OnLevelLoaded(LoadMode mode)
        {
            Debug.Log("URSMod.OnLevelLoaded");
            harmony = HarmonyInstance.Create("com.logic.urs");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void OnLevelUnloading()
        {
            Debug.Log("URSMod.OnLevelUnloading");
            harmony.UnpatchAll("com.logic.urs");
        }

        public void OnReleased() { }
        #endregion
    }
}
