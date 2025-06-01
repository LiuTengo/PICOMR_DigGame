using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using PICOMR.Scripts.ResourcesLoader.Interfaces;
using PICOMR.Scripts.ResourcesLoader.ResourcesSO;
using Test.Scripts.ResourcesLoader;
using Unity.XR.PXR;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PICOMR.Scripts.ResourcesLoader
{
    public class UID2EntityID
    {
        //空间锚点ID
        public Guid UID { get; set; }
        //物体ID
        public ulong EntityID { get; set; }
    }
    
    public class EntityManager : MonoBehaviour,IEntityManager
    {
        private bool isLeadingGameEntity = false;
        private List<IEntity> gameEntities = new();
        private bool _needUpdateRoomEntities = false;
        private ResourcesLoader resourcesLoader;
        public ResourcesLoader ResourcesLoader
        {
            set => resourcesLoader = value;
        }
        
        #region Game Anchor
        /// <summary>
        /// 初次/再次进入游戏时加载游戏实体
        /// </summary>
        public async UniTask<bool> LoadGameEntities()
        {
            var anchors = await SpatialAnchorManager.LoadSpatialAnchor();
            await PersistentLoader.LoadData();
            gameEntities.Clear();
            foreach (var anchor in anchors)
            {
                if (!PersistentLoader.TryGetEntityInfo(anchor.Uid,out var entityInfo))
                {
                    continue;
                }
                ulong entityID = entityInfo.EntityID;
                GameObject obj = resourcesLoader.LoadAsset
                    (entityID,anchor.Position,anchor.Rotation);
                IEntity entity = new Entity()
                {
                    AnchorData = anchor,
                    GameObject = obj,
                };
                gameEntities.Add(entity);
            }
            return true;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private async UniTask<IEntity> AddGameEntity(GameObject go)
        {
            Transform transform = go.transform;
            var anchorData = await SpatialAnchorManager.CreateSpatialAnchor(transform);
            if (anchorData != null)
            {
                Entity newEntity = new Entity
                {
                    GameObject = go,
                    AnchorData = anchorData
                };
                return newEntity;
            }
            else
            {
                return null;   
            }
        }
        
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="go">已实例化的游戏物体</param>
        /// <returns></returns>
        public async UniTask<IEntity> CreateGameEntity(GameObject go)
        {
            var entity = await AddGameEntity(go);
            if (entity != null)
            {
                gameEntities.Add(entity);
                return entity;
            }
            return null;
        }

        public async UniTask PersistEntity(IEntity entity)
        {
            if (!gameEntities.Contains(entity))
            {
                gameEntities.Add(entity);
            }
            await PXR_MixedReality.PersistSpatialAnchorAsync(entity.AnchorData.Handle);
        }
        
        public async UniTask UnPersistEntity(IEntity entity)
        {
            if (gameEntities.Contains(entity))
            {
                gameEntities.Remove(entity);    
            }
            await PXR_MixedReality.UnPersistSpatialAnchorAsync(entity.AnchorData.Handle);
        }

        /// <summary>
        /// 保存实体ID和空间锚点ID
        /// </summary>
        /// <returns></returns>
        public async UniTask SaveGameEntities()
        {
            List<UID2EntityID> dataList = new();
            foreach (var gameEntity in gameEntities)
            {
                var item = gameEntity.GameObject.GetComponent<IItem>();
                
                var data = new UID2EntityID
                {
                    UID = gameEntity.AnchorData.Uid,
                    EntityID = item.EntityID
                };
                dataList.Add(data);
            }
            //更新持久化Json数据
            PersistentLoader.StageAllEntityData(dataList);
            
            //TODO: Persistent Anchor保存AnchorData
            Debug.Log($"Start Save Game Anchors "+ dataList.Count);
            //保存空间锚点
            await SpatialAnchorManager.SaveGameAnchorToLocal(gameEntities.Select(x => x.AnchorData).ToList());
            Debug.Log( $"Finished Save Game Anchors");
            _ = PersistentLoader.SaveData();
        }

        public async UniTask ClearGameEntities()
        {
            foreach (var obj in gameEntities)
            {
                Object.Destroy(obj.GameObject);
            }
            gameEntities.Clear();
            //PXR_Manager.SceneAnchorDataUpdated -= DoSceneAnchorDataUpdated;
        }

        //待测试的方法
        public async UniTask DeleteEntityByAnchorUuid(Guid anchorUuid)
        {
            var entity = gameEntities.FirstOrDefault(e => e.AnchorData.Uid == anchorUuid);
            if (entity != null)
            {
                gameEntities.Remove(entity);
                Object.Destroy(entity.GameObject);
                await SpatialAnchorManager.DeleteGameAnchorById(anchorUuid);
                Debug.Log($"Entity and anchor with UUID {anchorUuid} deleted.");
            }
            else
            {
                Debug.LogError($"Entity with anchor UUID {anchorUuid} not found.");
            }
        }
        //待测试的方法
        public async UniTask DeleteEntityByEntityRef(IEntity entity)
        {
            if (gameEntities.Contains(entity))
            {
                gameEntities.Remove(entity);
                await SpatialAnchorManager.DeleteGameAnchorById(entity.AnchorData.Uid);
                Object.Destroy(entity.GameObject);
                Debug.Log($"Entity and anchor with UUID {entity.AnchorData.Uid} deleted.");
            }
            else
            {
                Debug.LogError($"Entity not found in gameEntities list.");
            }
        }
        #endregion 
        
        #region Room Anchor

        public async UniTask LoadRoomAnchors()
        {
            //PXR_Manager.SceneAnchorDataUpdated += DoSceneAnchorDataUpdated;
            var anchors = await SpatialAnchorManager.LoadRoomAnchors();
        }
        
        
        #endregion
    }
    
}