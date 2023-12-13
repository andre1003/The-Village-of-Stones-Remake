using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Kinematic : MonoBehaviour
{
    public TextMeshProUGUI sentenceText;
    public Image fadeImage;
    public Animation fadeAnimation;

    public List<GameObject> screens;
    public List<float> timers;

    [TextArea(3, 10)]
    public List<string> sentences;


    private int screenIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        StartKinematic();
    }

    public void StartKinematic()
    {
        fadeAnimation.Play("FirstFade");
        sentenceText.text = sentences[screenIndex];
        StartCoroutine(WaitForNextScreen());
        StartCoroutine(FullFade());
    }

    IEnumerator WaitForNextScreen()
    {
        yield return new WaitForSeconds(timers[screenIndex]);
        screens[screenIndex].SetActive(false);
        screenIndex++;
        
        if(screenIndex < screens.Count)
        {
            sentenceText.text = sentences[screenIndex];
            screens[screenIndex].SetActive(true);
            StartCoroutine(WaitForNextScreen());
        }
        else
        {
            LoadNextLevel();
        }
    }

    IEnumerator FullFade()
    {
        float fadeTime = screenIndex < screens.Count ? (timers[screenIndex] - 1f) : 0f;
        yield return new WaitForSeconds(fadeTime);

        fadeAnimation.Play("FadeClip");

        if(screenIndex < screens.Count)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(FullFade());
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Map");
        while(!asyncLoad.isDone)
        {
            // Update progress bar here
            yield return null;
        }
    }
}
