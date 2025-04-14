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
    }
}