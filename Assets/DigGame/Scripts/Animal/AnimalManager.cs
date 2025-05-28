using System;
using System.Collections;
using System.Collections.Generic;
using DerekLiu.Scripts;
using UnityEngine;

[Serializable]
public struct AnimalPair
{
    public AnimalType animalType;
    public GameObject animal;
}

public class AnimalManager : SingletonMono<AnimalManager>
{
    public List<AnimalPair> AnimalPrefabs = new ();
    private Dictionary<AnimalType, GameObject> AnimalSet = new ();

    public List<Dinosour> SpawnedAnimals = new();
    
    public override void Awake()
    {
        base.Awake();
        foreach(AnimalPair pair in AnimalPrefabs)
        {
            AnimalSet.Add(pair.animalType, pair.animal);
        }
    }

    public void GenerateAnimal(AnimalType animalType,Transform spawnTransform,Transform spawnParent)
    {
        AnimalSet.TryGetValue(animalType, out GameObject dinosaur);
        GameObject go = Instantiate(dinosaur,spawnTransform.position,Quaternion.identity,spawnParent);
        
    }
}
