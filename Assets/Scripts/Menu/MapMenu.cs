using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapMenu : MonoBehaviour
{
    #region Singleton
    public static MapMenu instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    #endregion

    public GameObject fadeCanvas;
    public Animation fadeAnimation;
    public AnimationClip fadeIn;
    public AnimationClip fadeOut;

    public int index = 1;
    public int mapIndex;


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fadeCanvas.SetActive(true);
        StartCoroutine(FadeInScreen());
    }

    public void LoadMap()
    {
        fadeCanvas.SetActive(true);
        StartCoroutine(LoadMapAsync());
    }

    public void LoadLevel()
    {
        fadeCanvas.SetActive(true);
        StartCoroutine(FadeOutScreen());
    }

    IEnumerator FadeOutScreen()
    {
        fadeAnimation.clip = fadeOut;
        fadeAnimation.Play();
        yield return new WaitForSeconds(fadeOut.length);
        StartCoroutine(LoadLevelAsync());
    }

    IEnumerator FadeInScreen()
    {
        fadeAnimation.clip = fadeIn;
        fadeAnimation.Play();
        yield return new WaitForSeconds(fadeIn.length);
        fadeCanvas.SetActive(false);
    }

    IEnumerator LoadLevelAsync()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        index++;
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadMapAsync()
    {
        fadeAnimation.clip = fadeOut;
        fadeAnimation.Play();
        yield return new WaitForSeconds(fadeOut.length);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(mapIndex);
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}
