using UnityEngine;

namespace UnifiedRailwaySystem
{
    public static class URSTrack
    {   
        public static void Convert()
        {
            Debug.Log("URSTrack.Convert");

            for (uint i = 0; i < PrefabCollection<NetInfo>.LoadedCount(); ++i)
            {
                NetInfo info = PrefabCollection<NetInfo>.GetLoaded(i);

                ItemClass.Service m_service = info.m_class.m_service;
                ItemClass.SubService m_subService = info.m_class.m_subService;

                if (m_service == ItemClass.Service.PublicTransport && m_subService == ItemClass.SubService.PublicTransportTrain)
                {
                    URSTrainTrack.Convert(info);
                }
                if (m_service == ItemClass.Service.PublicTransport && m_subService == ItemClass.SubService.PublicTransportMetro)
                {
                    URSMetroTrack.Convert(info);
                }
                if (m_service == ItemClass.Service.Road)
                {
                    URSTramTrack.Convert(info);
                }
            }
        }

        public static void Revert()
        {
            // URSTrainTrack.Revert();
            // URSMetroTrack.Revert();
            // URSTramTrack.Revert();
        }
    }
}
