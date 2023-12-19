using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterTownStory : DialogueAction
{
    // Black screen
    public GameObject blackScreen;
    public Fader blackScreenFader;

    // Execute method override
    public override void Execute()
    {
        GameFlow.instance.PauseAmbienceAudio();
        DialogueManager.instance.StopDialogue();
        StartCoroutine(LoadMonsterTownStory());
    }

    // Load MonsterTownStory scene
    IEnumerator LoadMonsterTownStory()
    {
        // Fade in black screen
        blackScreen.SetActive(true);
        blackScreenFader.FadeIn(0.5f);
        yield return new WaitForFade(blackScreenFader);

        // Load and add MonsterTownStory level
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MonsterTownStory", LoadSceneMode.Additive);
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
        
        // Fade out black screen
        blackScreenFader.FadeOut(0.5f);
        yield return new WaitForFade(blackScreenFader);
        blackScreen.SetActive(false);
    }
}
