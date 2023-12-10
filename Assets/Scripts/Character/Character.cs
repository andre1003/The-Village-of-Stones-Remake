using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Animation
    [Header("Animation")]
    public Animator animator;

    // Audio
    [Header("Audio")]
    public AudioSource characterAudioSource;
    public AudioClip attackClip;
    public AudioClip healClip;
    public AudioClip missClip;

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
        else if(damage > 0f)
            StartCoroutine(TakeHitAnimation());
    }

    IEnumerator TakeHitAnimation()
    {
        animator.SetBool("tookHit", true);
        float length = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(length);
        animator.SetBool("tookHit", false);
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
            PlayAudioFX(missClip);
            HUD.instance.SetInfo(name + " errou!");
            return 0;
        }
    }

    public void Attack()
    {
        PlayAudioFX(attackClip);
        HUD.instance.DisablePlayerActions();
        StartCoroutine(AttackAnimation());
    }

    IEnumerator AttackAnimation()
    {
        animator.SetBool("isAttacking", true);
        float length = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(length);
        float finalDamage = CalculateDamage();
        Bossfight.instance.GetEnemy().TakeHit(finalDamage);
        Bossfight.instance.NextRound();
        animator.SetBool("isAttacking", false);
    }


    public void Heal()
    {
        PlayAudioFX(healClip);
        health = Mathf.Clamp(health + basicHeal, 0, baseHealth);
        Bossfight.instance.NextRound();
        HUD.instance.SetInfo(name + " curou!");
        StartCoroutine(HealAnimation());
    }

    IEnumerator HealAnimation()
    {
        animator.SetBool("healed", true);
        float length = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(length);
        animator.SetBool("healed", false);
    }

    public void TakeDecision()
    {
        StartCoroutine(WaitForTakeDecision());
    }


    IEnumerator WaitForTakeDecision()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        // For now, let's make the AI just attack back
        if(Bossfight.instance.isFighting)
        {
            DecisionMaker();
        }
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

    private void PlayAudioFX(AudioClip clip)
    {
        characterAudioSource.clip = clip;
        characterAudioSource.Play();
    }
}
