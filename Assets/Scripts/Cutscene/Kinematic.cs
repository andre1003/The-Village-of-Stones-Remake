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

    public int nextLevelIndex = -1;

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
        int nextScene = nextLevelIndex;
        if(nextLevelIndex == -1)
        {
            nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        }

        if(nextScene < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadLevel(nextScene));
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIndex);
        while(!asyncLoad.isDone)
        {
            // Update progress bar here
            yield return null;
        }
    }
}
