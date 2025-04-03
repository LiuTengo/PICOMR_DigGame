using System;
using PICOMR.Scripts.ResourcesLoader.Interfaces;
using PICOMR.Scripts.ResourcesLoader.ResourcesSO;
using UnityEngine;

namespace PICOMR.Scripts.ResourcesLoader
{
    /// <summary>
    /// 该类用于加载/生成游戏预制体。
    /// </summary>
    public class ResourcesLoader : MonoBehaviour
    {
        [SerializeField]private AssetSO assets;

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
        public GameObject LoadAsset(ulong id,Vector3 position,Quaternion rotation)
        {
            GameObject res;
            var prefab = LoadGameObjectByID(id);
            if (prefab != null)
            {
                res = Instantiate(prefab, position, rotation);
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
                Debug.LogError($"Failed to load asset {id}");
            }
            return null;
        }

        /// <summary>
        /// 根据anchor handle获取预制体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private GameObject LoadGameObjectByID(ulong id)
        {
            assets.prefabDictionary.TryGetValue(id, out GameObject res);
            return res;
        }
    }
}