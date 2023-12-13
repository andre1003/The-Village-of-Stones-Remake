using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActionLevel1 : DialogueAction
{
    public int index;

    public AudioSource audioSource;
    public GameObject blackScreen;
    public Animation blackScreenAnimation;

    public override void Execute()
    {
        if(index == DialogueManager.instance.dialogueCount)
        {
            StartCoroutine(BlackScreen());
        }
        else
        {
            DialogueManager.instance.EndDialogue();
        }
    }

    IEnumerator BlackScreen()
    {
        GameFlow.instance.PauseAmbienceAudio();
        blackScreen.SetActive(true);
        blackScreenAnimation.Play();
        yield return new WaitForSeconds(0.5f);
        audioSource.Play();
        DialogueManager.instance.EndDialogue();
        DialogueManager.instance.SetCanNextSentence(false);
        yield return new WaitForSeconds(2f);
        blackScreen.SetActive(false);
        DialogueManager.instance.SetCanNextSentence(true);
        GameFlow.instance.UnPauseAmbienceAudio();
    }
}
