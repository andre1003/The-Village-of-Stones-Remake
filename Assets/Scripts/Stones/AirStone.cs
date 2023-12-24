using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStone : Stone
{
    // Player buff
    [Header("User buff")]
    [Range(0f, 1f)] public float basicDamageBuffPercent = 0.1f;
    [Range(0f, 1f)] public float magicDamageBuffPercent = 0.1f;

    // Enemy debuff
    [Header("Enemy debuff")]
    [Range(0f, 1f)] public float basicArmorDebuffPercent = 0.5f;
    [Range(0f, 1f)] public float magicArmorDebuffPercent = 0.5f;


    // Temporary buffs and debuffs
    private float basicDamageBuff;
    private float magicDamageBuff;
    private float basicArmorDebuff;
    private float magicArmorDebuff;

    // Stone use overridden for Air Stone
    public override void Use(Character user, Character enemy)
    {
        // If there are no uses left for the stone, don't use it
        if(uses <= 0)
        {
            return;
        }

        // Call base use method
        base.Use(user, enemy);

        // Apply Air Stone buffs
        basicDamageBuff = user.basicDamage * basicDamageBuffPercent;
        magicDamageBuff = user.magicDamage * magicDamageBuffPercent;
        user.BuffDamage(basicDamageBuff, magicDamageBuff);

        // Apply Air Stone debuffs
        basicArmorDebuff = enemy.basicArmor * basicArmorDebuffPercent;
        magicArmorDebuff = enemy.magicArmor * magicArmorDebuffPercent;
        enemy.DebuffArmor(basicArmorDebuff, magicArmorDebuff);

        // Call next round
        Bossfight.instance.NextRound();
    }

    // Remove effect overriden for Air Stone
    public override void RemoveEffect()
    {
        // Remove applied effects
        user.DebuffDamage(basicDamageBuff, magicDamageBuff);
        enemy.BuffArmor(basicArmorDebuff, magicArmorDebuff);

        // Call base remove effect method
        base.RemoveEffect();
    }
}
