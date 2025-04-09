using Unity.VisualScripting;
using UnityEngine;

namespace DerekLiu.Scripts
{
    public class DerekHammer : DerekDigGameTool
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