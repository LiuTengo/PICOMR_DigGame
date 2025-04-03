using UnityEngine;

namespace PICOMR.Scripts.ResourcesLoader.Interfaces
{
    public interface IEntity
    {
        AnchorData AnchorData { get; }
        
        GameObject GameObject { get; }
        bool IsRoomEntity { get; }
    }
}