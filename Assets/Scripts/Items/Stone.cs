using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoneType
{
    Water = 0,
    Fire = 1,
    Earth = 2,
    Air = 3
}

public class Stone : MonoBehaviour
{
    /* The stones are:
     * - Water Stone: Heals the user;
     * - Fire Stone: Damage the enemy;
     * - Earth Stone: Reduces the enemy damage;
     * - Air Stone: Skip enemy round.
     * 
     * When the player get all the stones, a second life will be granted.
     */

    public new string name;
    public string description;

    public StoneType type;
    public int effectValue;
    public int cooldown = 2;
    public bool isInCooldown = false;

    private int baseCooldown;

    void Awake()
    {
        baseCooldown = cooldown;
    }

    public void Use(Character player, Character enemy)
    {
        switch(type)
        {
            case StoneType.Water:
                player.health += effectValue;
                isInCooldown = true;
                break;

            case StoneType.Fire:
                enemy.health -= effectValue;
                break;

            case StoneType.Earth:
                break;

            case StoneType.Air:
                break;
        }
    }

    public void UpdateCooldown()
    {
        if(!isInCooldown)
        {
            return;
        }

        cooldown--;
        if(cooldown == 0)
        {
            isInCooldown = false;
            cooldown = baseCooldown;
        }
    }
}
