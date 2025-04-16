using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance;
    public List<GameObject> objectPrefabs;

    [Header("��������")]
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

        // ��ȡ��ǰץ��λ�ú���ת
        Vector3 spawnPosition = currentInteractor.attachTransform.position
                              + currentInteractor.attachTransform.forward * positionOffset;
        Quaternion spawnRotation = currentInteractor.attachTransform.rotation;

        // ���ٵ�ǰ����
        if (currentInteractor.selectTarget != null)
        {
            Destroy(currentInteractor.selectTarget.gameObject);
        }

        // ʵ����������
        currentIndex = (currentIndex + 1) % objectPrefabs.Count;
        GameObject newObj = Instantiate(
            objectPrefabs[currentIndex],
            spawnPosition,
            spawnRotation
        );

        // ����ץȡ����
        XRGrabInteractable grabInteractable = newObj.GetComponent<XRGrabInteractable>();
        ConfigureGrabInteractable(grabInteractable);

        // ǿ��ץȡ������
        yield return null; // �ȴ�һ֡ȷ�������ʼ��
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
