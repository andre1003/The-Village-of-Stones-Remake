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
    public string nextScene = "Map";
    public bool shouldUnloadScene;
    public AudioSource audioSource;

    // Fader
    public Fader fader;

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
        sentenceText.text = sentences[screenIndex];
        StartCoroutine(WaitForNextScreen());
        StartCoroutine(FullFade());
        fader.FadeOut(1.5f);
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
            sentenceText.text = "";
            yield return new WaitForSeconds(1.1f);
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
        fader.FullFade(0.9f, 0.2f);

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
        AsyncOperation asyncLoad;

        // Unload scene
        if(shouldUnloadScene)
        {
            if(audioSource)
                audioSource.Stop();
            GameFlow.instance.UnPauseAmbienceAudio();
            DialogueManager.instance.SetCanNextSentence(true);
            DialogueManager.instance.NextSentence();
            asyncLoad = SceneManager.UnloadSceneAsync(nextScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }
        // Load next scence
        else
            asyncLoad = SceneManager.LoadSceneAsync(nextScene);

        while(!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
