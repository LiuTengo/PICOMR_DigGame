using System;
using PICOMR.Scripts.ResourcesLoader;
using UnityEngine;

[Serializable]
public struct AnimalPair
{
    public AnimalType animalType;
    public GameObject animal;
}

public class AnimalManager : SingletonMono<AnimalManager>
{
    public async void GenerateAnimal(AnimalType animalType,Transform spawnTransform,Transform spawnParent)
    {
        uint animalID = 0;
        switch (animalType)
        {
            case AnimalType.Dinosaur:
                animalID = 401;
                break;
            case AnimalType.Ostrich:
                animalID = 402;
                break;
            case AnimalType.Stag:
                animalID = 403;
                break;
        }

        var go = Game.instance.ResourcesLoader.LoadAsset(animalID,
            spawnTransform.position, spawnTransform.rotation,spawnParent,ObjectType.Animal);
        //await Game.instance.EntityManager.CreateGameEntity(go);
    }
}
