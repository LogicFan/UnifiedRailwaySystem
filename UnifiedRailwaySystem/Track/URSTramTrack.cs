using System.Collections.Generic;
using UnityEngine;

namespace UnifiedRailwaySystem
{
    public static class URSTramTrack
    {
        private static List<Util.BackupInfo> _backups = new List<Util.BackupInfo>();

        public static void Convert(NetInfo info)
        {
            Debug.Log("URSTramTrack.Convert, info: " + info + ".");

            Util.BackupInfo backup = new BackupInfo();
            backup.Backup(info);
            _backups.Add(backup);

            foreach (NetInfo.Lane lane in info.m_lanes)
            {
                // Let Metro vehicle can pass though Tram Track.
                if ((lane.m_vehicleType & VehicleInfo.VehicleType.Tram) != 0)
                {
                    lane.m_vehicleType |= VehicleInfo.VehicleType.Metro;
                }
            }

            // To be able to let Tram Track connect to Train Track, please see
            // ref: UnifiedRailwaySystem.PatchRoadBridgeAI.URSCanConnectTo
            // ref: UnifiedRailwaySystem.PatchTrainTrackBaseAI.URSCanConnectTo
            // ref: UnifiedRailwaySystem.PatchRoadAI.URSCheckBuildPosition
            // ref: UnifiedRailwaySystem.PatchTrainTrackAI.URSCheckBuildPosition
        }

        public static void Revert()
        {
            foreach (Util.BackupInfo backup in _backups)
            {
                backup.Restore();
            }
        }

        class BackupInfo : Util.BackupInfo
        {
            NetInfo _info;

            private VehicleInfo.VehicleType[] _lanes_vehicleType;

            public void Backup<T>(T info) where T : PrefabInfo
            {
                _info = info as NetInfo;

                System.Diagnostics.Debug.Assert(_info != null);

                _lanes_vehicleType = new VehicleInfo.VehicleType[_info.m_lanes.Length];

                for (int i = 0; i < _info.m_lanes.Length; ++i)
                {
                    _lanes_vehicleType[i] = _info.m_lanes[i].m_vehicleType;
                }
            }

            public void Restore()
            {
                System.Diagnostics.Debug.Assert(_info != null);

                for (int i = 0; i < _info.m_lanes.Length; ++i)
                {
                    _info.m_lanes[i].m_vehicleType = _lanes_vehicleType[i];
                }
            }
        }
    }
}
