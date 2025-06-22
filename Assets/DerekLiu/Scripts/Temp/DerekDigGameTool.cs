using UnityEngine;

namespace DerekLiu.Scripts
{
    public enum DerekDigGameToolType
    {
        Hammer,
        Shovel,
        Brush,
        Magnifier
    }
    
    public class DerekDigGameTool : MonoBehaviour
    {
        public DerekDigGameToolType toolType;
        
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
            var interactableObj = collision.gameObject.GetComponent<IToolInteractableObject>();
            if (interactableObj != null)
            {
                var contacts = collision.contacts;
                interactableObj.SetHitAngle(contacts[0].normal);
                interactableObj.OnToolInteract(this);
            }
        }
    }
}