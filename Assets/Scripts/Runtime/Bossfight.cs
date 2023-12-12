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


    public List<Character> characters;
    public int characterIndex = 0;
    public int turn = 1;
    public bool isFighting = false;


    void Update()
    {
        if(!isFighting)
        {
            return;
        }
        CheckFightEnd();
    }

    public void StartFight()
    {
        characterIndex = 0;
        turn = 1;
        isFighting = true;
    }

    private void CheckFightEnd()
    {
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

    private void EnemyDecision()
    {
        Character character = GetCurrentTurnCharacter();
        if(character.isPlayer)
        {
            return;
        }
        character.TakeDecision();
    }

    public void NextRound()
    {
        if(!isFighting)
        {
            return;
        }

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
        HUD.instance.SetTurnText(turn.ToString());
    }

    public Character GetCurrentTurnCharacter()
    {
        return characters[characterIndex];
    }

    public string GetCurrentTurnCharacterName()
    {
        return characters[characterIndex].name;
    }

    public Character GetEnemyByIndex(int index)
    {
        if(index == characterIndex)
        {
            return null;
        }

        return characters[index];
    }

    public Character GetEnemy()
    {
        if(characterIndex == 1)
            return characters[0];
        return characters[1];
    }
    public bool IsPlayerTurn()
    {
        return characters[characterIndex].isPlayer;
    }
}
