using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketTest : MonoBehaviour
{
    public XRSocketInteractor socketInteractor;
    
    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        socketInteractor.selectEntered.AddListener(OnSelectEntered);
        socketInteractor.selectExited.AddListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        socketInteractor.fixedScale = new Vector3
            (1.0f/socketInteractor.transform.lossyScale.x, 1.0f/socketInteractor.transform.lossyScale.y, 1.0f/socketInteractor.transform.lossyScale.z);
        
        Transform targetTransform = args.interactableObject.transform;
        targetTransform.SetParent(socketInteractor.transform, true);
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        Transform targetTransform = args.interactableObject.transform;
        targetTransform.SetParent(null, true);
    }
    
    private void ApplyWorldScale(Transform target, Vector3 worldScale)
    {
        Transform parent = target.parent;
        Debug.Log(parent.name);
        if (parent != null)
        {
            Vector3 parentScale = parent.lossyScale;
            Vector3 newScale= new Vector3(
                worldScale.x / parentScale.x,
                worldScale.y / parentScale.y,
                worldScale.z / parentScale.z
            );
            target.localScale = newScale;
            Debug.Log(target.localScale);
        }
        else
        {
            target.localScale = worldScale;
        }
    }
}
