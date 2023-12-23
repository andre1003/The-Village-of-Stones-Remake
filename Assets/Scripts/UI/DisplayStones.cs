using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayStones : MonoBehaviour
{
    public GameObject stonesCanvas;
    public Fader stonesFader;
    public List<GameObject> stoneList;

    public void Display()
    {
        stonesCanvas.SetActive(true);
        stonesFader.FadeIn();

        // This is not the best option, but it will work perfectly fine for this game
        int mapIndex = MapMenu.instance.index;

        for(int i = 0; i < mapIndex; i++)
            stoneList[i].SetActive(true);

        stoneList[mapIndex].SetActive(true);
        stoneList[mapIndex].GetComponent<CanvasGroup>().alpha = 0.3f;
    }

    public void Hide()
    {
        StartCoroutine(WaitForHide());
    }

    IEnumerator WaitForHide()
    {
        stonesFader.FadeOut();
        yield return new WaitForFade(stonesFader);
        stonesCanvas.SetActive(false);
    }
}
