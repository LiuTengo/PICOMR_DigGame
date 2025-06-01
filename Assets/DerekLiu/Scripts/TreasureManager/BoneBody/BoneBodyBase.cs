using DerekLiu.Scripts;

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

    public void CheckIfFinish()
    {
        if (HasAllPartOfSkeleton())
        {
            DisableAllSocket();
        }
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

    private void DisableAllSocket()
    {
        foreach (CustomBoneSocket socket in BoneSockets)
        {
            socket.enabled = false;
        }
    }
}
