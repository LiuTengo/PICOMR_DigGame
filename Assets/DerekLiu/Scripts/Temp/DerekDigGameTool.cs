using UnityEngine;

namespace DerekLiu.Scripts
{
    public enum DerekDigGameToolType
    {
        Hammer,
        Shovel,
    }
    
    public class DerekDigGameTool : MonoBehaviour
    {
        public DerekDigGameToolType toolType;
    }
}