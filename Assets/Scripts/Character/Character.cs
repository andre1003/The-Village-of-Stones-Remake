using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Info
    [Header("Info")]
    public new string name;
    public string description;

    // Status
    [Header("Status")]
    public float health;
    public float damage;
    public bool isDead = false;
    public bool isPlayer = false;

    // Attack
    [Header("Attack")]
    [Range(1, 20)] public int attackSuccessPercent = 6;
    public int minCriticalValue = 20;

    // Heal
    [Header("Heal")]
    public float basicHeal = 2f;

    // Decision
    [Header("Decision")]
    [Range(1, 20)] public int attackSuccessChance;
    [Range(1, 20)] public int maxAttackSuccessChance = 10;
    [Range(1, 20)] public int minAttackSuccessChance = 2;


    // Status
    private float baseHealth;


    void Start()
    {
        baseHealth = health;
        attackSuccessChance = minAttackSuccessChance;
    }

    public void SuccessChanceAdapter()
    {
        if(isPlayer)
        {
            return;
        }

        if(health < 0.2f * baseHealth)
        {
            attackSuccessChance = maxAttackSuccessChance;
        }
        else
        {
            attackSuccessChance = minAttackSuccessChance;
        }
    }

    public void TakeHit(float damage)
    {
        health -= damage;
        if(health <= 0)
            isDead = true;
    }

    public float CalculateDamage()
    {
        int dice = Random.Range(1, 21);

        // Critical success
        if(dice >= minCriticalValue)
        {
            HUD.instance.SetInfo("Acerto crítico de " + name + "! Dano total: " + 2 * damage);
            return 2 * damage;
        }

        // Success
        else if(dice >= attackSuccessPercent)
        {
            HUD.instance.SetInfo("Acerto de " + name + ". Dano total: " + damage);
            return damage;
        }

        // Fail
        else
        {
            HUD.instance.SetInfo(name + " errou!");
            return 0;
        }
    }

    public void Attack()
    {
        float finalDamage = CalculateDamage();
        Bossfight.instance.GetEnemy().TakeHit(finalDamage);
        Bossfight.instance.NextRound();
    }

    public void Heal()
    {
        health = Mathf.Clamp(health + basicHeal, 0, baseHealth);
        Bossfight.instance.NextRound();
        HUD.instance.SetInfo(name + " curou!");
    }

    public void TakeDecision()
    {
        StartCoroutine(WaitForTakeDecision());
        
        // For now, let's make the AI just attack back
        //DecisionMaker();
    }

    IEnumerator WaitForTakeDecision()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        // For now, let's make the AI just attack back
        DecisionMaker();
    }

    private void DecisionMaker()
    {
        int dice = Random.Range(1, 21);
        if(dice >= attackSuccessChance)
        {
            Attack();
        }
        else
        {
            Heal();
        }
    }
}
