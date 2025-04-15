using System;
using System.Net.Sockets;
using UnityEngine;

namespace DerekLiu.Scripts
{
    public class TreasureSocket : MonoBehaviour
    {
        public bool IsOccupied;
        public bool CanAttachObject => !SocketParent.IsGrabbing;
        public Treasure SocketParent;
        
        private void Start()
        {
            SocketParent = transform.GetComponentInParent<Treasure>();
            IsOccupied = false;
        }

        public void OccupySocketBy(Treasure t)
        {
            IsOccupied = true;
            
            t.transform.SetParent(this.transform);
            t.transform.localPosition = Vector3.zero;
            t.transform.localRotation = Quaternion.identity;
            t.DisableGrab();
            t.AddGrabCollider(SocketParent);
            
            SocketParent?.ClearAttachedInfo();
            SocketParent?.UpdateSockets(t);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsOccupied) return;
            
            var t = other.transform.GetComponent<TreasureSocket>();
            if (t != null)
            {
                SocketParent?.SetAttachToSocket(t);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            SocketParent?.SetAttachToSocket(null);
        }
    }
}