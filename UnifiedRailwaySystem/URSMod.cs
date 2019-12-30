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

            URSInitializer.Init();

            URSPrefab.ChangeTracks();
            HarmonyInstance.DEBUG = true;
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

    #region Prefab
    static class URSPrefab
    {
        public static void ChangeTracks()
        {
            Debug.Log("ChangeTracks()");
            for (uint i = 0; i < PrefabCollection<NetInfo>.LoadedCount(); ++i)
            {
                NetInfo info = PrefabCollection<NetInfo>.GetLoaded(i);

                if(info.name.Contains("Train"))
                {
                    ChangeTrainTrack(info);
                    // ChangeTrainTrackExp(info);
                } 
                else if (info.name.Contains("Metro"))
                {
                    // ChangeMetroTrack(info);
                    ChangeMetroTrackExp(info);
                }
            }
            return;
        }

        private static void ChangeTrainTrackExp(NetInfo info)
        {
            info.m_intersectClass = null;

            ItemClass road = PrefabCollection<NetInfo>.FindLoaded("Basic Road").m_class;
            info.m_connectionClass = road;
        }

        private static void ChangeMetroTrackExp(NetInfo info)
        {
            ItemClass train = PrefabCollection<NetInfo>.FindLoaded("Train Track").m_class;
            info.m_connectionClass = train;

            info.m_class.m_layer = ItemClass.Layer.Default;
        }

        private static void ChangeTrainTrack(NetInfo info)
        {
            Debug.Log("ChangeTrainTrack(" + info + ")");
            foreach (NetInfo.Lane lane in info.m_lanes)
            {
                if ((lane.m_vehicleType & VehicleInfo.VehicleType.Train) != 0)
                {
                    lane.m_vehicleType = lane.m_vehicleType | VehicleInfo.VehicleType.Metro;
                }
                if ((lane.m_stopType & VehicleInfo.VehicleType.Train) != 0)
                {
                    lane.m_stopType = lane.m_stopType | VehicleInfo.VehicleType.Metro;
                }
            }

            return;
        }

        private static void ChangeMetroTrack(NetInfo info)
        {
            Debug.Log("ChangeMetroTrack(" + info + ")");
            info.m_nodeConnectGroups = info.m_nodeConnectGroups | NetInfo.ConnectGroup.DoubleTrain;
            info.m_netAI = PrefabCollection<NetInfo>.FindLoaded("Train Track Tunnel").m_netAI;
        }
    }
    #endregion
}
