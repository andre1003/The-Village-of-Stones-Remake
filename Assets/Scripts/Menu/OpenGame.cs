using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGame : MonoBehaviour
{
    // Fader
    public GameObject fadeGameObject;
    public Fader fader;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameFadeIn());
    }

    IEnumerator GameFadeIn()
    {
        fader.FadeOut();
        yield return new WaitForFade(fader);
        fadeGameObject.SetActive(false);
        Destroy(this);
    }
}
