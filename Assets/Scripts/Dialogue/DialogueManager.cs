using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    public static DialogueManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion

    // Dialogues list
    public List<Dialogue> dialogues = new List<Dialogue>();

    // Dialogue UI elements
    public GameObject dialogueCanvas;
    public Fader dialogueFader;
    public TextMeshProUGUI sentenceText;
    public TextMeshProUGUI nameText;
    public Image speakingCharacterImage;

    // Number of dialogues before bossfight
    public int dialoguesBeforeBossfight = 1;

    // For now, we will use a simple variable to check how many
    // dialogues should be before the bossfight ends, and use the
    // dialogue count to manage it.
    public int dialogueCount = 0;
    private bool canNextSentence = true;

    /******************************************************************
     * For the dialog, it might be interesting to use UnityEvents.    *
     * See more at: https://docs.unity3d.com/Manual/UnityEvents.html  *
     ******************************************************************/

    /******************************************************************
     * An amazing framework for dialogues: https://yarnspinner.dev    *
     ******************************************************************/


    // Start method
    void Start()
    {
        canNextSentence = false;
        dialogueFader.fadingCanvasGroup.alpha = 0f;

        bool hasDialogue = dialogues?.Any() == true;
        if(hasDialogue)
            StartCoroutine(WaitForFadeIn());
        else
            dialogueCanvas.SetActive(false);
    }

    // Update method
    void Update()
    {
        // If cannot call next sentence, exit
        if(!canNextSentence)
        {
            return;
        }

        // Call next sentence when player press space key
        if(Input.anyKeyDown)
        {
            NextSentence();
        }
    }

    // Wait for first fade in
    IEnumerator WaitForFadeIn()
    {
        yield return new WaitForSeconds(0.5f);
        dialogueCanvas.SetActive(true);
        NextSentence();
        dialogueFader.FadeIn(0.5f);
        yield return new WaitForFade(dialogueFader);
        canNextSentence = true;
        
    }

    // Get next dialogue sentence
    public void NextSentence()
    {
        // Activate dialogue canvas
        if(!dialogueCanvas.activeSelf)
        {
            dialogueCanvas.SetActive(true);
            dialogueFader.FadeIn(0.5f);
        }

        // If there are no dialogues left, exit
        if(dialogues.Count == 0)
        {
            return;
        }

        // If current dialogue has no more sentences, execute dialogue action and exit
        if(!dialogues[0].HasSentence())
        {
            canNextSentence = false;
            dialogues[0].ExecuteAction();
            return;
        }

        // Get dialogue info
        nameText.text = dialogues[0].names[0];
        sentenceText.text = dialogues[0].sentences[0];
        speakingCharacterImage.sprite = dialogues[0].sprites[0];
        dialogues[0].NextSentence();
        canNextSentence = true;
    }

    // End dialogue
    public void EndDialogue()
    {
        // Reset dialogue info
        nameText.text = "";
        sentenceText.text = "";
        dialogues.RemoveAt(0);
        dialogueCount++;

        // If dialogue count is less than number of dialogues before bossfight, continue dialogue
        if(dialogueCount < dialoguesBeforeBossfight)
        {
            NextSentence();
            return;
        }

        // Start bossfight and exit
        if(dialogueCount == dialoguesBeforeBossfight)
        {
            // Deactivate dialogue canvas and block next sentence call
            StartCoroutine(FadeOut());
            canNextSentence = false;
            GameFlow.instance.StartBossfight();
            return;
        }

        // If there are no more dialogues, end level and exit
        if(dialogues.Count == 0)
        {
            // Deactivate dialogue canvas and block next sentence call
            StartCoroutine(FadeOut());
            canNextSentence = false;
            GameFlow.instance.EndLevel();
            return;
        }

        // Call next sentence
        NextSentence();
    }

    // Stop dialogue
    public void StopDialogue(bool skip = true)
    {
        // Reset dialogue info
        nameText.text = "";
        sentenceText.text = "";

        // If requested, skip to next dialogue
        if(skip)
        {
            dialogues.RemoveAt(0);
            dialogueCount++;
        }

        // Deactivate dialogue canvas and block next sentence call
        StartCoroutine(FadeOut());
        canNextSentence = false;
    }

    // Set next sentence controller
    public void SetCanNextSentence(bool canNextSentence)
    {
        this.canNextSentence = canNextSentence;
        if(!dialogueCanvas.activeSelf)
        {
            dialogueCanvas.SetActive(true);
            dialogueFader.FadeIn(0.5f);
        }
    }

    // Fade out
    IEnumerator FadeOut()
    {
        dialogueFader.FadeOut(0.5f);
        yield return new WaitForFade(dialogueFader);
        dialogueCanvas.SetActive(false);
    }
}
