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

    void Start()
    {
        HUD.instance.SetHUDVisibility(false);
    }

    public void StartBossfight()
    {
        audioSource.Stop();
        audioSource.clip = fight;
        audioSource.loop = true;
        audioSource.Play();

        HUD.instance.SetHUDVisibility(true);
        Bossfight.instance.StartFight();
    }

    public void EndBossfight()
    {
        audioSource.Stop();
        audioSource.clip = dialogue;
        audioSource.loop = true;
        audioSource.Play();

        HUD.instance.SetHUDVisibility(false);
        DialogueManager.instance.NextSentence();
    }

    public void EndLevel()
    {
        HUD.instance.PlayerWins();
    }

    public void GameOver()
    {
        HUD.instance.GameOver();
    }

    public void Map()
    {

    }

    public void LoadLevel(int level)
    {

    }
}
