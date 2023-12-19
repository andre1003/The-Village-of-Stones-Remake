using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontDestroyObjects
{
    public class DontDestroy : MonoBehaviour
    {
        // Awake method
        void Awake()
        {
            gameObject.DontDestroyOnLoad();
        }
    }
}
