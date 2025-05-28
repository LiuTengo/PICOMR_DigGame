using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using PICOMR.Scripts.ResourcesLoader;
using PXR_Audio.Spatializer;
using Unity.XR.PXR;
using UnityEngine;

public static  class SpatialAnchorManager
{
    //Load Room Anchor Data
    private static bool isLoadingRoomData = false;
    private static List<AnchorData> roomData = new List<AnchorData>();
    
    //Load Game Anchor Data
    private static bool isLoadingGameData = false;
    private static List<AnchorData> gameAnchorDataList = new List<AnchorData>();

    #region Game Anchor
    public static async UniTask<List<AnchorData>> LoadSpatialAnchor()
    {
        isLoadingGameData = true;
        gameAnchorDataList.Clear();
        var result = await PXR_MixedReality.QuerySpatialAnchorAsync();
        if (result.result == PxrResult.SUCCESS)
        {
            if (result.anchorHandleList.Count > 0)
            {
                foreach (var anchor in result.anchorHandleList)
                {
                    PXR_MixedReality.GetAnchorUuid(anchor, out Guid anchorUuid);
                    AnchorData anchorData = new AnchorData(anchor,anchorUuid);
                    gameAnchorDataList.Add(anchorData);
                }
            }
        }
        isLoadingGameData = false;
        return gameAnchorDataList;
    }
    
    public static async UniTask<AnchorData> CreateSpatialAnchor(Transform transform)
    {
        var result = await PXR_MixedReality.CreateSpatialAnchorAsync(transform.position, transform.rotation);
        if (result.result == PxrResult.SUCCESS)
        {
            var anchorData = new AnchorData(result.anchorHandle,result.uuid);
            gameAnchorDataList.Add(anchorData);
            return anchorData;
        }
        return null;
    }

    public static async UniTask SaveGameAnchorToLocal(List<AnchorData> anchorDatas)
    {
        Debug.Log( $"Start SaveGameAnchorsToLocal "+ anchorDatas.Count);
        if (anchorDatas.Count <= 0)
            return;
        ulong[] handleList = anchorDatas.Select(x => x.Handle).ToArray();
        foreach (var anchor in handleList)
        {
            Debug.Log( $"Start SaveGameAnchorsToLocal anchor "+ anchor);
            await PXR_MixedReality.PersistSpatialAnchorAsync(anchor);
        }
    }

    public static async UniTask ClearGameAnchorsInLocal(List<AnchorData> anchorDatas)
    {
        if (anchorDatas.Count <= 0)
            return;
        ulong[] handleList = anchorDatas.Select(x => x.Handle).ToArray();
        foreach (var anchor in handleList)
        {
            Debug.Log( $"Start ClearGameAnchorsToLocal anchor "+ anchor);

            await PXR_MixedReality.UnPersistSpatialAnchorAsync(anchor);
        }
    }

    public static async UniTask DeleteGameAnchor(AnchorData anchorData)
    {
        var result = PXR_MixedReality.DestroyAnchor(anchorData.Handle);
        if (result == PxrResult.SUCCESS)
        {
            Debug.Log("PXR_MRSample Destroy spatial anchor succeed with anchorHandle " + anchorData.Handle);
        }
        else
        {
            Debug.Log("PXR_MRSample Destroy spatial anchor failed with result:" + result);
        }
        await PXR_MixedReality.UnPersistSpatialAnchorAsync(anchorData.Handle);
        Debug.Log($"Delete Anchor, uuid: {anchorData.Uid}, handle: {anchorData.Handle}");
    }

    public static async UniTask DeleteAllGameAnchors()
    {
        var result = await PXR_MixedReality.QuerySpatialAnchorAsync();
        Debug.unityLogger.Log($"LoadSpatialAnchorAsync: {result.result}");
        if (result.result == PxrResult.SUCCESS)
        {
            ulong[] handleList = result.anchorHandleList.ToArray();
            foreach (var anchor in handleList)
            {
                Debug.Log( $"Start DeleteAllAnchors anchor "+ anchor);

                await PXR_MixedReality.UnPersistSpatialAnchorAsync(anchor);
            }
        }
    }

    public static async UniTask DeleteGameAnchorById(Guid anchorUuid)
    {
        var anchorData = gameAnchorDataList.FirstOrDefault(a => a.Uid == anchorUuid);
        if (anchorData != null)
        {
            await DeleteGameAnchor(anchorData);
            gameAnchorDataList.Remove(anchorData);
            Debug.Log($"Anchor with UUID {anchorUuid} deleted.");
        }
        else
        {
            Debug.LogError($"Anchor with UUID {anchorUuid} not found.");
        }
    }

    #endregion

    #region Room Anchor
    /// <summary>
    /// 加载房间锚点
    /// </summary>
    /// <returns></returns>
    public async static UniTask<List<AnchorData>> LoadRoomAnchors()
    {
        roomData.Clear();
        isLoadingRoomData = true;
        var result = await PXR_MixedReality.QuerySceneAnchorAsync(default);
        Debug.Log($"LoadRoomAnchors: {result.result}");
        if (result.result == PxrResult.SUCCESS)
        {
            if (result.anchorDictionary.Count > 0)
            {
                foreach (var anchor in result.anchorDictionary)
                {
                    AnchorData anchorData = new AnchorData(anchor.Key,anchor.Value);
                    roomData.Add(anchorData);
                }
            }  
        }
        isLoadingGameData = false;
        return roomData;
    }
    
    #endregion
}
