using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Singleton
    public static PlayerStats instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // Non-combat info
    [Header("Non-Combat info")]
    public int coins;
    public int level;
    public int xp;
    public int nextLevelXp;

    //* Character info *//
    // Info
    [Space(10f)]
    [Header("Info")]
    public new string name;
    public string description;

    // Status
    [Header("Status")]
    public float health;

    // Damage
    [Header("Damage")]
    public float basicDamage;
    public float magicDamage;
    public float basicArmor;
    public float magicArmor;

    // Attack
    [Header("Attack")]
    [Range(1, 20)] public int basicAttackSuccessDice = 6;  // Minimal value for basic attack success
    [Range(1, 20)] public int magicAttackSuccessDice = 6;  // Minimal value for magic attack success
    [Range(1, 20)] public int minCriticalValue = 20;

    // Heal
    [Header("Heal")]
    public float basicHeal = 2f;


    // Called on scene load
    public void OnSceneLoaded()
    {
        // Try to find player game object
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        if(playerGameObject == null)
        {
            return;
        }

        // Find Character component in player
        Character player = playerGameObject.GetComponent<Character>();

        // Setup player data
        player.name = name;
        player.description = description;

        player.health = health;

        player.basicDamage = basicDamage;
        player.magicDamage = magicDamage;
        player.basicArmor = basicArmor;
        player.magicArmor = magicArmor;

        player.basicAttackSuccessDice = basicAttackSuccessDice;
        player.magicAttackSuccessDice = magicAttackSuccessDice;
        player.minCriticalValue = minCriticalValue;

        // Perform player initial setup
        player.InitialSetup();
    }

    // Get bossfight rewards
    public void GetRewards(int coins, int xp)
    {
        this.coins += coins;
        this.xp += xp;
        CheckLevelUp();
    }

    // Check level up
    private void CheckLevelUp()
    {
        // If player has the required XP, level up
        if(xp >= nextLevelXp)
        {
            xp -= nextLevelXp;
            level++;
            CalculateNextLevelXp();
        }
    }

    // Calculate next level required XP
    private void CalculateNextLevelXp()
    {
        nextLevelXp += 5;
    }
}
