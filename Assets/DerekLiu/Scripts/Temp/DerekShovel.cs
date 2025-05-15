using UnityEngine;

namespace DerekLiu.Scripts
{
    public class DerekShovel : DerekDigGameTool
    {
        private void OnCollisionEnter(Collision collision)
        {
            var interactableObj = collision.gameObject.GetComponent<IToolInteractableObject>();
            if (interactableObj != null)
            {
                interactableObj.OnToolInteract(this);
            }
        }
    }
}
