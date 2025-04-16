using System;
using System.Collections.Generic;
using PICOMR.Scripts.ResourcesLoader.ResourcesSO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DerekLiu.Scripts
{
    public class TreasureManager : SingletonMono<TreasureManager>
    {
        public AssetSO assetSO;
        
        private List<TreasureBase> foundTreasures = new List<TreasureBase>();
        private List<int> randomIndex = new List<int>();
        private List<int> generatedIndex = new List<int>();
        private void Start()
        {
            assetSO.InitAssetsData();

            foreach (var prefabPair in assetSO.prefabDictionary)
            {
                randomIndex.Add((int)prefabPair.Key);
            }
        }

        public GameObject SpawnTreasure(Vector3 position, Quaternion rotation)
        {
            if (randomIndex.Count == 0)
            {
                return null;
            }
            
            int index = randomIndex[Random.Range(0, randomIndex.Count)];

            var prefab = assetSO.prefabDictionary[(ulong)index];
            
            var go = Instantiate(prefab, position, rotation);
            var t = go.GetComponent<TreasureBase>();
            if (t != null)
            {
                t.id = index;
                foundTreasures.Add(t);
            }
            else
            {
                Destroy(go);
                return null;
            }
            
            randomIndex.Remove(index);
            generatedIndex.Add(index);
            
            return go;
        }

        public void DestroyTreasure(TreasureBase treasure)
        {
            randomIndex.Add(treasure.id);
            generatedIndex.Remove(treasure.id);
            foundTreasures.Remove(treasure);
            
            Destroy(treasure.gameObject);
        }
    }
}