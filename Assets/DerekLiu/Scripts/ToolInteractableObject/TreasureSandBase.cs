using PICOMR.Scripts.ResourcesLoader;
using PICOMR.Scripts.ResourcesLoader.Interfaces;
using UnityEngine;

namespace DerekLiu.Scripts
{
    public class TreasureSandBase : MonoBehaviour, 
        IToolInteractableObject,ISand
    {
        protected Vector3 knockAngle;
        protected SandManager sandManager;
        [SerializeField] protected ParticleSystem particleSystem;
        [SerializeField] protected AudioSource audioSource;
        [SerializeField] protected AudioClip audioClip;
        [SerializeField] protected int maxInteractCount = 3;
        [SerializeField] protected int currentInteractCount = 0;
        [SerializeField] protected ulong spawnIndex;

        protected bool triggered;
        public bool HasTriggered => triggered;

        public void SetSandManager(SandManager sandManager)
        {
            this.sandManager = sandManager;
        }
        
        public void SetMaxInteractCount(int value)
        {
            maxInteractCount = value;
        }

        public void SetSpawnGameObject(ulong goIndex)
        {
            spawnIndex = goIndex;
        }

        public void SetHitAngle(Vector3 HitAngle)
        {
            knockAngle = HitAngle;
        }

        public virtual void OnToolInteract(DerekDigGameTool tool)
        {
            if (HasTriggered)
            {
                return;
            }
            triggered = true;
        }
        
        public virtual void OnHit() { }
    }
}