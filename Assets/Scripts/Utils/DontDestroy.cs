using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Awake method
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
