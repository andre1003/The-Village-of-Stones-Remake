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
    public bool changeSceneWhenFinish = true;
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
        if(kinematicClip)
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
        if(changeSceneWhenFinish)
        {
            LoadNextLevel();
        }
        else
        {
            screens[screenIndex].SetActive(false);
            screenIndex = screens.Count - 1;
            if(sentenceText)
                sentenceText.text = sentences[screenIndex];
            screens[screenIndex].SetActive(true);
            fader.FadeOut();
            yield return new WaitForFade(fader);
            fader.gameObject.SetActive(false);
        }
    }

    // Start cutscene
    public void StartKinematic()
    {
        if(sentenceText)
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
            if(sentenceText)
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
            canSkip = false;
            if(changeSceneWhenFinish)
            {
                // Fade in black screen
                fader.FadeIn(0.9f);

                // Wait for fade end and load next level
                yield return new WaitForFade(fader);
                LoadNextLevel();
            }                
            else
                fader.gameObject.SetActive(false);
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
            AudioManager.instance.SwapTrack(GameFlow.instance.dialogue);
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
