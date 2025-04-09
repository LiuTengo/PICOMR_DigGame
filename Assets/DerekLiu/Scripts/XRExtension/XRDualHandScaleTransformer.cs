using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class XRDualHandScaleTransformer : XRBaseGrabTransformer
{
    public float maxScale = 1.1f;
    public float minScale = 0.2f;

    private float originalDistance = 0.1f;
    private Vector3 originalScale;
    
    protected override RegistrationMode registrationMode => RegistrationMode.Multiple;
    
    public override void OnGrab(XRGrabInteractable grabInteractable)
    {
        base.OnGrab(grabInteractable);
        InitScaleTransformer(grabInteractable);
    }

    public override void OnGrabCountChanged(XRGrabInteractable grabInteractable, Pose targetPose, Vector3 localScale)
    {
        base.OnGrabCountChanged(grabInteractable, targetPose, localScale);
        InitScaleTransformer(grabInteractable);
        originalScale = localScale;
    }

    public override void Process(XRGrabInteractable grabInteractable, XRInteractionUpdateOrder.UpdatePhase updatePhase, ref Pose targetPose, ref Vector3 localScale)
    {
        switch (updatePhase)
        {
            case XRInteractionUpdateOrder.UpdatePhase.OnBeforeRender:
                UpdateScale(grabInteractable,ref localScale);
                break;
            default:
                break;
        }
    }

    private void UpdateScale(XRGrabInteractable grabInteractable, ref Vector3 localScale)
    {
        Debug.Log("Processing scale transformer");
        if (grabInteractable.interactorsSelecting.Count == 2)
        {
            Debug.Assert(grabInteractable.interactorsSelecting.Count > 1, this);
            
            var primaryAttachPose = grabInteractable.interactorsSelecting[0].GetAttachTransform(grabInteractable).GetWorldPose();
            var secondaryAttachPose = grabInteractable.interactorsSelecting[1].GetAttachTransform(grabInteractable).GetWorldPose();
            
            float distance = Vector3.Distance(primaryAttachPose.position, secondaryAttachPose.position);

            float f = (distance - originalDistance) / originalDistance;
            if (f < 0.0f)
            {
                f *= 2.0f;
            }
            float t = (Mathf.Clamp(f,-1,1)+1)*0.5f;//<0时缩小，>0时放大
            float factor = Mathf.Lerp(minScale,maxScale,t);
            
            localScale = originalScale * factor;
            
        }
    }

    private void InitScaleTransformer(XRGrabInteractable grabInteractable)
    {
        if (grabInteractable.interactorsSelecting.Count  == 2)
        {
            var primaryAttachPose = grabInteractable.interactorsSelecting[0].GetAttachTransform(grabInteractable).GetWorldPose();
            var secondaryAttachPose = grabInteractable.interactorsSelecting[1].GetAttachTransform(grabInteractable).GetWorldPose();

            originalDistance = Vector3.Distance(primaryAttachPose.position , secondaryAttachPose.position);
            Debug.LogError($"Original distance: {originalDistance}");
        }
    }
}
