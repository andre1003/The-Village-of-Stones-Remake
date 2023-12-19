using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEmptyChildren : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int count = 0;
        foreach(Transform child in transform)
            count++;
        if(count == 0)
            Destroy(gameObject);
    }
}
