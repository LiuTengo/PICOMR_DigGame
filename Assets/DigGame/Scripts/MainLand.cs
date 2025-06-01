using PICOMR.Scripts.ResourcesLoader;
using PICOMR.Scripts.ResourcesLoader.Interfaces;
using UnityEngine;

public class MainLand : MonoBehaviour,IEntity,IItem
{
    public AnchorData AnchorData { get; }
    public GameObject GameObject { get; }
    public bool IsRoomEntity { get; }
    public ulong EntityID { get; set; }
}
