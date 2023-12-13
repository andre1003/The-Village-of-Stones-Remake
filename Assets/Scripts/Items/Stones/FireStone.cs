using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStone : Stone
{
    // Damage
    public float damage;


    // Stone use overridden for Fire Stone
    public override void Use(Character user, Character enemy)
    {
        // If there are no uses left for the stone, don't use it
        if(uses <= 0)
        {
            return;
        }

        // Call base use method
        base.Use(user, enemy);

        // Damage enemy
        enemy.TakeHit(damage, 0f);
    }
}