using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImageColor : MonoBehaviour
{
    public Image image;
    public Button button;

    private Color previousColor;
    public bool interactable = true;

    void Start()
    {
        previousColor = image.color;
    }

    void FixedUpdate()
    {
        if(button.interactable == interactable)
        {
            return;
        }

        interactable = button.interactable;
        image.color = previousColor;
        if(!interactable)
        {
            image.color *= button.colors.disabledColor;
        }
    }

    public void PointerEnter()
    {
        if(!interactable)
        {
            return;
        }

        image.color *= button.colors.highlightedColor;
    }

    public void PointerExit()
    {
        if(!interactable)
        {
            return;
        }

        image.color = previousColor;
    }
}
