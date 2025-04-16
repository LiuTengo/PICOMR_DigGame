using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance;
    public List<GameObject> objectPrefabs;

    [Header("生成设置")]
    [SerializeField] private float positionOffset = 0.1f;
    [SerializeField] private float switchCooldown = 0.5f;

    private XRBaseInteractor currentInteractor;
    private bool isSwitching;
    private int currentIndex;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterGrab(XRBaseInteractor interactor)
    {
        currentInteractor = interactor;
    }

    public void SwitchObject()
    {
        if (isSwitching || currentInteractor == null) return;

        StartCoroutine(SwitchProcess());
    }

    private IEnumerator SwitchProcess()
    {
        isSwitching = true;

        // 获取当前抓握位置和旋转
        Vector3 spawnPosition = currentInteractor.attachTransform.position
                              + currentInteractor.attachTransform.forward * positionOffset;
        Quaternion spawnRotation = currentInteractor.attachTransform.rotation;

        // 销毁当前物体
        if (currentInteractor.selectTarget != null)
        {
            Destroy(currentInteractor.selectTarget.gameObject);
        }

        // 实例化新物体
        currentIndex = (currentIndex + 1) % objectPrefabs.Count;
        GameObject newObj = Instantiate(
            objectPrefabs[currentIndex],
            spawnPosition,
            spawnRotation
        );

        // 配置抓取参数
        XRGrabInteractable grabInteractable = newObj.GetComponent<XRGrabInteractable>();
        ConfigureGrabInteractable(grabInteractable);

        // 强制抓取新物体
        yield return null; // 等待一帧确保组件初始化
        //currentInteractor = newObj.GetComponent<XRBaseInteractor>();
        //currentInteractor.StartManualInteraction(grabInteractable);



        isSwitching = false;
    }

    private void ConfigureGrabInteractable(XRGrabInteractable grabInteractable)
    {
        grabInteractable.movementType = XRBaseInteractable.MovementType.Instantaneous;
        grabInteractable.throwOnDetach = false;
        grabInteractable.attachEaseInTime = 0.1f;
        grabInteractable.smoothPosition = true;
        grabInteractable.smoothRotation = true;
        grabInteractable.smoothPositionAmount = 0.5f;
        grabInteractable.smoothRotationAmount = 0.5f;
    }
}
