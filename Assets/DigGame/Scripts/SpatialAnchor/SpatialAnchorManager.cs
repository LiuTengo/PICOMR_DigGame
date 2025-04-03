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
    private static List<AnchorData> anchorDataList = new List<AnchorData>();
    private static bool isLoadingRoomData = false;
    public static async UniTask<List<AnchorData>> LoadSpatialAnchor()
    {
        isLoadingRoomData = true;
        anchorDataList.Clear();
        var result = await PXR_MixedReality.QuerySpatialAnchorAsync();
        if (result.result == PxrResult.SUCCESS)
        {
            if (result.anchorHandleList.Count > 0)
            {
                foreach (var anchor in result.anchorHandleList)
                {
                    PXR_MixedReality.GetAnchorUuid(anchor, out Guid anchorUuid);
                    AnchorData anchorData = new AnchorData(anchor,anchorUuid);
                    anchorDataList.Add(anchorData);
                }
            }
        }
        isLoadingRoomData = false;
        return anchorDataList;
    }
    
    public static async UniTask<AnchorData> CreateSpatialAnchor(Transform transform)
    {
        var result = await PXR_MixedReality.CreateSpatialAnchorAsync(transform.position, transform.rotation);
        if (result.result == PxrResult.SUCCESS)
        {
            var anchorData = new AnchorData(result.anchorHandle,result.uuid);
            anchorDataList.Add(anchorData);
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

    public static async UniTask DeleteAnchor(AnchorData anchorData)
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

    public static async UniTask DeleteAllAnchors()
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

    public static async UniTask DeleteAnchorById(Guid anchorUuid)
    {
        var anchorData = anchorDataList.FirstOrDefault(a => a.Uid == anchorUuid);
        if (anchorData != null)
        {
            await DeleteAnchor(anchorData);
            anchorDataList.Remove(anchorData);
            Debug.Log($"Anchor with UUID {anchorUuid} deleted.");
        }
        else
        {
            Debug.LogError($"Anchor with UUID {anchorUuid} not found.");
        }
    }
}
