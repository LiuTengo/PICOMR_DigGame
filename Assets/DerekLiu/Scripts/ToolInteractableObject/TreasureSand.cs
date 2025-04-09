using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DerekLiu.Scripts
{
    public class TreasureSand : MonoBehaviour, 
        IToolInteractableObject,ISand
    {
        private Material material;
        private int maxInteractCount = 3;
        private int currentInteractCount = 0;
        
        private void Start()
        {
            material = GetComponent<Renderer>().material;
        }

        public void SetMaxInteractCount(int value)
        {
            maxInteractCount = value;
        }
            
        public void OnToolInteract(DerekDigGameTool tool)
        {
            Debug.Log("OnToolInteract");
            if(tool.toolType == DerekDigGameToolType.Hammer)
            {
                switch (currentInteractCount)
                {
                    case 1:
                        material.color = Color.red;
                        break;
                    case 2:
                        material.color = new Color(1.0f,0.5f,0.0f);
                        break;
                    case 3:
                        material.color = Color.yellow;
                        break;
                }

                currentInteractCount = Mathf.Min(currentInteractCount + 1, maxInteractCount);
                if (currentInteractCount >= maxInteractCount)
                {
                    TreasureManager.instance.SpawnTreasure();
                }
            }
        }
        
    }
}