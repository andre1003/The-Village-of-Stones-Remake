using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthStone : Stone
{
    // Armor buff
    [Range(0f, 1f)] public float basicArmorBuffPercent = 0.25f;
    [Range(0f, 1f)] public float magicArmorBuffPercent = 0.25f;


    // Awake method
    void Awake()
    {
        // Make sure this stone can be used only once
        uses = 1;
    }

    // Stone use overridden for Earth Stone
    public override void Use(Character user, Character enemy)
    {
        // If there are no uses left for the stone, don't use it
        if(uses <= 0)
        {
            return;
        }

        // Call base use method
        base.Use(user, enemy);

        // Set user invencible and buff armor
        user.isInvencible = true;
        float basicArmorBuff = user.basicArmor * basicArmorBuffPercent;
        float magicArmorBuff = user.magicArmor * magicArmorBuffPercent;
        user.BuffArmor(basicArmorBuff, magicArmorBuff);

        // Call next round
        Bossfight.instance.NextRound();
    }

    // Remove effect overriden for Earth Stone
    public override void RemoveEffect()
    {
        // Remove user invencibility
        user.isInvencible = false;

        // Call base remove effect method
        base.RemoveEffect();
    }
}
