using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pathfinding;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace DerekLiu.Scripts
{
    [Serializable]
    struct SandIDRange
    {
        public int minID;
        public int maxID;
    }
    
    public class SandManager : SingletonMono<SandManager>
    {
        [SerializeField]private SandIDRange sandIDRanges;
        public GameObject SandPrefab;
        public Transform minPos;
        public Transform maxPos;
        
        private HashSet<TreasureSandBase> sands = new HashSet<TreasureSandBase>();
        private AstarPath _aStarPath;
        private List<GraphNode> _walkableNodes = new List<GraphNode>();
        
        private AstarPath aStarPath
        {
            get
            {
                if (_aStarPath == null)
                {
                    _aStarPath = FindObjectOfType<AstarPath>();
                }
                return _aStarPath;
            }
        }
        private List<GraphNode> walkableNodes
        {
            get
            {
                if (_walkableNodes.Count == 0)
                {
                    aStarPath.data.GetNodes((node) =>
                    {
                        if (node.Walkable)
                        {
                            walkableNodes.Add(node);
                        }
                    });
                }
                return _walkableNodes;
            }
        }


        public int currentCount = 0;
        public int maxCount = 3;
        
        public void Update()
        {
            if (currentCount < maxCount)
            {
                SpawnTreasureSandInPlane();
            }
        }

        public void SpawnTreasureSand()
        {
            uint id = GetRandomSandID();
            Vector3 position = GetRandomPosition();
            
            var go = Game.instance.ResourcesLoader.LoadAsset(id,position+new Vector3(0,0.5f,0),Quaternion.identity) as GameObject; 
            _ = Game.instance.EntityManager.CreateAndAddEntity(go);

            var s = go.GetComponent<TreasureSandBase>();
            if (s != null)
            {
                sands.Add(s);
            }
        }

        public void SpawnTreasureSandInPlane()
        {
            Vector3 position = GetRandomPositionNoVR();
            position.y += 0.1f;
            var go = Instantiate(SandPrefab,position, Quaternion.identity);
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
        

        public async Task DestroyTreasureSand(TreasureSandBase sand)
        {
            if (sands.Contains(sand))
            {
                //TODO：删除
                await Game.instance.EntityManager.DeleteEntityAndAnchor(sand);
                sands.Remove(sand);
            }
        }

        private uint GetRandomSandID()
        {
            return (uint)Random.Range(sandIDRanges.minID,sandIDRanges.maxID);
        }

        private Vector3 GetRandomPosition()
        {
            int rand = Random.Range(0,walkableNodes.Count);
            return (Vector3)walkableNodes[rand].position;
        }
        
        private Vector3 GetRandomPositionNoVR()
        {
            Vector3 rand = (maxPos.position - minPos.position);
            rand.x = Random.Range(minPos.position.x, maxPos.position.x);
            rand.z = Random.Range(minPos.position.y, maxPos.position.z);
            
            return minPos.position + rand;
        }
    }
}