using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObjectBase : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("121");
        if(other.CompareTag("Points"))
        {
            Debug.Log("111");
            transform.parent=other.transform;
            transform.localPosition=Vector3.zero;
        }
    }
}
