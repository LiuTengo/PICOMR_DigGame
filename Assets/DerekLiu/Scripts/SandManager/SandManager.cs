using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pathfinding;
using PICOMR.Scripts.ResourcesLoader;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace DerekLiu.Scripts
{

    public class SandManager : SingletonMono<SandManager>
    {
        public float minRadius=1.2f;
        public float maxRadius=2.5f;

        private HashSet<TreasureSandBase> sands = new HashSet<TreasureSandBase>();
        
        public int currentCount = 0;
        public int maxCount = 3;
        
        public void Update()
        {
            if (currentCount < maxCount)
            {
                SpawnTreasureSandInPlane();
            }
        }

        private void SpawnTreasureSandInPlane()
        {
            ulong id = GetRandomSandID();
            Vector3 position = GetRandomPositionAroundMainLand();
            position.y += 1.5f;
            
            var go = Game.instance.ResourcesLoader.LoadAsset(id,position,Quaternion.identity,null,ObjectType.Sand);
            
            var sand = go.GetComponent<TreasureSandBase>();
            if (sand != null)
            {
                sand.SetSandManager(this);
                currentCount++;
                sands.Add(sand);
            }
        }

        public void DestroyTreasureSandInPlane(TreasureSandBase treasureSand)
        {
            if (treasureSand != null)
            {
                sands.Remove(treasureSand);
                Destroy(treasureSand.gameObject);
                currentCount--;
            }
        }
        

        private ulong GetRandomSandID()
        {
            return Game.instance.ResourcesLoader.GetRandomObjectID(ObjectType.Sand);
        }
        
        private Vector3 GetRandomPositionAroundMainLand()
        {
            float theta = Random.Range(0f, Mathf.PI * 2f);
            Vector2 randomCircle = new Vector2(Mathf.Cos(theta),Mathf.Sin(theta));
            float radius = Random.Range(minRadius,maxRadius);
            randomCircle *= radius;
            
            return transform.position + new Vector3(randomCircle.x,0,randomCircle.y);
        }
    }
}