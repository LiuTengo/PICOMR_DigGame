using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace DerekLiu.Scripts
{
    public class Treasure : MonoBehaviour,ITreasure
    {
        public List<TreasureSocket> sockets;
        public bool IsGrabbing = false;
        
        public TreasureSocket attachedSocket;
        private XRGrabInteractable attachedInteractable;
        private Collider collider;

        public void Start()
        {
            collider = GetComponent<Collider>();
            attachedInteractable = GetComponent<XRGrabInteractable>();
            
            attachedInteractable.selectEntered.AddListener(OnSelectEntered);
            attachedInteractable.selectExited.AddListener(OnRelease);
        }

        public void SetAttachToSocket(TreasureSocket socket)
        {
            attachedSocket = socket;
        }

        public void DisableGrab()
        {
            attachedInteractable.trackPosition = false;
            attachedInteractable.trackRotation = false;
        }
        
        public void AddGrabCollider(Treasure parentGrab)
        {
            parentGrab.attachedInteractable.colliders.Add(collider);
        }

        private void OnRelease(SelectExitEventArgs evt)
        {
            if (attachedSocket != null && attachedSocket.CanAttachObject)
            {
                attachedSocket.OccupySocketBy(this);
            }
            
            attachedSocket = null;
            IsGrabbing = false;
        }
        
        private void OnSelectEntered(SelectEnterEventArgs arg0)
        {
            attachedSocket = null;
            IsGrabbing = true;
        }

        public void ClearAttachedInfo()
        {
            attachedSocket = null;
        }

        public void UpdateSockets(Treasure t)
        {
            foreach (var s in t.sockets)
            {
                if (!s.IsOccupied)
                {
                    sockets.Add(s);
                    s.SocketParent = this;
                }
                else
                {
                    s.SocketParent = null;
                }
            }
            t.sockets.Clear();
        }
    }
}