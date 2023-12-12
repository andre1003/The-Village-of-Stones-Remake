using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    #region Singleton
    public static GameFlow instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public AudioSource audioSource;
    public AudioClip dialogue;
    public AudioClip fight;
    public AudioClip getStone;
    public AudioClip gameOver;

    public bool isFinalLevel = false;


    void Start()
    {
        HUD.instance.SetHUDVisibility(false);
    }

    public void StartBossfight()
    {
        ChangeAudio(fight);
        HUD.instance.SetHUDVisibility(true);
        Bossfight.instance.StartFight();
    }

    public void EndBossfight()
    {
        ChangeAudio(dialogue);
        HUD.instance.SetHUDVisibility(false);
        DialogueManager.instance.NextSentence();
    }

    public void EndLevel()
    {
        ChangeAudio(getStone);
        HUD.instance.GetStone();
        MapMenu.instance.IncrementLevelIndex();
    }

    public void GameOver()
    {
        ChangeAudio(gameOver, false);
        HUD.instance.GameOver();
    }

    public void Map()
    {
        MapMenu.instance.LoadMap();
    }

    private void ChangeAudio(AudioClip clip, bool loop = true)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }
}
