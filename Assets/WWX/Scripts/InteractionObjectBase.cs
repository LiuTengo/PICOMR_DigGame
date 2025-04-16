using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.OpenXR.NativeTypes;

public class InteractionObjectBase : MonoBehaviour
{
    private XRSocketInteractor xr;
    private Vector3 scale;

    void Start()
    {
        xr=GetComponent<XRSocketInteractor>();
        xr.selectEntered.AddListener(GetScale);
        xr.selectExited.AddListener(GetRelase);
    }
    
    private void GetScale(SelectEnterEventArgs args)
    {
        var obj=args.interactableObject as XRGrabInteractable;
        scale=obj.transform.localScale;
        Debug.Log(obj.transform.position);
        Debug.Log(obj.transform.localPosition);
        //s's's's's's's's's's's's's's's's's's'ssssssssssssssssssssobj.transform.SetParent(transform);
        obj.transform.localPosition=Vector3.zero;
        Debug.Log(obj.transform.localPosition);

    }
    private void GetRelase(SelectExitEventArgs args)
    {
        var obj=args.interactableObject as XRGrabInteractable;
        obj.transform.parent=null;
        obj.transform.localScale=scale;
    }
}
