using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueActionLevel4Choice : DialogueAction
{
    public GameObject endChoiceCanvas;
    public GameObject optionsCanvas;
    public Animation fadeAnimation;
    public Animation optionsFadeAnimation;
    public Animation longFadeAnimation;
    public TextMeshProUGUI infoText;

    // Execute method override
    public override void Execute()
    {
        endChoiceCanvas.SetActive(true);
        fadeAnimation.Play();
        DialogueManager.instance.StopDialogue(false);
    }

    public void SaveHumanTown()
    {
        ClearScreenAndSetInfo(
            "The hero decided to save HumanTown. Now, the monsters have been extinguished and the village is safe." +
            "\n\nWas this the righ choice to be made?");
    }

    public void SaveMonsterTown()
    {
        ClearScreenAndSetInfo(
            "The hero decided to save MonsterTown. But doing so, the village was vanished..." +
            "\n\nWas this the righ choice to be made?");
    }

    private void ClearScreenAndSetInfo(string info)
    {
        optionsFadeAnimation.Play();
        longFadeAnimation.Play();
        infoText.text = info;
        StartCoroutine(WaitForFinishLevel());
    }

    IEnumerator WaitForFinishLevel()
    {
        yield return new WaitForSeconds(1f);
        optionsCanvas.SetActive(false);
        yield return new WaitForSeconds(15f);
        DialogueManager.instance.EndDialogue();
    }
}
