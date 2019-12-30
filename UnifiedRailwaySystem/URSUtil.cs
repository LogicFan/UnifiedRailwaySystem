using ICities;
using UnityEngine;

namespace UnifiedRailwaySystem
{
    public static class URSPrefabCloner
    {
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
