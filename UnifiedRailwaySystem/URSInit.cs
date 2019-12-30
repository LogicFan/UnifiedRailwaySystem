using UnityEngine;

namespace UnifiedRailwaySystem
{
    public static class URSInitializer
    {
        public static void Init()
        {
            Debug.Log("URS initializing...");
            AddNetInfo();
            Debug.Log("URS initialized.");
        }

        #region NetInfo
        private static void AddNetInfo()
        {
            NetInfo info = CreateMetroTrack();

            PrefabCollection<NetInfo>.InitializePrefabs("URS", info, null);
            PrefabCollection<NetInfo>.BindPrefabs();
        }

        private static NetInfo CreateMetroTrack()
        {
            NetInfo trainTrack = PrefabCollection<NetInfo>.FindLoaded("Train Track");
            NetInfo metroTrack = PrefabCollection<NetInfo>.FindLoaded("Metro Track");
            NetInfo URSMetroTrack = URSPrefabCloner.Clone<NetInfo>(trainTrack);

            URSMetroTrack.name = "URS Metro Track";
            URSMetroTrack.m_class = metroTrack.m_class;
            URSMetroTrack.m_connectionClass = trainTrack.m_class;
            URSMetroTrack.m_connectGroup = NetInfo.ConnectGroup.DoubleTrain;

            foreach (NetInfo.Lane lane in URSMetroTrack.m_lanes)
            {
                if ((lane.m_vehicleType & VehicleInfo.VehicleType.Train) != 0)
                {
                    lane.m_vehicleType = (lane.m_vehicleType | VehicleInfo.VehicleType.Metro) & ~VehicleInfo.VehicleType.Train;
                }
                if ((lane.m_stopType & VehicleInfo.VehicleType.Train) != 0)
                {
                    lane.m_stopType = (lane.m_stopType | VehicleInfo.VehicleType.Metro) & ~VehicleInfo.VehicleType.Train;
                }
            }

            return URSMetroTrack;
        }
        #endregion
    }
}
