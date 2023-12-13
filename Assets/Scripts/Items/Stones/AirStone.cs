using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStone : Stone
{
    // Player buff
    [Header("Player buff")]
    public float basicDamageBuff;
    public float magicDamageBuff;

    // Enemy debuff
    [Header("Enemy debuff")]
    public float basicArmorDebuff;
    public float magicArmorDebuff;


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

        // Apply Air Stone effects
        user.BuffDamage(basicDamageBuff, magicDamageBuff);
        enemy.DebuffArmor(basicArmorDebuff, magicArmorDebuff);
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
