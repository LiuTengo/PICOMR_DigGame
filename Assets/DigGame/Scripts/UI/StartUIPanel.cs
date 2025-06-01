using System.Collections;
using System.Collections.Generic;
using PICOMR.Scripts.ResourcesLoader;
using Test.Scripts.ResourcesLoader;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class StartUIPanel : MonoBehaviour
{
    public Button startBtn;
    public Button continueBtn;
    public Button quitBtn;

    private void OnEnable()
    {
        startBtn.GetComponent<XRSimpleInteractable>().lastSelectExited.AddListener(StartGame);
        continueBtn.GetComponent<XRSimpleInteractable>().lastSelectExited.AddListener(ContinueGame);
        quitBtn.GetComponent<XRSimpleInteractable>().lastSelectExited.AddListener(QuitGame);

        if (PersistentLoader.HasJsonFile())
        {
            continueBtn.interactable = true;
        }
        else
        {
            continueBtn.interactable = false;
        }
    }
    
    private void OnDisable()
    {
        startBtn.GetComponent<XRSimpleInteractable>().lastSelectExited.RemoveListener(StartGame);
        continueBtn.GetComponent<XRSimpleInteractable>().lastSelectExited.RemoveListener(ContinueGame);
        quitBtn.GetComponent<XRSimpleInteractable>().lastSelectExited.AddListener(QuitGame);
    }

    private async void StartGame(SelectExitEventArgs arg0)
    {
        PersistentLoader.DeleteJsonFile();
        var res = await Game.instance.EntityManager.LoadGameEntities();

        if (res)
        {
            //Load MainLand
            LoadMainLand();    
        }
        else
        {
            Debug.LogError($"Init Game Error");
        }
        
        Destroy(this.gameObject);
    }

    private async void ContinueGame(SelectExitEventArgs arg0)
    {
        var res = await Game.instance.EntityManager.LoadGameEntities();
        if (res)
        {
            Destroy(this.gameObject);    
        }
    }
    
    private void QuitGame(SelectExitEventArgs arg0)
    {
        Application.Quit();
    }

    private void LoadMainLand()
    {
        var floorList = SpatialAnchorManager.GetRoomAnchorByLabel(PxrSemanticLabel.Floor);
        if (floorList != null && floorList.Count > 0)
        {
            var trans = PXR_Manager.Instance.transform;
            Vector3 pos = trans.position;
            pos.y = floorList[0].Position.y;
            pos += trans.forward * 0.05f;
            
            var go = Game.instance.ResourcesLoader.LoadAsset
                (101,pos,Quaternion.identity);
            _ = Game.instance.EntityManager.CreateGameEntity(go);
        }
        else
        {
            var go = Game.instance.ResourcesLoader.LoadAsset
                (101,Vector3.zero,Quaternion.identity);
            _ = Game.instance.EntityManager.CreateGameEntity(go);
            Debug.LogError($"Init Spatial Tracking State Error");
        }
    }
}
