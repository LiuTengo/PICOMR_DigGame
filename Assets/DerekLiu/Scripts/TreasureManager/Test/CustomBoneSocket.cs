using System;
using System.Collections;
using System.Collections.Generic;
using DerekLiu.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomBoneSocket : XRSocketInteractor
{
    public Transform parentTransform;

    public bool HasSkeleton => transform.childCount > 0;

    protected override void Awake()
    {
        base.Awake();
        if (parentTransform == null)
        {
            parentTransform = transform.parent;
        }
        if (attachTransform == null)
        {
            attachTransform = transform;
        }
    }

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        var treasure = interactable.transform.GetComponent<Treasure>();
        return (base.CanHover(interactable) && treasure.CanHover);
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        var treasure = interactable.transform.GetComponent<Treasure>();
        return (base.CanSelect(interactable)&& treasure.CanHover);
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        var treasure = args.interactableObject.transform.GetComponent<Treasure>();
        treasure.CanHover = false;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        var treasure = args.interactableObject.transform.GetComponent<Treasure>();
        treasure.CanHover = true;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        
        var t = args.interactableObject.transform;
        var treasure = t.GetComponent<Treasure>();
        treasure.OnAttachToSocket(attachTransform);
        
        socketActive = false;
    }
    
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        
        var t = args.interactableObject.transform;
        var treasure = t.GetComponent<Treasure>();
        treasure.OnDetachFromSocket(null);
        socketActive = true;
    }
}
