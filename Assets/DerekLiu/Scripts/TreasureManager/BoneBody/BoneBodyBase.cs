using System;
using System.Collections.Generic;
using DerekLiu.Scripts;
using UnityEngine;

public enum AnimalType
{
    Dinosaur,
    Stag,
    Ostrich,
}

public class BoneBodyBase : Treasure
{
    public AnimalType animalType;
    
    private CustomBoneSocket[] BoneSockets;

    private void Start()
    {
        BoneSockets = transform.GetComponentsInChildren<CustomBoneSocket>();
    }

    public bool HasAllPartOfSkeleton()
    {
        foreach (CustomBoneSocket socket in BoneSockets)
        {
            if (!socket.HasSkeleton)
            {
                return false;
            }
        }
        return true;
    }
    
}
