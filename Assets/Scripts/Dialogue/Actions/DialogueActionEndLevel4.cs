using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActionEndLevel4 : DialogueAction
{
    public float bossDamageMultiplier = 0.2f;
    public float bossArmorMultiplier = 0.2f;
    public AudioClip finalDialogAudio;

    // Execute method override
    public override void Execute()
    {
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
        GameFlow.instance.dialogue = finalDialogAudio;
        HUD.instance.ResetForNewFight();
        GameFlow.instance.StartBossfight();
    }
}
