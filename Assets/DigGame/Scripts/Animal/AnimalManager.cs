using System;
using System.Collections.Generic;
using PICOMR.Scripts.ResourcesLoader;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public struct AnimalPair
{
    public AnimalType animalType;
    public GameObject animal;
}

public class AnimalManager : SingletonMono<AnimalManager>
{
    public Transform platformRoot;
    private Vector3 lastPlatformPos;
    public List<NavMeshAgent> animalOnPlane = new ();

    private void Start()
    {
        lastPlatformPos = platformRoot.position;
    }


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
        var agent = go.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            animalOnPlane.Add(agent);
        }
    }

    private void Update()
    {
        Vector3 delta = platformRoot.position - lastPlatformPos;

        if (delta.sqrMagnitude > 0.001f)
        {
            foreach (var agent in animalOnPlane)
            {
                if (agent != null)
                {
                    agent.Warp(agent.transform.position + delta);
                }
            }

            lastPlatformPos = platformRoot.position;
        }
    }
}


