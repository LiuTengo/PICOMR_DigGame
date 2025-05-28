using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace DerekLiu.Scripts
{
    public class Treasure : TreasureBase
    {
        public bool CanHover = true;
        
        private XRGrabInteractable grabInteractable;

        public void Start()
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
        }

        public void OnAttachToSocket(Transform socketTransform)
        {
            grabInteractable.attachEaseInTime = 0.01f;
            grabInteractable.trackScale = false;
        
            transform.SetParent(socketTransform);
            transform.position = socketTransform.position;
            transform.rotation = socketTransform.rotation;
            transform.localScale = socketTransform.localScale;
        }

        public void OnDetachFromSocket(Transform socketTransform)
        {
            CanHover = true;
            
            grabInteractable.attachEaseInTime = 1.0f;
            grabInteractable.trackScale = true;
        }
    }
}