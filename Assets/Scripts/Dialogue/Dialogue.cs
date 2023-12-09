using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public List<string> names = new List<string>();

    public List<Sprite> sprites = new List<Sprite>();

    [TextArea(3, 10)]
    public List<string> sentences = new List<string>();


    public void NextSentence()
    {
        names.RemoveAt(0);
        sentences.RemoveAt(0);
        sprites.RemoveAt(0);
    }

    public bool HasSentence()
    {
        return sentences.Count > 0;
    }
}
