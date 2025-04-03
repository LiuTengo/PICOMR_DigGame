using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    private static T Instance;

    public static T instance => Instance;

    public virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = (T)this;
    }
}
