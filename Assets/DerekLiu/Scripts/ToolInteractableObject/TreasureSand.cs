using UnityEngine;

namespace DerekLiu.Scripts
{
    public class TreasureSand : TreasureSandBase
    {
        public Texture[] textures;
        
        public override void OnToolInteract(DerekDigGameTool tool)
        {
            if(tool.toolType == DerekDigGameToolType.Hammer)
            {
                material.SetTexture("_BaseMap", textures[currentInteractCount]);

                currentInteractCount = Mathf.Min(currentInteractCount + 1, maxInteractCount);
                if (currentInteractCount >= maxInteractCount)
                {
                    TreasureManager.instance.SpawnTreasure(transform.position, transform.rotation);
                    sandManager.DestroyTreasureSandInPlane(this);
                }
            }
        }
    }
}