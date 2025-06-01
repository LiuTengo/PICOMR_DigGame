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
        
    }
}