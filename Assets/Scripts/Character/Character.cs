using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public bool isDead = false;
    public bool isPlayer = false;

    // Damage
    [Header("Damage")]
    public float basicDamage;
    public float magicDamage;
    public float basicArmor;
    public float magicArmor;

    // Attack
    [Header("Attack")]
    [Range(1, 20)] public int basicAttackSuccessDice = 6;  // Minimal value for basic attack success
    [Range(1, 20)] public int magicAttackSuccessDice = 6;  // Minimal value for magic attack success
    [Range(1, 20)] public int minCriticalValue = 20;

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
        InitialSetup();
    }

    public void InitialSetup()
    {
        baseHealth = health;
        attackSuccessChance = minAttackSuccessChance;
    }

    #region Take Hit
    public void TakeHit(float damage, float armor)
    {
        health -= Mathf.Clamp(damage - armor, 0f, damage);
        CheckDeath(damage);
    }

    private void CheckDeath(float damage)
    {
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
    #endregion

    #region Damage
    public float CalculateDamage(float damage, float attackSuccessDice)
    {
        int dice = Random.Range(1, 21);

        // Critical success
        if(dice >= minCriticalValue)
        {
            HUD.instance.SetInfo(name + "'s critical hit! Total damage: " + 2 * damage);
            return 2 * damage;
        }

        // Success
        else if(dice >= attackSuccessDice)
        {
            HUD.instance.SetInfo(name + "'s successfull hit. Total damage: " + damage);
            return damage;
        }

        // Fail
        else
        {
            PlayAudioFX(missClip);
            HUD.instance.SetInfo(name + " missed!");
            return 0;
        }
    }

    public void BasicAttack()
    {
        PlayAudioFX(attackClip);
        HUD.instance.DisablePlayerActions();
        float enemyArmor = Bossfight.instance.GetEnemy().basicArmor;
        StartCoroutine(AttackAnimation(basicDamage, enemyArmor, basicAttackSuccessDice));
    }

    public void MagicAttack()
    {
        PlayAudioFX(attackClip);
        HUD.instance.DisablePlayerActions();
        float enemyArmor = Bossfight.instance.GetEnemy().magicArmor;
        StartCoroutine(AttackAnimation(magicDamage, enemyArmor, magicAttackSuccessDice));
    }

    IEnumerator AttackAnimation(float damage, float armor, float attackSuccessDice)
    {
        animator.SetBool("isAttacking", true);
        float length = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        yield return new WaitForSeconds(length);

        float finalDamage = CalculateDamage(damage, attackSuccessDice);
        Bossfight.instance.GetEnemy().TakeHit(finalDamage, armor);

        Bossfight.instance.NextRound();
        animator.SetBool("isAttacking", false);
    }
    #endregion

    #region Heal
    public void Heal()
    {
        PlayAudioFX(healClip);
        health = Mathf.Clamp(health + basicHeal, 0, baseHealth);
        Bossfight.instance.NextRound();
        HUD.instance.SetInfo(name + " healed!");
        StartCoroutine(HealAnimation());
    }

    IEnumerator HealAnimation()
    {
        animator.SetBool("healed", true);
        float length = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(length);
        animator.SetBool("healed", false);
    }
    #endregion

    #region Decision
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
            BasicAttack();
        }
        else
        {
            Heal();
        }
    }
    #endregion

    private void PlayAudioFX(AudioClip clip)
    {
        characterAudioSource.clip = clip;
        characterAudioSource.Play();
    }
}
