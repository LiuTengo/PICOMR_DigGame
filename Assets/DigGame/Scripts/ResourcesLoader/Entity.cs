using System;
using PICOMR.Scripts.ResourcesLoader.Interfaces;
using Unity.XR.PXR;
using UnityEditor;
using UnityEngine;

namespace PICOMR.Scripts.ResourcesLoader
{
    public class AnchorData
    {
        public ulong Handle;
        public Guid Uid;

        public Vector3 Position
        {
            get
            {
                PXR_MixedReality.LocateAnchor(Handle,out var anchorPosition,out _);
                return anchorPosition;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                PXR_MixedReality.LocateAnchor(Handle,out _,out var anchorRotation);
                return anchorRotation;
            }
        }

        public AnchorData(ulong handle, Guid uid)
        {
            Handle = handle;
            Uid = uid;
        }
        
    }
    
    public class Entity : IEntity
    {
        public AnchorData AnchorData { get; set; }
        public GameObject GameObject { get; set; }
        public bool IsRoomEntity { get; set; }
    }
}
