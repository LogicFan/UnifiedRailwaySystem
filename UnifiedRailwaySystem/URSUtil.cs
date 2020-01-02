using ICities;
using UnityEngine;

// currently not used

namespace UnifiedRailwaySystem
{
    public static class Util
    {
        public static class Cache
        {
            #region TrackItemClass
            private static ItemClass _trainTrackItemClass;
            private static ItemClass _metroTrackItemClass;
            private static ItemClass _tramTrackItemClass;
            public static ItemClass trainTrackItemClass => _trainTrackItemClass;
            public static ItemClass metroTrackItemClass => _metroTrackItemClass;
            public static ItemClass tramTrackItemClass => _tramTrackItemClass;
            #endregion

            #region ConnectGroup
            private static NetInfo.ConnectGroup _trainConnectGroup;
            private static NetInfo.ConnectGroup _tramConnectGroup;
            public static NetInfo.ConnectGroup trainConnectGroup => _trainConnectGroup;
            public static NetInfo.ConnectGroup tramConnectGroup => _tramConnectGroup;
            #endregion

            public static void Initialize()
            {
                _trainTrackItemClass = PrefabCollection<NetInfo>.FindLoaded("Train Track").m_class;
                _metroTrackItemClass = PrefabCollection<NetInfo>.FindLoaded("Metro Track").m_class;
                _tramTrackItemClass = PrefabCollection<NetInfo>.FindLoaded("Basic Road").m_class;

                _trainConnectGroup = NetInfo.ConnectGroup.DoubleTrain
                | NetInfo.ConnectGroup.SingleTrain
                | NetInfo.ConnectGroup.TrainStation;
                _tramConnectGroup = NetInfo.ConnectGroup.CenterTram
                | NetInfo.ConnectGroup.NarrowTram
                | NetInfo.ConnectGroup.SingleTram
                | NetInfo.ConnectGroup.WideTram;
            }
        }

        public static void Initialize()
        {
            Debug.Log("Util.Initialize");
            Cache.Initialize();
        }

        public static T Clone<T>(T originalPrefab) where T : PrefabInfo
        {
            var instance = Object.Instantiate(originalPrefab.gameObject);

            // magical code comes from MOM
            instance.transform.parent = originalPrefab.gameObject.transform;
            // instance.transform.SetParent(transform);
            // instance.transform.localPosition = new Vector3(-7500, -7500, -7500);

            var newPrefab = instance.GetComponent<T>();
            instance.SetActive(false);
            // This value will set to be true at PrefabCollection<T>.InitializePrefabImpl
            newPrefab.m_prefabInitialized = false;
            return newPrefab;
        }
    }
}
