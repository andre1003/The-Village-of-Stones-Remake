using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDuplicated : MonoBehaviour
{
    // Awake method
    void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(gameObject.tag);
        if(objects.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
