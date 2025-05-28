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
    public GameObject testUIPrefab;
    
    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        InitManager();
        await InitGameAsync();

        //Instantiate(ResourcesLoader.assets.prefabDictionary[4],new Vector3(0,0,5.0f),Quaternion.identity);

        PXRInputControllerManager.secondaryBtnRight.action.performed += TestInput;
    }

    private void InitManager()
    {
        EntityManager.ResourcesLoader = ResourcesLoader;
    }
    
    private  void TestInput(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Create Prefab3");
    }
    
    private async Task InitGameAsync()
    {
        //开启视频透视
        PXR_Manager.EnableVideoSeeThrough = true;
        
        //开启空间锚点
        await StartSceneCaptureProvider();
        await StartSpatialAnchorProvider();
        
        var result = await PXR_MixedReality.StartSceneCaptureAsync();
        
        var spatialState = await CheckSpatialTrackingStateAsync();
        if (spatialState && result == PxrResult.SUCCESS)
        {
            await EntityManager.LoadRoomEntities();
            Debug.Log("Load Room Entities Finished");
        }
        else
        {
            Debug.LogError($"Init Spatial Tracking State Error");
        }
        await EntityManager.LoadGameEntities();
    }

    private async UniTask<bool> CheckSpatialTrackingStateAsync()
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
    
}
