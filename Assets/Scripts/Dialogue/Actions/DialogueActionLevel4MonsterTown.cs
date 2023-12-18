using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueActionLevel4MonsterTown : DialogueAction
{
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
        blackScreen.SetActive(true);
        blackScreenFader.FadeIn(0.5f);
        yield return new WaitForSeconds(0.6f);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MonsterTownStory", LoadSceneMode.Additive);
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
        
        blackScreenFader.FadeOut(0.5f);
        yield return new WaitForSeconds(0.6f);
        blackScreen.SetActive(false);
    }
}
