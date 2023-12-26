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

    // Simulator
    [Header("Simulator")]
    public bool isSimulator = false;
    public int numberOfFights = 100;


    // Simulator
    private int playerWins = 0;
    private int enemyWins = 0;


    void Start()
    {
        if(isSimulator)
            StartFight();
    }

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
        for(int i = 0; i < characters[0].stones.Count; i++)
            characters[0].stones[i].ResetStone();

        // Reset character index, turn and fight controller
        characterIndex = isSimulator ? 1 : 0;
        turn = 1;
        isFighting = true;

        if(isSimulator)
            NextRound();
    }

    // Check if the fight must end
    private void CheckFightEnd()
    {
        // Loop fighting characters
        foreach(var character in characters)
        {
            // If character is not dead, continue
            if(!character.isDead)
                continue;

            // Game over
            if(character.isPlayer)
            {
                isFighting = false;
                if(isSimulator)
                {
                    enemyWins++;
                    RestartFight();
                }
                    
                else
                    GameFlow.instance.GameOver();
            }

            // Next step
            else
            {
                isFighting = false;
                if(isSimulator)
                {
                    playerWins++;
                    RestartFight();
                }
                    
                else
                    GameFlow.instance.EndBossfight();
            }
        }
    }

    // Restart fight
    private void RestartFight()
    {
        string winner = characters[0].isDead ? characters[1].name : characters[0].name;
        Debug.Log(">> Fight results: " + turn + " rounds. Winner: " + winner);

        numberOfFights--;
        if(numberOfFights <= 0)
        {
            Debug.Log("End of fights. Player won: " + playerWins + " times. Enemy won: " + enemyWins + " times.");
            return;
        }
        
        characters[0].ResetStats();
        characters[1].ResetStats();
        characterIndex = 0;
        turn = 1;
        isFighting = true;
    }

    // Enemy decision
    private void CharacterDecision()
    {
        // Check if current turn character is not player and call TakeDecision method
        Character character = GetCurrentTurnCharacter();
        if(character.isPlayer && !isSimulator)
        {
            return;
        }
        character.ai.MakeDecision();
    }

    // Go to next round
    public void NextRound()
    {
        // If is not fighting, exit
        if(!isFighting)
        {
            return;
        }

        isFighting = false;

        // Increase turn and character index
        turn++;
        characterIndex++;
        if(characterIndex == characters.Count)
        {
            characterIndex = 0;
        }

        // Adapt AI attack rate
        foreach(Character character in characters)
            character.ai?.AttackRateAdapter();

        // Make decision
        CharacterDecision();

        // Update stone cooldowns and update turn text
        characters[0].UpdateAllStonesCooldown();  // Player stones
        characters[1].UpdateAllStonesCooldown();  // Enemy stones
        if(!isSimulator)
            HUD.instance.SetTurnText(turn.ToString());

        isFighting = true;
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

    // Get boss
    public Character GetBoss()
    {
        return characters[1];
    }

    // Check if this is player's turn
    public bool IsPlayerTurn()
    {
        return characters[characterIndex].isPlayer;
    }

    // Give the stone to the player
    public void GiveStoneToPlayer(Stone stone)
    {
        characters[0].stones.Add(stone);
        characters[1].stones.Clear();
    }
}
