using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace DerekLiu.Scripts
{
    public class SandTableModel : PlaceableObject, IToolInteractableObject
    {
        public XRGrabInteractable xrGrabInteractable;

        private void Awake()
        {
            xrGrabInteractable = GetComponent<XRGrabInteractable>();
        }

        public override void PlaceObjectInSandTable(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public override void ScaleObject()
        {
            
        }

        public void OnToolInteract(DerekDigGameTool tool)
        {
            switch (tool.toolType)
            {
                
            }
        }
    }
}