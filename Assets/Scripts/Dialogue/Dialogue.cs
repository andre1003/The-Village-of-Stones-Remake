using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    // Dialogue names
    public List<string> names = new List<string>();

    // Dialogue sprites
    public List<Sprite> sprites = new List<Sprite>();

    // Dialogue sentences
    [TextArea(3, 10)]
    public List<string> sentences = new List<string>();

    // Action execution delegate
    public delegate void Execute();
    public Execute executeAction;


    // Awake method
    void Awake()
    {
        executeAction = new Execute(ExecuteActionCallback);
    }

    // Default dialogue action execution
    public static void ExecuteActionCallback()
    {
        DialogueManager.instance.EndDialogue();
    }

    // Get dialogue next sentence
    public void NextSentence()
    {
        names.RemoveAt(0);
        sentences.RemoveAt(0);
        sprites.RemoveAt(0);
    }

    // Check if this dialogue has any sentence left
    public bool HasSentence()
    {
        return sentences.Count > 0;
    }

    // Execute dialogue action if any, or end dialogue
    public void ExecuteAction()
    {
        executeAction.Invoke();
    }
}
