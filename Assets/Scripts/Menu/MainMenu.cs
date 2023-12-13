using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Fade
    public GameObject fadeCanvas;
    public Animation fadeAnimation;
    public AnimationClip fadeInClip;


    // Play game
    public void Play()
    {
        StartCoroutine(StartGameAsync());
    }

    // Start game async
    IEnumerator StartGameAsync()
    {
        // Fade screen
        fadeCanvas.SetActive(true);
        fadeAnimation.clip = fadeInClip;
        fadeAnimation.Play();
        yield return new WaitForSeconds(fadeInClip.length);

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
