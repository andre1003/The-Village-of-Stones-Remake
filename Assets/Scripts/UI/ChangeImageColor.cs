using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImageColor : MonoBehaviour
{
    // Image and Button references
    public Image image;
    public Button button;

    // Interactable
    public bool interactable = true;


    // Previous color
    private Color previousColor;


    // Start method
    void Start()
    {
        previousColor = image.color;
    }

    // Fixed Update method
    void FixedUpdate()
    {
        // If button interactable and interactable controller have the same value, exit
        if(button.interactable == interactable)
        {
            return;
        }

        // Set interactable and image color
        interactable = button.interactable;
        image.color = previousColor;

        // If interactable is false, set image color to disabled color
        if(!interactable)
        {
            image.color *= button.colors.disabledColor;
        }
    }

    // On pointer enter
    public void PointerEnter()
    {
        if(!interactable)
        {
            return;
        }

        image.color *= button.colors.highlightedColor;
    }

    // On pointer exit
    public void PointerExit()
    {
        if(!interactable)
        {
            return;
        }

        image.color = previousColor;
    }
}
