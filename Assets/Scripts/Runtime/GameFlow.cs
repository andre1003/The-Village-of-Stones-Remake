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

    // Audio
    public AudioSource audioSource;
    public AudioClip dialogue;
    public AudioClip fight;
    public AudioClip getStone;
    public AudioClip gameOver;

    // Final level
    public bool isFinalLevel = false;


    // Start method
    void Start()
    {
        HUD.instance.SetHUDVisibility(false);
    }

    // Start bossfight
    public void StartBossfight()
    {
        // Change audio, activate HUD and call bossfight starter
        ChangeAudio(fight);
        HUD.instance.SetHUDVisibility(true);
        Bossfight.instance.StartFight();
    }

    // End bossfight
    public void EndBossfight()
    {
        // Change audio, disable HUD and start post fight dialogue
        ChangeAudio(dialogue);
        HUD.instance.SetHUDVisibility(false);
        DialogueManager.instance.NextSentence();
    }

    // End current level
    public void EndLevel()
    {
        // Change audio, set HUD to get stone, incremente the level index and give win rewards to player
        ChangeAudio(getStone);
        HUD.instance.GetStone();
        MapMenu.instance.IncrementLevelIndex();
        GiveRewards(true);
    }

    // Game over
    public void GameOver()
    {
        // Change audio, disable HUD, set Game Over interface and give lose rewards to player
        ChangeAudio(gameOver, false);
        HUD.instance.SetHUDVisibility(false);
        HUD.instance.GameOver();
        GiveRewards(false);
    }

    // Load map level
    public void Map()
    {
        MapMenu.instance.LoadMap();
    }

    // Pause ambience music
    public void PauseAmbienceAudio()
    {
        audioSource.Pause();
    }

    // Unpause ambience music
    public void UnPauseAmbienceAudio()
    {
        audioSource.UnPause();
    }

    // Change audio clip
    private void ChangeAudio(AudioClip clip, bool loop = true)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    // Give rewards to player
    private void GiveRewards(bool win)
    {
        // Set coins and xp variables
        int coins = 0;
        int xp = 0;

        // If player wins, get rewards for winning
        if(win)
        {
            coins = Bossfight.instance.winCoins;
            xp = Bossfight.instance.winXp;
        }

        // Else, get rewards for losing
        else
        {
            coins = Bossfight.instance.loseCoins;
            xp = Bossfight.instance.loseXp;
        }
        
        // Give rewards to player
        PlayerStats.instance.GetRewards(coins, xp);
    }
}
