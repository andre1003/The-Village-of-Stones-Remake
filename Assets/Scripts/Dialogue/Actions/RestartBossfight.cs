using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartBossfight : DialogueAction
{
    // Boss damage multiplier
    public float bossDamageMultiplier = 0.2f;
    public float bossArmorMultiplier = 0.2f;

    // New fight clip
    public AudioClip fightClip2;

    // Execute method override
    public override void Execute()
    {
        // Change fight clip
        GameFlow.instance.fight = fightClip2;

        // Get boss
        Character boss = Bossfight.instance.GetBoss();
        float health = boss.GetBaseHealth() * 2;

        // Reset boss attributes
        boss.health = health;
        boss.basicArmor *= bossArmorMultiplier;
        boss.magicArmor *= bossArmorMultiplier;
        boss.basicDamage *= bossDamageMultiplier;
        boss.magicDamage *= bossDamageMultiplier;
        boss.isDead = false;

        // Stop dialogue and start bossfight
        DialogueManager.instance.StopDialogue();
        HUD.instance.ResetForNewFight();
        GameFlow.instance.StartBossfight();
    }
}
