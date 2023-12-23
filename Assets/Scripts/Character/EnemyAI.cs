using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EnemyAI : MonoBehaviour
{
    // Attack
    [Header("Attack")]
    [Tooltip("Higher: lower chance to attack.\nLower: higher chance to attack.")]
    [Range(1, 20)] public int attackRate;  // Higher: lower chance to attack - Lower: higher chance to attack

    [Tooltip("Lower chance to attack.\nYou might want to use a value bigger than 10.")]
    [Range(1, 20)] public int lowerAttackRate = 10;  // Lower chance to attack. You might want to use a value bigger than 10

    [Tooltip("Higher chance to attack.\nYou might want to use a value lower than 10.")]
    [Range(1, 20)] public int higherAttackRate = 2;  // Higher chance to attack. You might want to use a value lower than 10

    // Basic and magic attack rate
    [Tooltip("Higher: lower chance to perform a basic attack.\nLower: higher chance to perform a basic attack.")]
    [Range(1, 20)] public int basicAttackRate;
    [Tooltip("Higher: lower chance to perform a magic attack.\nLower: higher chance to perform a magic attack.\nTHIS WILL BE AUTOMATICALLY CALCULATED!")]
    [Range(1, 20)] public int magicAttackRate;

    [Header("Stone use")]
    [Tooltip("Higher: lower chance to use the stone.\nLower: higher chance to use the stone.")]
    [Range(1, 20)] public int stoneUseRate;  // Higher: lower chance to use the stone - Lower: higher chance to use the stone

    [Header("Heal")]
    [Tooltip("Higher: lower chance to heal.\nLower: higher chance to heal.")]
    [Range(1, 20)] public int healRate;  // Higher: lower chance to heal - Lower: higher chance to heal

    // Action delay
    [Header("Action delay")]
    public float minActionDelay = 1f;
    public float maxActionDelay = 3f;


    // Character reference
    private Character character;

    // Base stats
    private int baseAttackRate;
    private int baseBasicAttackRate;
    private int baseMagicAttackRate;
    private int baseStoneUseRate;
    private int baseHealRate;

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

    // Start method
    void Start()
    {
        baseAttackRate = attackRate;
        baseBasicAttackRate = basicAttackRate;
        baseMagicAttackRate = magicAttackRate;
        baseStoneUseRate = stoneUseRate;
        baseHealRate = healRate;
    }

    // Reset all AI stats
    public void ResetAIStats()
    {
        attackRate = baseAttackRate;
        basicAttackRate = baseBasicAttackRate;
        magicAttackRate = baseMagicAttackRate;
        stoneUseRate = baseStoneUseRate;
        healRate = baseHealRate;
    }

    // Adapt attack rate for AI, based on character's health
    public void AttackRateAdapter()
    {
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
        yield return new WaitForSeconds(Random.Range(minActionDelay, maxActionDelay));

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

        // Stone index
        bool noStone = character.stones?.Any() != true;
        int stoneIndex = noStone ? -1 : Random.Range(0, character.stones.Count);

        // Roll dice while all of them are invalid
        do
        {
            attackDice = RollD20();
            stoneDice = CanUseStone(stoneIndex) ? 0 : RollD20();
            healDice = RollD20();
        }
        while(!CheckDice(attackDice, stoneDice, healDice));

        // Attack dice is higher
        if(attackDice >= attackRate && attackDice > stoneDice && attackDice > healDice)
        {
            if(RollD20() >= basicAttackRate)
                character.BasicAttack();
            else
                character.MagicAttack();
        }

        // Stone dice is higher
        else if(CanUseStone(stoneIndex) && stoneDice >= stoneUseRate && stoneDice > attackDice && stoneDice > healDice)
        {
            Debug.ClearDeveloperConsole();
            Debug.Log(character.name + " using stone!");
            character.UseStone(stoneIndex);
        }

        // Heal dice is higher
        else if(healDice >= healRate && healDice > attackDice && healDice > stoneDice)
        {
            character.Heal();
        }

        // If none of the above conditions are met, perform the action with higher rate
        else
        {
            PerformHigherRateAction(stoneIndex);
        }
    }

    // Perform the action with higher rate
    private void PerformHigherRateAction(int stoneIndex)
    {
        bool hasStone = character.stones?.Any() == true;

        // Attack
        if(attackRate <= stoneUseRate && attackRate <= healRate)
        {
            if(basicAttackRate >= magicAttackRate)
                character.BasicAttack();
            else
                character.MagicAttack();
        }

        // Use stone
        else if(CanUseStone(stoneIndex) && stoneUseRate < attackRate && stoneUseRate <= healRate)
        {
            character.UseStone(stoneIndex);
        }

        // Heal
        else
        {
            character.Heal();
        }
    }

    // Check if any dice is valid
    private bool CheckDice(int attackDice, int stoneDice, int healDice)
    {
        return attackDice >= attackRate || stoneDice >= stoneUseRate || healDice >= healRate;
    }

    // Check if the stone can be used
    private bool CanUseStone(int index)
    {
        bool hasStone = character.stones?.Any() == true;
        return hasStone && character.stones[index].CanUseStone();
    }

    // Roll a 1d20 dice
    private int RollD20()
    {
        return Random.Range(1, 21);
    }
}
