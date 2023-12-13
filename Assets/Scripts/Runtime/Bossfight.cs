using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossfight : MonoBehaviour
{
    #region Singleton
    public static Bossfight instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion

    // Controllers
    [Header("Controllers")]
    public List<Character> characters;
    public int characterIndex = 0;
    public int turn = 1;
    public bool isFighting = false;

    // Rewards
    [Header("Rewards")]
    public int winXp;
    public int winCoins;
    public int loseXp;
    public int loseCoins;


    // Update method
    void Update()
    {
        // If is not fighting, exit
        if(!isFighting)
        {
            return;
        }

        // Check for fight end
        CheckFightEnd();
    }

    // Start bossfight
    public void StartFight()
    {
        // Reset character index, turn and fight controller
        characterIndex = 0;
        turn = 1;
        isFighting = true;
    }

    // Check if the fight must end
    private void CheckFightEnd()
    {
        // Loop fighting characters
        foreach(var character in characters)
        {
            // If character is not dead, continue
            if(!character.isDead)
            {
                character.SuccessChanceAdapter();
                continue;
            }

            // Game over
            if(character.isPlayer)
            {
                GameFlow.instance.GameOver();
                isFighting = false;
            }

            // Next step
            else
            {
                GameFlow.instance.EndBossfight();
                isFighting = false;
            }
        }
    }

    // Enemy decision
    private void EnemyDecision()
    {
        // Check if current turn character is not player and call TakeDecision method
        Character character = GetCurrentTurnCharacter();
        if(character.isPlayer)
        {
            return;
        }
        character.TakeDecision();
    }

    // Go to next round
    public void NextRound()
    {
        // If is not fighting, exit
        if(!isFighting)
        {
            return;
        }

        // Increase turn and character index
        turn++;
        characterIndex++;
        if(characterIndex == characters.Count)
        {
            characterIndex = 0;
        }
        else
        {
            EnemyDecision();
        }

        // Update stone cooldowns and update turn text
        characters[0].UpdateAllStonesCooldown();  // Player stones
        characters[1].UpdateAllStonesCooldown();  // Enemy stones
        HUD.instance.SetTurnText(turn.ToString());
    }

    // Get the current turn character
    public Character GetCurrentTurnCharacter()
    {
        return characters[characterIndex];
    }

    // Get the current turn character name
    public string GetCurrentTurnCharacterName()
    {
        return characters[characterIndex].name;
    }

    // Get enemy by index
    public Character GetEnemyByIndex(int index)
    {
        if(index == characterIndex)
        {
            return null;
        }

        return characters[index];
    }

    // Get current turn enemy
    public Character GetEnemy()
    {
        if(characterIndex == 1)
            return characters[0];
        return characters[1];
    }

    // Get player
    public Character GetPlayer()
    {
        return characters[0];
    }

    // Check if this is player's turn
    public bool IsPlayerTurn()
    {
        return characters[characterIndex].isPlayer;
    }
}
