using ICities;
using UnityEngine;
using Harmony;
using System.Reflection;

namespace UnifiedRailwaySystem
{
    public class URSMod : IUserMod, ILoadingExtension
    {
        // HarmonyInstance harmony;

        string version = "v0.0.1 (alpha)";

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

            URSPrefab.ChangeTracks();

            // harmony = HarmonyInstance.Create("com.logic.urs");
            // harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void OnLevelUnloading()
        {
            Debug.Log("URSMod.OnLevelUnloading");
            // harmony.UnpatchAll("com.logic.urs");
        }

        public void OnReleased() { }
        #endregion
    }

    #region Prefab
    static class URSPrefab
    {
        public static void ChangeTracks()
        {
            Debug.Log("URSPrefab.ChangeTracks");
            for (uint i = 0; i < PrefabCollection<NetInfo>.LoadedCount(); ++i)
            {
                NetInfo info = PrefabCollection<NetInfo>.GetLoaded(i);

                if(info.name.Contains("Train"))
                {
                    ChangeTrainTrack(info);
                } 
                else if (info.name.Contains("Metro"))
                {
                    ChangeMetroTrack(info);
                }
            }
            return;
        }

        private static void ChangeTrainTrack(NetInfo info)
        {
            Debug.Log("URSPrefab.ChangeTrainTrack, info: " + info + ".");
            foreach (NetInfo.Lane lane in info.m_lanes)
            {
                // Let Metro vehicle can pass though Train Track.
                if ((lane.m_vehicleType & VehicleInfo.VehicleType.Train) != 0)
                {
                    lane.m_vehicleType |= VehicleInfo.VehicleType.Metro;
                }
            }

            return;
        }

        private static void ChangeMetroTrack(NetInfo info)
        {
            // Add layer Default, which is the same layer as Train Track.
            info.m_class.m_layer |= ItemClass.Layer.Default;

            // Let Metro Track be able to connect to Train Track.
            ItemClass train = PrefabCollection<NetInfo>.FindLoaded("Train Track").m_class;
            info.m_connectionClass = train;
        }

        #region notInUse
        private static void NotInUseChangeTrainTrackExp(NetInfo info)
        {
            info.m_intersectClass = null;

            ItemClass road = PrefabCollection<NetInfo>.FindLoaded("Basic Road").m_class;
            info.m_connectionClass = road;
        }

        private static void NotInUseChangeMetroTrackExp(NetInfo info)
        {
            ItemClass train = PrefabCollection<NetInfo>.FindLoaded("Train Track").m_class;
            info.m_connectionClass = train;

            info.m_class.m_layer = ItemClass.Layer.Default;
        }

        private static void NotInUseChangeTrainTrack(NetInfo info)
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

        private static void NotInUseChangeMetroTrack(NetInfo info)
        {
            Debug.Log("ChangeMetroTrack(" + info + ")");
            info.m_nodeConnectGroups = info.m_nodeConnectGroups | NetInfo.ConnectGroup.DoubleTrain;
            info.m_netAI = PrefabCollection<NetInfo>.FindLoaded("Train Track Tunnel").m_netAI;
        }
        #endregion
    }
    #endregion
}
