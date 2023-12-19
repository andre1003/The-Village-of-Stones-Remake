using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBlackScreen : DialogueAction
{
    // Action required elements
    public AudioSource audioSource;
    public GameObject blackScreen;
    public Fader blackScreenFader;


    // Execute action
    public override void Execute()
    {
        StartCoroutine(BlackScreen());
    }

    // Black screen
    IEnumerator BlackScreen()
    {
        // Pause ambience audio and start black screen animation
        GameFlow.instance.PauseAmbienceAudio();
        blackScreen.SetActive(true);
        blackScreenFader.FullFade(0.5f, 1.5f);

        // Wait black screen fade in
        yield return new WaitForHalfFade(blackScreenFader);

        // Play black screen audio, end dialogue and block next sentence
        audioSource.Play();
        DialogueManager.instance.EndDialogue();
        DialogueManager.instance.SetCanNextSentence(false);

        // Wait for black screen animation to end
        yield return new WaitForFullFade(blackScreenFader);

        // Disable black screen, allow player to continue dialogue and unpause the ambience audio
        blackScreen.SetActive(false);
        DialogueManager.instance.SetCanNextSentence(true);
        GameFlow.instance.UnPauseAmbienceAudio();
    }
}
