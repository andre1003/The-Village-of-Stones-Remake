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
    public AudioClip dialogue;
    public AudioClip fight;
    public AudioClip getStone;
    public AudioClip gameOver;

    // Final level
    public bool isFinalLevel = false;

    // Stone
    public Stone stone;


    // Start method
    void Start()
    {
        HUD.instance.SetHUDVisibility(false, true);
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
        if(isFinalLevel && stone)
        {
            DialogueManager.instance.SetCanNextSentence(false);
            GetStone();
            StartCoroutine(WaitPlayerGetStone());
        }
    }

    // Wait for player to get stone and change ambience audio
    IEnumerator WaitPlayerGetStone()
    {
        yield return new WaitForSeconds(13f);
        ChangeAudio(dialogue);
    }

    // End current level
    public void EndLevel()
    {
        // If it is the final level, load the credits
        if(isFinalLevel)
        {
            Credits();
            return;
        }

        // Change audio, set HUD to get stone, incremente the level index and give win rewards to player
        GetStone();
        MapMenu.instance.IncrementLevelIndex();
        GiveRewards(true);
    }

    // Give stone to player
    public void GetStone()
    {
        ChangeAudio(getStone);
        HUD.instance.GetStone();
        stone?.ResetStone();
        Bossfight.instance.GiveStoneToPlayer(stone);
        stone = null;
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

    // Load the credits
    public void Credits()
    {
        MapMenu.instance.LoadCredits();
    }

    // Pause ambience music
    public void PauseAmbienceAudio()
    {
        AudioManager.instance.PauseSound();
    }

    // Unpause ambience music
    public void UnPauseAmbienceAudio()
    {
        AudioManager.instance.UnPauseSound();
    }

    // Change audio clip
    private void ChangeAudio(AudioClip clip, bool loop = true)
    {
        AudioManager.instance.SwapTrack(clip, loop);
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
        PlayerStats.instance?.GetRewards(coins, xp);
    }
}
