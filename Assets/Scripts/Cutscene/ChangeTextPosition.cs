using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeTextPosition : MonoBehaviour
{
    public RectTransform text;
    public TextMeshProUGUI textMesh;
    public Vector3 targetPosition = Vector3.zero;

    void OnEnable()
    {
        text.localPosition = targetPosition;
    }
}
