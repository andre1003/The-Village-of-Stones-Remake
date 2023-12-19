using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAction : MonoBehaviour
{
    // Awake method
    void Awake()
    {
        Dialogue dialogue;
        bool found = gameObject.TryGetComponent(out dialogue);
        if(found)
            dialogue.executeAction = Execute;
    }

    // End dialogue action execution. Must be override!
    public virtual void Execute() { }
}
