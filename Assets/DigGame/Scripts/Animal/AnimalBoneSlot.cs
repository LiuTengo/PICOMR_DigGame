using DerekLiu.Scripts;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AnimalBoneSlot : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform animalParent;
    
    private XRSocketInteractor SocketInteractor;
    private BoneBodyBase treasure;
    
    private void OnEnable()
    {
        if (SocketInteractor == null)
        {
            SocketInteractor = GetComponentInChildren<XRSocketInteractor>();
            SocketInteractor.selectEntered.AddListener(OnSetAnimalBone);
        }
    }

    private void OnDisable()
    {
        SocketInteractor.selectEntered.RemoveListener(OnSetAnimalBone);
    }

    private void OnSetAnimalBone(SelectEnterEventArgs eventArgs)
    {
        treasure = eventArgs.interactableObject.transform.GetComponent<BoneBodyBase>();
        if (treasure != null)
        {
            if (treasure.HasAllPartOfSkeleton())
            {
                AnimalManager.instance.GenerateAnimal(treasure.animalType,spawnPoint,animalParent);
                Invoke(nameof(DestroySlotAndBone), 0.5f);  
            }
            else
            {
                //TODO: Do something
            }
        }
    }

    private void DestroySlotAndBone()
    {
        Destroy(treasure.gameObject);
        Destroy(this.gameObject);
    }
}
