using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActionLevel1 : DialogueAction
{
    // Index
    public int index;

    // Action required elements
    public AudioSource audioSource;
    public GameObject blackScreen;
    public Animation blackScreenAnimation;


    // Action execute override
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

    // Black screen
    IEnumerator BlackScreen()
    {
        // Pause ambience audio and start black screen animation
        GameFlow.instance.PauseAmbienceAudio();
        blackScreen.SetActive(true);
        blackScreenAnimation.Play();

        // Wait 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        // Play black screen audio, end dialogue and block next sentence
        audioSource.Play();
        DialogueManager.instance.EndDialogue();
        DialogueManager.instance.SetCanNextSentence(false);

        // Wait for black screen animation to end
        yield return new WaitForSeconds(2f);

        // Disable black screen, allow player to continue dialogue and unpause the ambience audio
        blackScreen.SetActive(false);
        DialogueManager.instance.SetCanNextSentence(true);
        GameFlow.instance.UnPauseAmbienceAudio();
    }
}
