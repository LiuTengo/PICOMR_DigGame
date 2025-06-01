using System;
using BehaviorDesigner.Runtime;
using DerekLiu.Scripts;
using PICOMR.Scripts.ResourcesLoader;
using PICOMR.Scripts.ResourcesLoader.Interfaces;
using UnityEngine;

public class Dinosour : MonoBehaviour,IEntity
{
    public AnimalType animalType;
    public AnchorData AnchorData { get; }
    public GameObject GameObject { get; }
    public bool IsRoomEntity { get; }
    
    //public ulong EntityID { get; set; }
    
    private Animator animator;
    private BehaviorTree behaviorTree;

    private void Start()
    {
        animator = GetComponent<Animator>();
        behaviorTree = GetComponent<BehaviorTree>();
        behaviorTree.StartWhenEnabled = true;  // 可选，默认是 true
        behaviorTree.EnableBehavior();         // 显式启动行为树
    }
}
