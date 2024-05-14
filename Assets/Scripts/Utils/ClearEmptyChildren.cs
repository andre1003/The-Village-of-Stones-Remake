using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontDestroyObjects
{
    public class ClearEmptyChildren : MonoBehaviour
    {
        // LateUpdate method
        void LateUpdate()
        {
            int count = 0;
            foreach(Transform child in transform)
                count++;
            if(count == 0)
                gameObject.DestroyObj();
            else
                Destroy(this);
        }
    }
}
