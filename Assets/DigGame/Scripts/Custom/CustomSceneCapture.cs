using System;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

namespace Test.PXR.Custom
{
    public struct FloorAnchorLocation
    {
        public ulong anchorHandel;
        public Vector3 position;
        public Quaternion rotation;

        public FloorAnchorLocation(ulong handel,Vector3 position, Quaternion rotation)
        {
            this.anchorHandel = handel;
            this.position = position;
            this.rotation = rotation;
        }
    }
    
    public class CustomSceneCapture : MonoBehaviour
    {
        private const string TAG = "[CustomSceneCapture]";
        
        private List<Guid> sceneAnchorList;
        private List<FloorAnchorLocation> sceneFloorAnchorList;

        public GameObject FloorAnchorPrefab;
        
        void Start()
        {
            PXR_Manager.EnableVideoSeeThrough = true;
            sceneAnchorList = new List<Guid>();
            sceneFloorAnchorList = new List<FloorAnchorLocation>();
            StartSceneCaptureProvider();
        }
        
        void OnEnable()
        {
            PXR_Manager.SceneAnchorDataUpdated += SceneAnchorDataUpdated;
        }

        void OnDisable()
        {
            PXR_Manager.SceneAnchorDataUpdated -= SceneAnchorDataUpdated;
        }
        
        public List<FloorAnchorLocation> GetSceneFloorAnchorList => sceneFloorAnchorList;
        
        private void SceneAnchorDataUpdated()
        {
            LoadSceneData();
        }
        
        private async void StartSceneCaptureProvider()
        {
            var result0 = await PXR_MixedReality.StartSenseDataProvider(PxrSenseDataProviderType.SceneCapture);
            if (result0 == PxrResult.SUCCESS)
            {
                LoadSceneData();
            }
            else
            {
                PLog.e(TAG, "SceneCaptureProvider start fail", false);
            }

        }
        
        private async void LoadSceneData()
        {
            var result = await PXR_MixedReality.QuerySceneAnchorAsync(default);

            if (result.result == PxrResult.SUCCESS)
            {
                if (result.anchorDictionary.Count > 0)
                {
                    foreach (var item in result.anchorDictionary)
                    {
                        if (!sceneAnchorList.Contains(item.Value))
                        {
                            var result1 = PXR_MixedReality.GetSceneSemanticLabel(item.Key, out var label);
                            if (result1 == PxrResult.SUCCESS)
                            {
                                AddFloorAnchor(item.Key, label);
                            }
                            sceneAnchorList.Add(item.Value);
                        }
                    }
                }
                else
                {
                    var result2 = await PXR_MixedReality.StartSceneCaptureAsync(default);
                    if (result2 == PxrResult.SUCCESS)
                    {
                        LoadSceneData();
                    }
                    PLog.e(TAG, "Query scene anchor count is 0", false);
                }
            }
            else
            {
                PLog.e(TAG, "Query scene anchor fail" + result.result, false);
            }
        }

        private void AddFloorAnchor(ulong anchorHandel,PxrSemanticLabel label)
        {
            switch (label)
            {
                case PxrSemanticLabel.Table:
                    var result = PXR_MixedReality.LocateAnchor(anchorHandel, out Vector3 location,out Quaternion rotation);
                    var anchorLocation = new FloorAnchorLocation(anchorHandel,location, rotation);                    
                    sceneFloorAnchorList.Add(anchorLocation);
                    GameObject floorAnchor = Instantiate(FloorAnchorPrefab, location, rotation);
                    
                    // var floorRes = PXR_MixedReality.GetScenePolygonData(anchorHandel, out var floorVert);
                    // if (floorRes == PxrResult.SUCCESS)
                    // {
                    //     foreach (var v in floorVert)
                    //     {
                    //         Vector3 pos = new Vector3(v.x,v.y,anchorLocation.position.z);
                    //         GameObject go = Instantiate(FloorAnchorPrefab, pos, Quaternion.identity);
                    //     }
                    // }
                    
                    break;
                default:
                    break;
            }
        }
        
    }
}