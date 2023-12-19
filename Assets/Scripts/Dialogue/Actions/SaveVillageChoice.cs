using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveVillageChoice : DialogueAction
{
    // Game objects
    public GameObject endChoiceCanvas;
    public GameObject optionsCanvas;

    // Faders
    public Fader endChoicesFader;
    public Fader backgroundFader;
    public Fader optionsFader;

    // UI
    public TextMeshProUGUI infoText;

    // Audio
    public AudioClip finalClip;


    // Execute method override
    public override void Execute()
    {
        GameFlow.instance.dialogue = finalClip;
        AudioManager.instance.SwapTrack(finalClip);
        endChoiceCanvas.SetActive(true);
        endChoicesFader.FadeIn();
        DialogueManager.instance.StopDialogue(false);
    }

    // Seve HumanTown
    public void SaveHumanTown()
    {
        ClearScreenAndSetInfo(
            "The hero decided to save HumanTown. Now, the monsters have been extinguished and the village is safe." +
            "\n\nWas this the righ choice to be made?");
    }

    // Save MonsterTown
    public void SaveMonsterTown()
    {
        ClearScreenAndSetInfo(
            "The hero decided to save MonsterTown. But doing so, the village was vanished..." +
            "\n\nWas this the righ choice to be made?");
    }

    // Clear screen and show choice consequence
    private void ClearScreenAndSetInfo(string info)
    {
        optionsFader.FadeOut(0.5f);
        backgroundFader.FadeIn(14f);
        infoText.text = info;
        StartCoroutine(WaitForFinishLevel());
    }

    // Wait for finish this level
    IEnumerator WaitForFinishLevel()
    {
        // Wait for options to complete fade and disable it
        yield return new WaitForFade(optionsFader);
        optionsCanvas.SetActive(false);

        // Wait for background to complete fade in and end dialogue
        yield return new WaitForFade(backgroundFader);
        DialogueManager.instance.EndDialogue();
    }
}
