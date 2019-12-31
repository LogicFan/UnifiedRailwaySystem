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

            for(uint i = 0; i < PrefabCollection<NetInfo>.LoadedCount(); ++i)
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
            Debug.Log("URSTrack.ChangeMetroTrack, info: " + info + ".");
            // Add layer Default, which is the same layer as Train Track.
            info.m_class.m_layer |= ItemClass.Layer.Default;

            // Let Metro Track be able to connect to Train Track.
            info.m_connectionClass = Util.trainItemClass;
        }

        public static void UnchangeTrack()
        {

        }
    }
}
