using UnityEngine;

namespace DerekLiu.Scripts
{
    public interface IToolInteractableObject
    {
        public void SetHitAngle(Vector3 HitAngle);
        
        public void OnToolInteract(DerekDigGameTool tool);
    }
}