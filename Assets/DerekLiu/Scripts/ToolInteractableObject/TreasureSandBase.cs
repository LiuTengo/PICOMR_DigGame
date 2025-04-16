using System;
using PICOMR.Scripts.ResourcesLoader;
using PICOMR.Scripts.ResourcesLoader.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DerekLiu.Scripts
{
    public class TreasureSandBase : MonoBehaviour,IEntity, 
        IToolInteractableObject,ISand
    {
        protected Material material;
        protected int maxInteractCount = 3;
        protected int currentInteractCount = 0;
        
        private void Start()
        {
            material = GetComponent<Renderer>().material;
        }

        public void SetMaxInteractCount(int value)
        {
            maxInteractCount = value;
        }
            
        public virtual void OnToolInteract(DerekDigGameTool tool)
        {
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
                    TreasureManager.instance.SpawnTreasure(transform.position, transform.rotation);
                }
            }
        }

        public AnchorData AnchorData { get; }
        public GameObject GameObject { get; }
        public bool IsRoomEntity { get; }
    }
}