using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject fadeCanvas;
    public Animation fadeAnimation;
    public AnimationClip fadeInClip;


    public void Play()
    {
        StartCoroutine(StartGameAsync());
    }

    IEnumerator StartGameAsync()
    {
        fadeCanvas.SetActive(true);
        fadeAnimation.clip = fadeInClip;
        fadeAnimation.Play();
        yield return new WaitForSeconds(fadeInClip.length);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Prologue");
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
