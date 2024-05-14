using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsUI : MonoBehaviour
{
    public static RewardsUI instance;

    void Awake()
    {
        if(instance != null) Destroy(gameObject);
        instance = this;
    }
}
