using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Fader
    public GameObject fadeGameObject;
    public Fader fader;


    // Play game
    public void Play()
    {
        AudioManager.instance.TurnOffSound();
        StartCoroutine(StartGameAsync());
    }

    // Start game async
    IEnumerator StartGameAsync()
    {
        // Fade screen
        fadeGameObject.SetActive(true);
        fader.FadeIn();
        yield return new WaitForFade(fader);

        // Load first level async
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Prologue");
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    // Quit game
    public void QuitGame()
    {
        Application.Quit();
    }
}
