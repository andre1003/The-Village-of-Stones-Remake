using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public GameObject fadeCanvas;
    public Fader fader;

    // Load main menu
    public void LoadMainMenu()
    {
        fadeCanvas.SetActive(true);
        StartCoroutine(LoadMainMenuAsync());
    }

    // Load main menu async
    IEnumerator LoadMainMenuAsync()
    {
        fader.FadeIn();
        yield return new WaitForFade(fader);
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
