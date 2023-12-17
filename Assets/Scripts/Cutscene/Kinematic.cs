using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Kinematic : MonoBehaviour
{
    // Sentence text
    public TextMeshProUGUI sentenceText;

    // Fade
    public Image fadeImage;
    public Animation fadeAnimation;

    // Cutscene screens and timers
    public List<GameObject> screens;
    public List<float> timers;

    // Cutscene sentences
    [TextArea(3, 10)]
    public List<string> sentences;


    // Current screen index
    private int screenIndex = 0;


    // Start method
    void Start()
    {
        StartKinematic();
    }

    // Start cutscene
    public void StartKinematic()
    {
        fadeAnimation.Play(AnimationHelper.GetAnimationClipNameByIndex(fadeAnimation, 0));
        sentenceText.text = sentences[screenIndex];
        StartCoroutine(WaitForNextScreen());
        StartCoroutine(FullFade());
    }

    // Wait for next cutscene screen
    IEnumerator WaitForNextScreen()
    {
        // Wait for next screen
        yield return new WaitForSeconds(timers[screenIndex]);
        screens[screenIndex].SetActive(false);
        screenIndex++;
        
        // If there are screens left, get next screen
        if(screenIndex < screens.Count)
        {
            sentenceText.text = sentences[screenIndex];
            screens[screenIndex].SetActive(true);
            StartCoroutine(WaitForNextScreen());
        }

        // Else, load next scene
        else
        {
            LoadNextLevel();
        }
    }

    // Perform full screen fade
    IEnumerator FullFade()
    {
        // Get fade timer
        float fadeTime = screenIndex < screens.Count ? (timers[screenIndex] - 1f) : 0f;
        yield return new WaitForSeconds(fadeTime);

        // Play full fade animation
        fadeAnimation.Play(AnimationHelper.GetAnimationClipNameByIndex(fadeAnimation, 1));

        // If there are screens left, prepare for next full fade
        if(screenIndex < screens.Count)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(FullFade());
        }
    }

    // Load next level
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    // Load next level async
    IEnumerator LoadLevel()
    {
        // Load map
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Map");
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
