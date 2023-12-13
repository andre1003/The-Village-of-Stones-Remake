using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthStone : Stone
{
    // Armor buff
    public float basicArmorBuff;
    public float magicArmorBuff;


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
        user.BuffArmor(basicArmorBuff, magicArmorBuff);
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
