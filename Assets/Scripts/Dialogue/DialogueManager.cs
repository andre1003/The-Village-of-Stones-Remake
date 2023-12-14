using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        NextSentence();
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            NextSentence();
        }
    }

    // Get next dialogue sentence
    public void NextSentence()
    {
        // Activate dialogue canvas
        dialogueCanvas.SetActive(true);

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

        // Deactivate dialogue canvas and block next sentence call
        dialogueCanvas.SetActive(false);
        canNextSentence = false;

        // Start bossfight and exit
        if(dialogueCount == dialoguesBeforeBossfight)
        {
            GameFlow.instance.StartBossfight();
            return;
        }

        // If there are no more dialogues, end level and exit
        if(dialogues.Count == 0)
        {
            GameFlow.instance.EndLevel();
            return;
        }

        // Call next sentence
        NextSentence();
    }

    // Stop dialogue
    public void StopDialogue()
    {
        // Reset dialogue info
        nameText.text = "";
        sentenceText.text = "";
        dialogues.RemoveAt(0);
        dialogueCount++;

        // Deactivate dialogue canvas and block next sentence call
        dialogueCanvas.SetActive(false);
        canNextSentence = false;
    }

    // Set next sentence controller
    public void SetCanNextSentence(bool canNextSentence)
    {
        this.canNextSentence = canNextSentence;
    }
}
