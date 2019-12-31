using ICities;
using UnityEngine;

namespace UnifiedRailwaySystem
{
    public class URSMod : IUserMod, ILoadingExtension
    {
        string version = "v0.1.1 (beta)";

        #region IUserMod
        public string Name
        {
            get { return "Unified Railway System"; }
        }

        public string Description
        {
            get { return "[" + version + "] Let Train, Metro and Tram share the railway."; }
        }
        #endregion

        #region ILoadingExtension
        public void OnCreated(ILoading loading) { }

        public void OnLevelLoaded(LoadMode mode)
        {
            Debug.Log("URSMod.OnLevelLoaded");

            Util.Initialize();
            URSTrack.ChangeTrack();
            URSPatch.Patch();
        }

        public void OnLevelUnloading()
        {
            Debug.Log("URSMod.OnLevelUnloading");

            URSTrack.UnchangeTrack();
            URSPatch.Unpatch();
        }

        public void OnReleased() { }
        #endregion
    }
}
