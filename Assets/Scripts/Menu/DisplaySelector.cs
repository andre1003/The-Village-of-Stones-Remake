using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySelector : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TextMeshProUGUI infoText;

    public void ShowDisplaysList()
    {
        dropdown.options.Clear();
        dropdown.captionText.text = "Display 0";
        int count = 0;
        foreach(Display display in Display.displays)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = "Display " +  count});
            if(display.active)
            {
                dropdown.captionText.text = "Display " + count;
                dropdown.value = count;
            }
                
            count++;
        }
    }

    public void SelectDisplay(int index)
    {
        if(index < 0 || index >= Display.displays.Length)
        {
            return;
        }

        PlayerPrefs.SetInt("UnitySelectMonitor", index);
        infoText.text = "You must restart the game to apply some settings!";
    }
}
