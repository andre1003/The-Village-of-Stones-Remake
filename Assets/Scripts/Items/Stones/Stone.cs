using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    /* The stones are:
     * - Air Stone: Buff player damage and debuff enemy armor;
     * - Water Stone: Heals the user;
     * - Fire Stone: Damage the enemy;
     * - Earth Stone: Permanently increase user armor and give invulnerabilty for a moment.
     * 
     * When the player get all the stones, a second life will be granted.
     */

    // Stone stats
    public new string name;
    public string description;
    public int cooldown = 2;
    public bool isInCooldown = false;
    public int uses = 4;  // Number of uses during a bossfight


    // Stone user and enemy references
    protected Character user;
    protected Character enemy;


    // Base attributes
    private int baseCooldown;
    private int baseUses;


    // Start method
    void Start()
    {
        baseCooldown = cooldown;
        baseUses = uses;
    }

    // Base stone use method. Must be override!
    public virtual void Use(Character user, Character enemy)
    {
        // Set user and enemy
        this.user = user;
        this.enemy = enemy;

        // Put the stone on cooldown and decrease the use
        isInCooldown = true;
        uses--;
    }

    // Base remove stone effect method. Must be override!
    public virtual void RemoveEffect()
    {
        // Clear user and enemy references
        this.user = null;
        this.enemy = null;
    }

    // Update stone cooldown
    public void UpdateCooldown()
    {
        // If the stone is not on cooldown, exit
        if(!isInCooldown)
        {
            return;
        }

        // Decrease cooldown and check if it reaches 0
        cooldown--;
        if(cooldown == 0)
        {
            // End stone cooldown, reset it and call RemoveEffect method
            isInCooldown = false;
            cooldown = baseCooldown;
            RemoveEffect();
        }
    }

    // Can use this stone?
    public bool CanUseStone()
    {
        if(isInCooldown || uses == 0)
        {
            return false;
        }
        return true;
    }

    // Reset stone attributes
    public void ResetStone()
    {
        uses = baseUses;
        cooldown = baseCooldown;
        isInCooldown = false;
    }
}
