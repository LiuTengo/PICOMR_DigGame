using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using PICOMR.Scripts.ResourcesLoader;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game : SingletonMono<Game>
{
    public ResourcesLoader ResourcesLoader;
    public EntityManager EntityManager;
    public PXRInputControllerManager PXRInputControllerManager;
    
    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        InitManager();
        await InitGameAsync();
        
        var playerTransform = PXR_Manager.Instance.transform;
        Vector3 pos = playerTransform.position;
        pos.y -= 0.5f;
        pos += playerTransform.right * 50.0f;
        
        ResourcesLoader.LoadAsset(102,pos,Quaternion.identity);
        //Instantiate(ResourcesLoader.assets.prefabDictionary[4],new Vector3(0,0,5.0f),Quaternion.identity);
        //PXRInputControllerManager.secondaryBtnRight.action.performed += TestInput;
    }

    private void InitManager()
    {
        EntityManager.ResourcesLoader = ResourcesLoader;
    }
    
    
    private async Task InitGameAsync()
    {
        //开启视频透视
        PXR_Manager.EnableVideoSeeThrough = true;
        
        //开启空间锚点
        await StartSceneCaptureProvider();
        await StartSpatialAnchorProvider();
        
        var result = await PXR_MixedReality.StartSceneCaptureAsync();
        //Load RoomEntity
        var spatialState = await Game.instance.CheckSpatialTrackingStateAsync();
        if (spatialState)
        {
            await Game.instance.EntityManager.LoadRoomAnchors();
            Debug.Log("Load Room Entities Finished");
        }
        else
        {
            Debug.LogError($"Init Spatial Tracking State Error");
        }
    }

    public async UniTask<bool> CheckSpatialTrackingStateAsync()
    {
        await UniTask.CompletedTask;
        return true;
    }
    
    private async UniTask StartSceneCaptureProvider()
    {
        var result0 = await PXR_MixedReality.StartSenseDataProvider(PxrSenseDataProviderType.SceneCapture);
        Debug.Log($"StartSceneCaptureProvider:SceneCapture: {result0}");
    }
    
    private async UniTask StartSpatialAnchorProvider()
    {
        var result0 = await PXR_MixedReality.StartSenseDataProvider(PxrSenseDataProviderType.SpatialAnchor);
        Debug.Log($"StartSenseDataProvider: {result0}");
    }

    private void OnApplicationQuit()
    {
        _ =EntityManager.SaveGameEntities();
    }
}
