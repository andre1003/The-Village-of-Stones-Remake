using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStone : Stone
{
    // Heal
    [Range(0f, 1f)] public float healPercent = 0.2f;
    [Range(0, 20)] public int diceForFullHeal = 5;


    // Stone use overridden for Water Stone
    public override void Use(Character user, Character enemy)
    {
        // If there are no uses left for the stone, don't use it
        if(uses <= 0)
        {
            return;
        }

        // Call base use method
        base.Use(user, enemy);

        // Roll a 1d20 and if roll successfully, make a full heal to the user        
        int dice = Random.Range(1, 21);
        if(dice >= diceForFullHeal)
        {
            user.FullHeal();
            HUD.instance.AddInfo(user.name + " was fully healed");
        }

        // Otherwise, just heal the heal points defined above
        else
        {
            float heal = user.GetBaseHealth() * healPercent;
            user.Heal(heal);
            HUD.instance.AddInfo(user.name + " healed " + heal);
        }
    }
}
