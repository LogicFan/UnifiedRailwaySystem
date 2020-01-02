using ICities;
using UnityEngine;

namespace UnifiedRailwaySystem
{
    /// <summary>
    /// A utility class for the mod.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// A class that contains information will be later used in the mod.
        /// </summary>
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

            /// <summary>
            /// Initialize the class.
            /// </summary>
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

        /// <summary>
        /// Initialize the class.
        /// </summary>
        public static void Initialize()
        {
            Debug.Log("Util.Initialize");
            Cache.Initialize();
        }

        /// <summary>
        /// A clone method for <c>PrefabInfo</c>
        /// </summary>
        /// <param name="originalPrefab"> original <c>PrefabInfo</c> will be cloned. </param>
        /// <typeparam name="T"> the type of <c>originalPrefab</c>. </typeparam>
        /// <returns> a clone of <c>originalPrefab</c>. </returns>
        public static T Clone<T>(T originalPrefab) where T : PrefabInfo
        {
            var instance = Object.Instantiate(originalPrefab.gameObject);
            instance.transform.parent = originalPrefab.gameObject.transform;

            var newPrefab = instance.GetComponent<T>();
            instance.SetActive(false);

            // This value will set to be true at PrefabCollection<T>.InitializePrefabImpl
            newPrefab.m_prefabInitialized = false;
            
            return newPrefab;
        }
    }
}
