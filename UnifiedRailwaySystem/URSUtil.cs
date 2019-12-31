using ICities;
using UnityEngine;

// currently not used

namespace UnifiedRailwaySystem
{
    public static class Util
    {
        #region dataItemClass
        private static ItemClass _trainItemClass;
        private static ItemClass _metroItemClass;
        private static ItemClass _roadItemClass;
        public static ItemClass trainItemClass => _trainItemClass;
        public static ItemClass metroItemClass => _metroItemClass;
        public static ItemClass roadItemClass => _roadItemClass;
        #endregion

        public static void Initialize()
        {
            // begin initialize data region
            _trainItemClass = PrefabCollection<NetInfo>.FindLoaded("Train Track").m_class;
            _metroItemClass = PrefabCollection<NetInfo>.FindLoaded("Metro Track").m_class;
            _roadItemClass = PrefabCollection<NetInfo>.FindLoaded("Basic Road").m_class;
            // end   initialize data region
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
