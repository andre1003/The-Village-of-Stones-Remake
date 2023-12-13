using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeTextPosition : MonoBehaviour
{
    // Text references
    public RectTransform text;
    public TextMeshProUGUI textMesh;
    public Vector3 targetPosition = Vector3.zero;

    // On game object enable
    void OnEnable()
    {
        // Change text local position to target
        text.localPosition = targetPosition;
    }
}
