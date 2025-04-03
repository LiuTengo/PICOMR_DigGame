using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using PICOMR.Scripts.ResourcesLoader;
using Unity.XR.PXR;
using UnityEngine;

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
        

       var go = Game.instance.ResourcesLoader.LoadAsset(4,new Vector3(0,0,100.0f),Quaternion.identity) as GameObject; 
       await Game.instance.EntityManager.CreateAndAddEntity(go);
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
        await StartSpatialAnchorProvider();
        await EntityManager.LoadGameEntities();
    }

    private async UniTask StartSpatialAnchorProvider()
    {
        var result0 = await PXR_MixedReality.StartSenseDataProvider(PxrSenseDataProviderType.SpatialAnchor);
        Debug.Log($"StartSenseDataProvider: {result0}");
    }
}
