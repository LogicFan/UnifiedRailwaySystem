using UnityEngine;

namespace UnifiedRailwaySystem
{
    public static class URSTrack
    {
        enum TrackType
        {
            None = 0x0,
            TrainTrack = 0x1,
            MetroTrack = 0x2,
            TramTrack = 0x4
        }

        public static void ChangeTrack()
        {
            Debug.Log("URSTrack.ChangeTrack");

            for (uint i = 0; i < PrefabCollection<NetInfo>.LoadedCount(); ++i)
            {
                NetInfo info = PrefabCollection<NetInfo>.GetLoaded(i);

                switch (GetTrackType(info.m_class))
                {
                    case TrackType.TrainTrack:
                        {
                            ChangeTrainTrack(info);
                            break;
                        }
                    case TrackType.MetroTrack:
                        {
                            ChangeMetroTrack(info);
                            break;
                        }
                    case TrackType.TramTrack:
                        {
                            ChangeTramTrack(info);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private static TrackType GetTrackType(ItemClass m_class)
        {
            if (m_class.m_service == ItemClass.Service.PublicTransport
                && m_class.m_subService == ItemClass.SubService.PublicTransportTrain)
            {
                return TrackType.TrainTrack;
            }
            if (m_class.m_service == ItemClass.Service.PublicTransport
                && m_class.m_subService == ItemClass.SubService.PublicTransportMetro)
            {
                return TrackType.MetroTrack;
            }
            if (m_class.m_service == ItemClass.Service.Road)
            {
                return TrackType.TramTrack;
            }

            return TrackType.None;
        }

        private static void ChangeTrainTrack(NetInfo info)
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

        private static void ChangeMetroTrack(NetInfo info)
        {
            Debug.Log("URSTrack.ChangeMetroTrack, info: " + info + ".");
            foreach (NetInfo.Lane lane in info.m_lanes)
            {
                // Let Tram vehicle can pass though Metro Track.
                if ((lane.m_vehicleType & VehicleInfo.VehicleType.Metro) != 0)
                {
                    lane.m_vehicleType |= VehicleInfo.VehicleType.Tram;
                }
            }

            // Let Metro Track be able to connect to Train Track and Tram Track
            info.m_class.m_layer |= ItemClass.Layer.Default;

            foreach (NetInfo.Node node in info.m_nodes)
            {
                node.m_connectGroup |= Util.Cache.tramConnectGroup | NetInfo.ConnectGroup.DoubleTrain;
            }
            info.m_connectGroup |= Util.Cache.tramConnectGroup | NetInfo.ConnectGroup.DoubleTrain;
            info.m_nodeConnectGroups |= Util.Cache.tramConnectGroup | NetInfo.ConnectGroup.DoubleTrain;

            info.m_connectionClass = Util.Cache.tramTrackItemClass;
        }

        private static void ChangeTramTrack(NetInfo info)
        {
            Debug.Log("URSTrack.ChangeTramTrack, info: " + info + ".");
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

        public static void UnchangeTrack()
        {

        }
    }
}
