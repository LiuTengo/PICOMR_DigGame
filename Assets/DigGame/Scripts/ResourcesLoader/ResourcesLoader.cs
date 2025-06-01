using PICOMR.Scripts.ResourcesLoader.Interfaces;
using PICOMR.Scripts.ResourcesLoader.ResourcesSO;
using UnityEngine;

namespace PICOMR.Scripts.ResourcesLoader
{
    public enum ObjectType
    {
        General,
        Sand,
        Treasure,
        Animal
    }
    
    /// <summary>
    /// 该类用于加载/生成游戏预制体。
    /// </summary>
    public class ResourcesLoader : MonoBehaviour
    {
        [SerializeField]public AssetSO assets;

        public void Awake()
        {
            assets.InitAssetsData();
        }

        /// <summary>
        /// 根据anchor handle在指定位置生成物体
        /// </summary>
        /// <param name="id">Anchor句柄</param>
        /// <param name="position">位置</param>
        /// <param name="rotation">旋转</param>
        /// <returns></returns>
        public GameObject LoadAsset(ulong id,Vector3 position,Quaternion rotation,Transform parent = null,ObjectType objectType=ObjectType.General)
        {
            var prefab = LoadGameObjectByID(objectType,id);
            if (prefab != null)
            {
                GameObject res = Instantiate(prefab, position, rotation,parent);
                var item = res.GetComponent<IItem>();
                if (item != null)
                {
                    item.EntityID = id;
                    //TODO: Do something else
                }
                return res;
            }
            else
            {
                Debug.LogWarning($"Failed to load asset {id}");
            }
            return null;
        }
        
        /// <summary>
        /// 根据anchor handle获取预制体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private GameObject LoadGameObjectByID(ObjectType objType,ulong id)
        {
            GameObject res = null;
            switch (objType)
            {
                case ObjectType.General:
                    assets.PrefabDictionary.TryGetValue(id, out res);
                    break;
                case ObjectType.Sand:
                    assets.SandDictionary.TryGetValue(id, out res);
                    break;
                case ObjectType.Treasure:
                    assets.TreasureDictionary.TryGetValue(id, out res);
                    break;
                case ObjectType.Animal:
                    assets.AnimalDictionary.TryGetValue(id, out res);
                    break;
            }
            return res;
        }

        public ulong GetRandomObjectID(ObjectType objType)
        {
            switch (objType)
            {
                case ObjectType.General:
                    return assets.generalPrefabs[Random.Range(0,assets.generalPrefabs.Count)].id;
                    break;
                case ObjectType.Sand:
                    return assets.sandPrefabs[Random.Range(0,assets.sandPrefabs.Count)].id; 
                    break;
                case ObjectType.Treasure:
                    return assets.treasurePrefabs[Random.Range(0, assets.treasurePrefabs.Count)].id;
                    break;
                case ObjectType.Animal:
                    return assets.animalPrefabs[Random.Range(0,assets.animalPrefabs.Count)].id;
                    break;
            }
            return 0;
        }
    }
}