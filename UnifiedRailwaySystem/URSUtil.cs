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
            public static ItemClass trainTrackItemClass { get; private set; }
            public static ItemClass metroTrackItemClass { get; private set; }
            public static ItemClass tramTrackItemClass { get; private set; }
            #endregion

            #region ConnectGroup
            public static NetInfo.ConnectGroup trainConnectGroup { get; private set; }
            public static NetInfo.ConnectGroup tramConnectGroup { get; private set; }
            #endregion

            /// <summary>
            /// Initialize the class.
            /// </summary>
            public static void Initialize()
            {
                trainTrackItemClass = PrefabCollection<NetInfo>.FindLoaded("Train Track").m_class;
                metroTrackItemClass = PrefabCollection<NetInfo>.FindLoaded("Metro Track").m_class;
                tramTrackItemClass = PrefabCollection<NetInfo>.FindLoaded("Basic Road").m_class;

                trainConnectGroup = NetInfo.ConnectGroup.DoubleTrain
                    | NetInfo.ConnectGroup.SingleTrain
                    | NetInfo.ConnectGroup.TrainStation;
                tramConnectGroup = NetInfo.ConnectGroup.CenterTram
                    | NetInfo.ConnectGroup.NarrowTram
                    | NetInfo.ConnectGroup.SingleTram
                    | NetInfo.ConnectGroup.WideTram;
            }
        }

        public interface BackupInfo
        {
            void BackupInfo<T>(T prefabInfo) where T : PrefabInfo;
            void Restore();
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
