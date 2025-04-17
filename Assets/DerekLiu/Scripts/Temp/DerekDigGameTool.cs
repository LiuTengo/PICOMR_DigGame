using UnityEngine;

namespace DerekLiu.Scripts
{
    public enum DerekDigGameToolType
    {
        Hammer,
        Shovel,
        Brush,
    }
    
    public class DerekDigGameTool : MonoBehaviour
    {
        public DerekDigGameToolType toolType;

        public virtual bool CanInteract()
        {
            return true;
        }
    }
}