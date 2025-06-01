using System;
using System.Collections.Generic;
using PICOMR.Scripts.ResourcesLoader;
using PICOMR.Scripts.ResourcesLoader.ResourcesSO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DerekLiu.Scripts
{
    public class TreasureManager : SingletonMono<TreasureManager>
    {
        private List<ulong> generatedTreasures = new List<ulong>();
        private List<ulong> leftTreasures = new List<ulong>();

        public GameObject SpawnTreasure(ulong index,Vector3 position, Quaternion rotation)
        {
            var go = Game.instance.ResourcesLoader.LoadAsset(index,position,rotation,null,ObjectType.Treasure);
            var t = go.GetComponent<TreasureBase>();
             if (t != null)
             {
                 
             }
             else
             {
                 Destroy(go);
                 return null;
             }
            
            return null;
        }

        public ulong GetRandomTreasurePrefab()
        {
             if (leftTreasures.Count == 0) {
                 ResetTreasuresPool();
             }

             ulong index = leftTreasures[Random.Range(0, leftTreasures.Count)];
             leftTreasures.Remove(index);
             generatedTreasures.Add(index);
             return index;
        }

        private void ResetTreasuresPool()
        {
            leftTreasures.Clear();
            generatedTreasures.Clear();
            
            var dic = Game.instance.ResourcesLoader.assets.treasurePrefabs;
            foreach (var prefab in dic)
            {
                leftTreasures.Add(prefab.id);
            }
        }
    }
}