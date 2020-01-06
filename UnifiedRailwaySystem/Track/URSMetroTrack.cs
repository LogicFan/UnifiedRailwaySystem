using System.Collections.Generic;
using UnityEngine;

namespace UnifiedRailwaySystem
{
    public static class URSMetroTrack
    {
        private static List<Util.BackupInfo> _backups = new List<Util.BackupInfo>();

        public static void Convert(NetInfo info)
        {
            Debug.Log("URSMetroTrack.Convert, info: " + info + ".");

            Util.BackupInfo backup = new BackupInfo();
            backup.Backup(info);
            _backups.Add(backup);

            foreach (NetInfo.Lane lane in info.m_lanes)
            {
                // Let Tram vehicle can pass though Metro Track.
                if ((lane.m_vehicleType & VehicleInfo.VehicleType.Metro) != 0)
                {
                    lane.m_vehicleType |= VehicleInfo.VehicleType.Tram;
                }
            }

            // Let Metro Track be able to connect to Train Track and Tram Track
            foreach (NetInfo.Node node in info.m_nodes)
            {
                node.m_connectGroup |= Util.Cache.tramConnectGroup | NetInfo.ConnectGroup.DoubleTrain;
            }
            info.m_connectGroup |= Util.Cache.tramConnectGroup | NetInfo.ConnectGroup.DoubleTrain;
            info.m_nodeConnectGroups |= Util.Cache.tramConnectGroup | NetInfo.ConnectGroup.DoubleTrain;

            info.m_class.m_layer |= ItemClass.Layer.Default;

            info.m_connectionClass = Util.Cache.tramTrackItemClass;
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
            private NetInfo.ConnectGroup[] _nodes_connectGroup;
            private NetInfo.ConnectGroup _connectGroup;
            private NetInfo.ConnectGroup _nodeConnectGroup;
            private ItemClass.Layer _class_layer;
            private ItemClass _connectionClass;

            public void Backup<T>(T info) where T : PrefabInfo
            {
                _info = info as NetInfo;

                System.Diagnostics.Debug.Assert(_info != null);

                _lanes_vehicleType = new VehicleInfo.VehicleType[_info.m_lanes.Length];
                _nodes_connectGroup = new NetInfo.ConnectGroup[_info.m_nodes.Length];

                for (int i = 0; i < _info.m_lanes.Length; ++i)
                {
                    _lanes_vehicleType[i] = _info.m_lanes[i].m_vehicleType;
                }
                for (int i = 0; i < _info.m_nodes.Length; ++i)
                {
                    _nodes_connectGroup[i] = _info.m_nodes[i].m_connectGroup;
                }
                _connectGroup = _info.m_connectGroup;
                _nodeConnectGroup = _info.m_nodeConnectGroups;
                _class_layer = _info.m_class.m_layer;
                _connectionClass = _info.m_connectionClass;
            }

            public void Restore()
            {
                System.Diagnostics.Debug.Assert(_info != null);

                for (int i = 0; i < _info.m_lanes.Length; ++i)
                {
                    _info.m_lanes[i].m_vehicleType = _lanes_vehicleType[i];
                }
                for (int i = 0; i < _info.m_nodes.Length; ++i)
                {
                    _info.m_nodes[i].m_connectGroup = _nodes_connectGroup[i];
                }
                _info.m_connectGroup = _connectGroup;
                _info.m_nodeConnectGroups = _nodeConnectGroup;
                _info.m_class.m_layer = _class_layer;
                _info.m_connectionClass = _connectionClass;
            }
        }
    }
}
