using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStone : Stone
{
    // Heal
    public float heal;
    [Range(0, 20)] public int diceForFullHeal = 10;


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
        }

        // Otherwise, just heal the heal points defined above
        else
        {
            user.Heal(heal);
        }
    }
}
