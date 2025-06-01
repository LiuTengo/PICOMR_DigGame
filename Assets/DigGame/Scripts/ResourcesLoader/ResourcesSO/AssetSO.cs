using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField]public List<ID2Prefab> generalPrefabs;//1__
        [SerializeField]public List<ID2Prefab> sandPrefabs;//2__
        [SerializeField]public List<ID2Prefab> treasurePrefabs;//3__
        [SerializeField]public List<ID2Prefab> animalPrefabs;//4__
        
        public Dictionary<ulong, GameObject> PrefabDictionary { get; private set; }
        public Dictionary<ulong, GameObject> SandDictionary { get; private set; }
        public Dictionary<ulong, GameObject> TreasureDictionary { get; private set; }
        public Dictionary<ulong, GameObject> AnimalDictionary { get; private set; }

        public void InitAssetsData()
        {
            PrefabDictionary = new Dictionary<ulong, GameObject>();
            SandDictionary = new Dictionary<ulong, GameObject>();
            TreasureDictionary = new Dictionary<ulong, GameObject>();
            AnimalDictionary = new Dictionary<ulong, GameObject>();
            foreach (var p in generalPrefabs)
            {
                PrefabDictionary.Add(p.id, p.prefab);
            }
            foreach (var sP in sandPrefabs)
            {
                SandDictionary.Add(sP.id, sP.prefab);
                PrefabDictionary.Add(sP.id, sP.prefab);
            }
            foreach (var tP in treasurePrefabs)
            {
                TreasureDictionary.Add(tP.id, tP.prefab);
                PrefabDictionary.Add(tP.id, tP.prefab);
            }
            foreach (var aP in animalPrefabs)
            {
                AnimalDictionary.Add(aP.id, aP.prefab);
                PrefabDictionary.Add(aP.id, aP.prefab);
            }
        }
    }
}