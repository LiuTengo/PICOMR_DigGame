using Test.Scripts.ResourcesLoader;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class UITest : MonoBehaviour
{
    public Button saveAnchorBtn;
    public Button clearAnchorBtn;
    public Button createPrefab1Btn;
    public Button createPrefab2Btn;
    public Button createPrefab3Btn;
    public Button deleteEntityBtn;

    private void OnEnable()
    {
        saveAnchorBtn.GetComponent<XRSimpleInteractable>().lastSelectExited.AddListener(SaveAnchor);
        clearAnchorBtn.GetComponent<XRSimpleInteractable>().lastSelectExited.AddListener(ClearAnchor);
        createPrefab1Btn.GetComponent<XRSimpleInteractable>().lastSelectExited.AddListener(CreatePrefab1);
        createPrefab2Btn.GetComponent<XRSimpleInteractable>().lastSelectExited.AddListener(CreatePrefab2);
        createPrefab3Btn.GetComponent<XRSimpleInteractable>().lastSelectExited.AddListener(CreatePrefab3);
        deleteEntityBtn.GetComponent<XRSimpleInteractable>().lastSelectExited.AddListener(DeleteEntity);
    }

    private void OnDisable()
    {
        saveAnchorBtn.GetComponent<XRSimpleInteractable>().lastSelectExited.RemoveListener(SaveAnchor);
        clearAnchorBtn.GetComponent<XRSimpleInteractable>().lastSelectExited.RemoveListener(ClearAnchor);
        createPrefab1Btn.GetComponent<XRSimpleInteractable>().lastSelectExited.RemoveListener(CreatePrefab1);
        createPrefab2Btn.GetComponent<XRSimpleInteractable>().lastSelectExited.RemoveListener(CreatePrefab2);
        createPrefab3Btn.GetComponent<XRSimpleInteractable>().lastSelectExited.RemoveListener(CreatePrefab3);
        deleteEntityBtn.GetComponent<XRSimpleInteractable>().lastSelectExited.RemoveListener(DeleteEntity);
    }

    private void SaveAnchor(SelectExitEventArgs arg0)
    {
        _ = Game.instance.EntityManager.SaveGameEntities();
        //真正持久化数据
        _ = PersistentLoader.SaveData();
    }
    
    private  void ClearAnchor(SelectExitEventArgs arg0)
    {
        _ = Game.instance.EntityManager.ClearGameEntities();
        PersistentLoader.DeleteAllData();
    }
    private void CreatePrefab1(SelectExitEventArgs arg0)
    {
        var go = Game.instance.ResourcesLoader.LoadAsset(1,new Vector3(0,0,0),Quaternion.identity) as GameObject; 
        _ = Game.instance.EntityManager.CreateAndAddEntity(go);
    }
    private  void CreatePrefab2(SelectExitEventArgs arg0)
    {
        var go = Game.instance.ResourcesLoader.LoadAsset(2,new Vector3(10,0,0),Quaternion.identity) as GameObject; 
         _ = Game.instance.EntityManager.CreateAndAddEntity(go);
    }
    private  void CreatePrefab3(SelectExitEventArgs arg0)
    {
        var go = Game.instance.ResourcesLoader.LoadAsset(3,new Vector3(0,0,10),Quaternion.identity) as GameObject; 
         _ = Game.instance.EntityManager.CreateAndAddEntity(go);
    }

    private async void DeleteEntity(SelectExitEventArgs arg0)
    {
        // 假设我们要删除第一个实体
        // if (Game.instance.EntityManager.gameEntities.Count > 0)
        // {
        //     var entity = Game.instance.EntityManager.gameEntities[0];
        //     await Game.instance.EntityManager.DeleteEntityAndAnchor(entity);
        // }
    }
}
