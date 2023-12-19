using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontDestroyObjects
{
    public class DontDestroyException : MonoBehaviour
    {
        // Awake method
        void Awake()
        {
            gameObject.DontDestroyOnLoadExecption();
        }
    }
}