using UnityEngine;

namespace DerekLiu.Scripts
{
    public class TreasureSand : TreasureSandBase
    {
        private void OnEnable()
        {
            SetSpawnGameObject(TreasureManager.instance.GetRandomTreasurePrefab());
        }
        
        public override void OnHit()
        {
            //material.SetTexture("_BaseMap", textures[currentInteractCount]);
            currentInteractCount ++;
            
            particleSystem.Play();
            audioSource.PlayOneShot(audioClip);
            
            if (currentInteractCount >= maxInteractCount)
            {
                //Spawn Treasure
                TreasureManager.instance.SpawnTreasure(spawnIndex,transform.position, transform.rotation);
                sandManager.DestroyTreasureSandInPlane(this);
            }
        }

        public override void OnToolInteract(DerekDigGameTool tool)
        {
            base.OnToolInteract(tool);
            
            if(tool.toolType == DerekDigGameToolType.Shovel ||
               tool.toolType == DerekDigGameToolType.Brush)
            {
                OnHit();
            }
        }
    }
}