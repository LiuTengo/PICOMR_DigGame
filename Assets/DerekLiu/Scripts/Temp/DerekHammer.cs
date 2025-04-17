using System;
using Unity.VisualScripting;
using UnityEngine;

namespace DerekLiu.Scripts
{
    public class DerekHammer : DerekDigGameTool
    {
        private Rigidbody m_rigidbody;

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            var interactableObj = collision.gameObject.GetComponent<IToolInteractableObject>();
            if (interactableObj != null)
            {
                interactableObj.OnToolInteract(this);
            }
        }

        public override bool CanInteract()
        {
            return base.CanInteract();
        }
    }
}