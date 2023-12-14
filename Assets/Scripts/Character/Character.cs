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
    public bool isDead = false;
    public bool isPlayer = false;
    public bool isInvencible = false;

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

    // Stones
    [Header("Stones")]
    public List<Stone> stones = new List<Stone>();

    // Animation
    [Header("Animation")]
    public Animator animator;

    // Audio
    [Header("Audio")]
    public AudioSource characterAudioSource;
    public AudioClip attackClip;
    public AudioClip healClip;
    public AudioClip missClip;
    public AudioClip deathClip;

    // Decision
    [Header("Decision")]
    [Range(1, 20)] public int attackSuccessChance;
    [Range(1, 20)] public int maxAttackSuccessChance = 10;
    [Range(1, 20)] public int minAttackSuccessChance = 2;


    // Status
    private float baseHealth;


    // Start method
    void Start()
    {
        InitialSetup();
    }

    // Initial setup
    public void InitialSetup()
    {
        baseHealth = health;
        attackSuccessChance = minAttackSuccessChance;
    }

    // Get baseHealth value
    public float GetBaseHealth()
    {
        return baseHealth;
    }

    #region Take Hit
    // Take hit
    public void TakeHit(float damage, float armor)
    {
        // If character is invencible, exit
        if(isInvencible)
        {
            return;
        }

        // Decrease health and check death
        health -= Mathf.Clamp(damage - armor, 0f, damage);
        CheckDeath(damage);
    }

    // Check character's death
    private void CheckDeath(float damage)
    {
        // If health is less or equal to 0, character is dead
        if(health <= 0)
        {
            isDead = true;
            PlayAudioFX(deathClip);
        }

        // Else, just play hit animation
        else if(damage > 0f)
        {
            StartCoroutine(TakeHitAnimation());
        }
    }

    // Take hit animation
    IEnumerator TakeHitAnimation()
    {
        animator.SetBool("tookHit", true);
        float length = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(length);
        animator.SetBool("tookHit", false);
    }
    #endregion

    #region Damage
    // Calculate damage
    public float CalculateDamage(float damage, float attackSuccessDice)
    {
        // Get a random value between 1 and 20 (1d20 roll)
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

    // Perform a basic attack
    public void BasicAttack()
    {
        // Play attack audio and disable player actions
        PlayAudioFX(attackClip);
        HUD.instance.DisablePlayerActions();

        // Check if enemy is invencible and if it is, exit
        Character enemy = Bossfight.instance.GetEnemy();
        if(enemy.isInvencible)
        {
            HUD.instance.SetInfo(enemy.name + " is invencible! No damage caused.");
            Bossfight.instance.NextRound();
            return;
        }

        // If enemy is NOT invencible, continue basic attack
        float enemyArmor = enemy.basicArmor;
        StartCoroutine(AttackAnimation(basicDamage, enemyArmor, basicAttackSuccessDice));
    }

    // Perform a magic attack
    public void MagicAttack()
    {
        // Play attack audio and disable player actions
        PlayAudioFX(attackClip);
        HUD.instance.DisablePlayerActions();

        // Check if enemy is invencible and if it is, exit
        Character enemy = Bossfight.instance.GetEnemy();
        if(enemy.isInvencible)
        {
            HUD.instance.SetInfo(enemy.name + " is invencible! No damage caused.");
            Bossfight.instance.NextRound();
            return;
        }

        // If enemy is NOT invencible, continue magic attack
        float enemyArmor = enemy.magicArmor;
        StartCoroutine(AttackAnimation(magicDamage, enemyArmor, magicAttackSuccessDice));
    }

    // Attack animation
    IEnumerator AttackAnimation(float damage, float armor, float attackSuccessDice)
    {
        // Set attack animation
        animator.SetBool("isAttacking", true);
        float length = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        // Wait animation to end
        yield return new WaitForSeconds(length);

        // Calculate damage and inflinge it to enemy
        float finalDamage = CalculateDamage(damage, attackSuccessDice);
        Bossfight.instance.GetEnemy().TakeHit(finalDamage, armor);

        // Call next round
        Bossfight.instance.NextRound();
        animator.SetBool("isAttacking", false);
    }
    #endregion

    #region Heal
    // Heal character
    public void Heal()
    {
        PlayAudioFX(healClip);
        health = Mathf.Clamp(health + basicHeal, 0, baseHealth);
        Bossfight.instance.NextRound();
        HUD.instance.SetInfo(name + " healed!");
        StartCoroutine(HealAnimation());
    }

    // Heal character a certain amount of points
    public void Heal(float heal)
    {
        PlayAudioFX(healClip);
        health = Mathf.Clamp(health + heal, 0, baseHealth);
        Bossfight.instance.NextRound();
        HUD.instance.SetInfo(name + " healed!");
        StartCoroutine(HealAnimation());
    }

    public void FullHeal()
    {
        PlayAudioFX(healClip);
        health = baseHealth;
        Bossfight.instance.NextRound();
        HUD.instance.SetInfo(name + " fully healed!");
        StartCoroutine(HealAnimation());
    }

    // Heal animation
    IEnumerator HealAnimation()
    {
        animator.SetBool("healed", true);
        float length = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(length);
        animator.SetBool("healed", false);
    }
    #endregion

    #region Decision
    // Adapt attack success chance for AI, based on character's health
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

    // Take decision
    public void TakeDecision()
    {
        StartCoroutine(WaitForTakeDecision());
    }

    // Delayed take decision
    IEnumerator WaitForTakeDecision()
    {
        // Wait 1 to 3 seconds before take decision
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        // For now, let's make the AI just attack back
        if(Bossfight.instance.isFighting)
        {
            DecisionMaker();
        }
    }

    // AI round decision maker
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

    #region Buffs and Debuffs
    // Buff character damage
    public void BuffDamage(float basicDamageBuff, float magicDamageBuff)
    {
        basicDamage += basicDamageBuff;
        magicDamage += magicDamageBuff;
    }

    // Debuff character damage
    public void DebuffDamage(float basicDamageBuff, float magicDamageBuff)
    {
        basicDamage -= basicDamageBuff;
        magicDamage -= magicDamageBuff;
    }

    // Buff character armor
    public void BuffArmor(float basicArmorBuff, float magicArmorBuff)
    {
        basicArmor += basicArmorBuff;
        magicArmor += magicArmorBuff;
    }

    // Debuff character armor
    public void DebuffArmor(float basicArmorBuff, float magicArmorBuff)
    {
        basicArmor -= basicArmorBuff;
        magicArmor -= magicArmorBuff;
    }
    #endregion

    // Play new audio clip
    private void PlayAudioFX(AudioClip clip)
    {
        characterAudioSource.clip = clip;
        characterAudioSource.Play();
    }

    // Update all stones cooldown
    public void UpdateAllStonesCooldown()
    {
        foreach(var stone in stones)
        {
            stone.UpdateCooldown();
        }
    }

    // Use a stone
    public void UseStone(int stoneIndex)
    {
        // Index is out of bounds handler
        if(stoneIndex < 0 || stoneIndex >= stones.Count)
        {
            return;
        }
        HUD.instance.DisablePlayerActions();
        stones[stoneIndex].Use(this, Bossfight.instance.GetEnemy());
        Bossfight.instance.NextRound();
    }
}
