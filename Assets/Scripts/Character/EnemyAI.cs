using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyAI : MonoBehaviour
{
    // Attack
    [Header("Attack")]
    [Tooltip("Higher: higher chance to attack.\nLower: lower chance to attack.")]
    [Range(1, 20)] public int attackRate;  // Higher: higher chance to attack - Lower: lower chance to attack

    [Tooltip("Lower chance to attack.\nYou might want to use a value bigger than 10.")]
    [Range(1, 20)] public int lowerAttackRate = 10;  // Lower chance to attack. You might want to use a value bigger than 10

    [Tooltip("Higher chance to attack.\nYou might want to use a value lower than 10.")]
    [Range(1, 20)] public int higherAttackRate = 2;  // Higher chance to attack. You might want to use a value lower than 10

    // Basic and magic attack rate
    [Tooltip("Higher: higher chance to perform a basic attack.\nLower: lower chance to perform a basic attack.")]
    [Range(1, 20)] public int basicAttackRate;
    [Tooltip("Higher: higher chance to perform a magic attack.\nLower: lower chance to perform a magic attack.\nTHIS WILL BE AUTOMATICALLY CALCULATED!")]
    [Range(1, 20)] public int magicAttackRate;

    [Header("Stone use")]
    [Tooltip("Higher: higher chance to use the stone.\nLower: lower chance to use the stone.")]
    [Range(1, 20)] public int stoneUseRate;  // Higher: higher chance to use the stone - Lower: lower chance to use the stone

    [Header("Heal")]
    [Tooltip("Higher: higher chance to heal.\nLower: lower chance to heal.")]
    [Range(1, 20)] public int healRate;  // Higher: higher chance to heal - Lower: lower chance to heal


    // Character reference
    private Character character;


    // Awake method
    void Awake()
    {
        character = GetComponent<Character>();
        if(character == null)
        {
            Destroy(gameObject);
        }

        // Adjust the attack types rate
        magicAttackRate = 20 - basicAttackRate;
    }

    // Adapt attack rate for AI, based on character's health
    public void AttackRateAdapter()
    {
        // If character is player, exit
        if(character.isPlayer)
        {
            return;
        }

        // If character's healht is lower than 20%, decrease the attack rate. Otherwise, increase it
        if(character.health < 0.2f * character.GetBaseHealth())
        {
            attackRate = lowerAttackRate;
        }
        else
        {
            attackRate = higherAttackRate;
        }
    }

    // Decision maker caller
    public void MakeDecision()
    {
        StartCoroutine(WaitForMakeDecision());
    }

    // Decision maker delay
    IEnumerator WaitForMakeDecision()
    {
        // Wait 1 to 3 seconds before take decision
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        // If there is a fight going on, make a decision
        if(Bossfight.instance.isFighting)
        {
            DecisionMaker();
        }
    }

    // Make a decision
    private void DecisionMaker()
    {
        // Define dices
        int attackDice = 0;
        int stoneDice = 0;
        int healDice = 0;

        // Roll dice while all of them are invalid
        do
        {
            attackDice = RollD20();
            stoneDice = character.stones.Count == 0 || character.stones[0].isInCooldown ? 0 : RollD20();
            healDice = RollD20();
        }
        while(!CheckDice(attackDice, stoneDice, healDice));

        // Attack dice is higher
        if(attackDice >= attackRate && attackDice >= stoneDice && attackDice >= healDice)
        {
            attackDice = RollD20();
            if(attackDice >= basicAttackRate)
                character.BasicAttack();
            else
                character.MagicAttack();
        }

        // Stone dice is higher
        else if(stoneDice >= stoneUseRate && stoneDice > attackDice && stoneDice >= healDice)
        {
            character.UseStone(0);
        }

        // Heal dice is higher
        else if(healDice >= healRate && healDice > attackDice && healDice > stoneDice)
        {
            character.Heal();
        }

        // If none of the above conditions are met, perform the action with higher rate
        else
        {
            PerformHigherRateAction();
        }
    }

    // Check if any dice is valid
    private bool CheckDice(int attackDice, int stoneDice, int healDice)
    {
        return attackDice >= attackRate || stoneDice >= stoneUseRate || healDice >= healRate;
    }

    // Perform the action with higher rate
    private void PerformHigherRateAction()
    {
        // Attack
        if(attackRate >= stoneUseRate && attackRate >= healRate)
        {
            if(basicAttackRate >= magicAttackRate)
                character.BasicAttack();
            else
                character.MagicAttack();
        }

        // Use stone
        else if(character.stones.Count > 0 && !character.stones[0].isInCooldown
            && stoneUseRate > attackRate && stoneUseRate >= healRate)
        {
            character.UseStone(0);
        }

        // Heal
        else
        {
            character.Heal();
        }
    }

    // Roll a 1d20 dice
    private int RollD20()
    {
        return Random.Range(1, 21);
    }
}
