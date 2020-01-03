using System.Collections.Generic;
using UnityEngine;

namespace UnifiedRailwaySystem
{ 
    public static class URSTrainTrack
    {
        public static void Backup(NetInfo info)
        {
            Debug.Log("URSTrainTrack.Backup, info: " + info + ".");

            BackupInfo backup = new BackupInfo();
            backup.lanes_vehicleType = new VehicleInfo.VehicleType[info.m_lanes.Length];
            backup.nodes_connectGroup = new NetInfo.ConnectGroup[info.m_nodes.Length];

            for (int i = 0; i < info.m_lanes.Length; ++i)
            {
                backup.lanes_vehicleType[i] = info.m_lanes[i].m_vehicleType;
            }
            for (int i = 0; i < info.m_nodes.Length; ++i)
            {
                 backup.nodes_connectGroup[i] = info.m_nodes[i].m_connectGroup;
            }
            backup.connectGroup = info.m_connectGroup;
            backup.nodeConnectGroup = info.m_nodeConnectGroups;
            backup.connectionClass = info.m_connectionClass;
            backup.intersectClass = info.m_intersectClass;

            _backupDictionary.Add(info, backup);
        }

        public static void Change(NetInfo info)
        {
            Debug.Log("URSTrack.ChangeTrainTrack, info: " + info + ".");

            foreach (NetInfo.Lane lane in info.m_lanes)
            {
                // Let Metro and Tram vehicle can pass though Train Track.
                if ((lane.m_vehicleType & VehicleInfo.VehicleType.Train) != 0)
                {
                    lane.m_vehicleType |= VehicleInfo.VehicleType.Metro | VehicleInfo.VehicleType.Tram;
                }
            }

            // Let Train Track be able to connect to Metro Track and Tram Track
            foreach (NetInfo.Node node in info.m_nodes)
            {
                if ((node.m_connectGroup & Util.Cache.trainConnectGroup) != 0)
                {
                    node.m_connectGroup |= Util.Cache.tramConnectGroup;
                }
            }
            info.m_connectGroup |= Util.Cache.tramConnectGroup;
            info.m_nodeConnectGroups |= Util.Cache.tramConnectGroup;

            info.m_connectionClass = Util.Cache.tramTrackItemClass;
            info.m_intersectClass = null;

            return;
        }

        public static void Restore()
        {
            foreach(KeyValuePair<NetInfo, BackupInfo> kvp in _backupDictionary)
            {
                NetInfo info = kvp.Key;
                BackupInfo backup = kvp.Value;

                Debug.Log("URSTrainTrack.Restore, info: " + info + ".");
                
                for (int i = 0; i < info.m_lanes.Length; ++i)
                {
                    info.m_lanes[i].m_vehicleType = backup.lanes_vehicleType[i];
                }
                for(int i = 0; i < info.m_nodes.Length; ++i)
                {
                    info.m_nodes[i].m_connectGroup = backup.nodes_connectGroup[i];
                }
                info.m_connectGroup = backup.connectGroup;
                info.m_nodeConnectGroups = backup.nodeConnectGroup;
                info.m_connectionClass = backup.connectionClass;
                info.m_intersectClass = backup.intersectClass;
            }
        }

        private class BackupInfo
        {
            public VehicleInfo.VehicleType[] lanes_vehicleType;
            public NetInfo.ConnectGroup[] nodes_connectGroup;
            public NetInfo.ConnectGroup connectGroup;
            public NetInfo.ConnectGroup nodeConnectGroup;
            public ItemClass connectionClass;
            public ItemClass intersectClass;
        }

        private static Dictionary<NetInfo, BackupInfo> _backupDictionary;
    }
}
