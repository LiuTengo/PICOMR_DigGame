using System;
using System.Collections.Generic;
using UnityEngine;

namespace PICOMR.Scripts.ResourcesLoader.ResourcesSO
{
    [Serializable]
    public struct ID2Prefab{
        public ulong id;
        public GameObject prefab; 
    }
    
    [CreateAssetMenu(fileName = "AssetSO", menuName = "ResourcesLoader/AssetSO")]
    public class AssetSO : ScriptableObject
    {
        [SerializeField]private List<ID2Prefab> prefabs;

        public Dictionary<ulong, GameObject> prefabDictionary = new Dictionary<ulong, GameObject>();
        public void InitAssetsData()
        {
            foreach (var p in prefabs)
            {
                prefabDictionary.Add(p.id, p.prefab);
            }
        }
    }
}