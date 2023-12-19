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
    public AudioClip kinematicClip;

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
    private bool skipped = false;
    private bool canSkip = false;


    // Start method
    void Start()
    {
        AudioManager.instance.SwapTrack(kinematicClip, false);
        StartKinematic();
    }

    // Update method
    void Update()
    {
        if(skipped || !canSkip)
            return;
        CheckSkipKinematic();
    }

    // Check for skip kinematic input
    private void CheckSkipKinematic()
    {
        if(Input.anyKeyDown)
        {
            skipped = true;
            canSkip = false;
            StopAllCoroutines();
            StartCoroutine(SkipKinematic());
        }
    }

    // Skip kinematic
    IEnumerator SkipKinematic()
    {
        fader.FadeIn();
        yield return new WaitForFade(fader);
        LoadNextLevel();
    }

    // Start cutscene
    public void StartKinematic()
    {
        sentenceText.text = sentences[screenIndex];
        StartCoroutine(WaitForNextStep());
        fader.FadeOut(1.5f);
        StartCoroutine(WaitForAllowSkip());
    }

    // Wait for initial fade to allow skip
    IEnumerator WaitForAllowSkip()
    {
        yield return new WaitForFade(fader);
        canSkip = true;
    }

    // Screen navigation handler
    IEnumerator WaitForNextStep()
    {
        if(skipped)
            yield break;

        // Wait for timer
        yield return new WaitForSeconds(timers[screenIndex]);

        // If the screen index is not the last
        if(screenIndex < screens.Count - 1)
        {
            // Full fade black screen (2 seconds for full fade)
            fader.FullFade(0.9f, 0.2f);
            canSkip = false;

            // Wait for half full fade, disable current screen and increment index
            yield return new WaitForHalfFade(fader);
            screens[screenIndex].SetActive(false);
            screenIndex++;

            // Set new text and screen
            sentenceText.text = sentences[screenIndex];
            screens[screenIndex].SetActive(true);

            // Wait for full fade to end and call WaitForNextStep
            yield return new WaitForFullFade(fader);
            canSkip = true;
            StartCoroutine(WaitForNextStep());
            
        }
        // If it's the last screen
        else
        {
            // Fade in black screen
            fader.FadeIn(0.9f);

            // Wait for fade end and load next level
            yield return new WaitForFade(fader);
            LoadNextLevel();
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
            AudioManager.instance.ReturnToDefault();
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
