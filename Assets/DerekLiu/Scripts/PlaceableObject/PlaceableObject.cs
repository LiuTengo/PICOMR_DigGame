using UnityEngine;

namespace DerekLiu.Scripts
{
    public abstract class PlaceableObject : MonoBehaviour
    {
        public abstract void PlaceObjectInSandTable(Transform parent);
        public abstract void ScaleObject();
    }
}
