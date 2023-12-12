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

    public List<Dialogue> dialogues = new List<Dialogue>();

    public GameObject dialogueCanvas;
    public TextMeshProUGUI sentenceText;
    public TextMeshProUGUI nameText;
    public Image speakingCharacterImage;

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


    void Start()
    {
        NextSentence();
    }

    void Update()
    {
        if(!canNextSentence)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            NextSentence();
        }
    }

    public void NextSentence()
    {
        dialogueCanvas.SetActive(true);

        if(dialogues.Count == 0)
        {
            return;
        }

        if(!dialogues[0].HasSentence())
        {
            canNextSentence = false;
            dialogues[0].ExecuteAction();
            return;
        }

        nameText.text = dialogues[0].names[0];
        sentenceText.text = dialogues[0].sentences[0];
        speakingCharacterImage.sprite = dialogues[0].sprites[0];
        dialogues[0].NextSentence();
        canNextSentence = true;
    }

    public void EndDialogue()
    {
        nameText.text = "";
        sentenceText.text = "";
        dialogues.RemoveAt(0);
        dialogueCount++;

        if(dialogueCount < dialoguesBeforeBossfight)
        {
            NextSentence();
            return;
        }

        dialogueCanvas.SetActive(false);
        canNextSentence = false;

        if(dialogueCount == dialoguesBeforeBossfight)
        {
            GameFlow.instance.StartBossfight();
            return;
        }

        if(dialogues.Count == 0)
        {
            GameFlow.instance.EndLevel();
        }

        NextSentence();
    }

    public void SetCanNextSentence(bool canNextSentence)
    {
        this.canNextSentence = canNextSentence;
    }
}
