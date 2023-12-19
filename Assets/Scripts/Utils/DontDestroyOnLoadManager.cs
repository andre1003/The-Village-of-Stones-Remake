using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontDestroyObjects
{
    static class DontDestroyOnLoadManager
    {
        // List of all DontDestroyOnLoad objects
        private static List<GameObject> ddolObjects = new List<GameObject>();

        // Add an object to ddolObjects list
        public static void DontDestroyOnLoad(this GameObject obj)
        {
            Object.DontDestroyOnLoad(obj);
            ddolObjects.Add(obj);
        }
        
        // Add an object to ddolObjects list. This object will NEVER be destroyed
        public static void DontDestroyOnLoadExecption(this GameObject obj)
        {
            Object.DontDestroyOnLoad(obj);
        }

        // Destroy all ddolObjects items
        public static void DestroyAll()
        {
            foreach(GameObject obj in ddolObjects)
                if(obj != null)
                    Object.Destroy(obj);
            ddolObjects.Clear();
        }
    }
}
