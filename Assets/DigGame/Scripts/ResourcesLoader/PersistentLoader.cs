using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using PICOMR.Scripts.ResourcesLoader;
using UnityEngine;

namespace Test.Scripts.ResourcesLoader
{
    /// <summary>
    /// Json记录的类
    /// Guid和实体ID的映射
    /// </summary>
    public class PersistentData
    {
        [JsonProperty("AnchorData")]
        public UID2EntityID[] AnchorData;
    }
    
    /// <summary>
    /// 该类用于读写持久化的数据。
    /// 通过Json记录Guid对应的实体ID（使用了Newtonsoft.Json）
    /// Guid可以用来查找空间锚点的句柄（anchor handle）
    /// 通过PICO的API可以输入Guid获取空间锚点的数据（位置、选择等）
    /// </summary>
    public static class PersistentLoader
    {
        private static readonly string dataPath = Path.Combine(Application.persistentDataPath, "DigGameData01.json");
        public static Dictionary<Guid,UID2EntityID> Uid2EntityIds = new Dictionary<Guid, UID2EntityID>();

        public static bool TryGetEntityInfo(Guid guid,out UID2EntityID entityInfo)
        {
            return Uid2EntityIds.TryGetValue(guid, out entityInfo);
        }
        
        public static async UniTask LoadData()
        {
            if (File.Exists(dataPath))
            {
                Debug.Log("Start loading data ......");
                Uid2EntityIds.Clear();
                var dataJson = await File.ReadAllTextAsync(dataPath);
                var persistentData = JsonConvert.DeserializeObject<PersistentData>(dataJson);
                if (persistentData != null)
                {
                    foreach (var uid2EntityID in persistentData.AnchorData)
                    {
                        Uid2EntityIds.Add(uid2EntityID.UID, uid2EntityID);
                    }
                    
                    Debug.Log("Loading data finished.");
                }
                else
                {
                    Debug.Log("Loading data failed, type mismatch.");
                }
            }
            else
            {
                Debug.Log($"{dataPath}, No such file in directory.");
            }
        }

        public static void StageAllEntityData(List<UID2EntityID> datas)
        {
            Uid2EntityIds.Clear();
            foreach (var uid2ID in datas)
            {
                Uid2EntityIds.Add(uid2ID.UID, uid2ID);
            }
        }
        
        public static async UniTask SaveData()
        {
            Debug.Log("Start saving data ......");
            var persistentData = new PersistentData()
            {
                AnchorData = Uid2EntityIds.Select(x=>x.Value).ToArray(),
            };
            
            string json = JsonConvert.SerializeObject(persistentData);
            await File.WriteAllTextAsync(dataPath, json);
            Debug.Log("Data saved successfully.");
        }

        public static UniTask DeleteAllData()
        {
            Debug.Log("Start deleting data ......");
            if (File.Exists(dataPath))
            {
                File.Delete(dataPath);
            }
            Debug.Log("Data deleted successfully.");
            return UniTask.CompletedTask;
        }
        
        public static void ClearAllEntityData()
        {
            Uid2EntityIds.Clear();
        }
    }
}
