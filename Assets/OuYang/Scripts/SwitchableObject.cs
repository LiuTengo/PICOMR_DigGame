using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SwitchableObject : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        ToolManager.Instance.RegisterGrab((XRBaseInteractor)args.interactorObject);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        ToolManager.Instance.RegisterGrab(null);
    }
}
