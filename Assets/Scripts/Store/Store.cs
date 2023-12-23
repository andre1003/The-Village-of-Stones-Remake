using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Store : MonoBehaviour
{
    #region Singleton
    public static Store instance;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    // Is first time
    [Header("First time")]
    [SerializeField] private bool isFirstTime = true;
    public GameObject firstTimeCanvas;
    public Fader firstTimeFader;

    // Buffs per level
    [Header("Buffs per level")]
    public float basicArmorBuffPerLevel = 5f;
    public float magicArmorBuffPerLevel = 5f;
    public float basicDamageBuffPerLevel = 10f;
    public float magicDamageBuffPerLevel = 10f;

    // Max level
    [Header("Max level")]
    public int maxLevel = 10;

    // Cost per level
    // Considering an arithmetic progression for this, the player should
    // spend 10.000 coins for max level all attributes.
    [Header("Cost")]
    public float costPerLevel = 50f;
    public float initialCost = 25f;

    // Attributes UI
    [Header("Attributes UI")]
    public TextMeshProUGUI basicArmorLvlText;
    public TextMeshProUGUI magicArmorLvlText;
    public TextMeshProUGUI basicDamageLvlText;
    public TextMeshProUGUI magicDamageLvlText;
    public GameObject storeCanvas;
    public Fader storeFader;

    // Static UI
    [Header("Static UI")]
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI tempCostText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI basicArmorCostText;
    public TextMeshProUGUI magicArmorCostText;
    public TextMeshProUGUI basicDamageCostText;
    public TextMeshProUGUI magicDamageCostText;


    // Temporary cost
    private float tempCost = 0f;


    // Level up method
    public void LevelUp()
    {
        // Get level difference
        int basicArmorLvl = int.Parse(basicArmorLvlText.text) - PlayerStats.instance.basicArmorLvl;
        int magicArmorLvl = int.Parse(magicArmorLvlText.text) - PlayerStats.instance.magicArmorLvl;
        int basicDamageLvl = int.Parse(basicDamageLvlText.text) - PlayerStats.instance.basicDamageLvl;
        int magicDamageLvl = int.Parse(magicDamageLvlText.text) - PlayerStats.instance.magicDamageLvl;

        // Get attributes values to be added
        float addBasicArmor = basicArmorBuffPerLevel * basicArmorLvl;
        float addMagicArmor = magicArmorBuffPerLevel * magicArmorLvl;
        float addBasicDamage = basicDamageBuffPerLevel * basicDamageLvl;
        float addMagicDamage = magicDamageBuffPerLevel * magicDamageLvl;

        // Add attributes values
        PlayerStats.instance.basicArmor += addBasicArmor;
        PlayerStats.instance.magicArmor += addMagicArmor;
        PlayerStats.instance.basicDamage += addBasicDamage;
        PlayerStats.instance.magicDamage += addMagicDamage;

        // Add level
        PlayerStats.instance.basicArmorLvl += basicArmorLvl;
        PlayerStats.instance.magicArmorLvl += magicArmorLvl;
        PlayerStats.instance.basicDamageLvl += basicDamageLvl;
        PlayerStats.instance.magicDamageLvl += magicDamageLvl;

        // Pay the coins
        PlayerStats.instance.coins -= tempCost;
        tempCost = 0f;
        UpdateInfo();
    }

    // Add level of a given attribute
    public void AddLevelOfAttribute(TextMeshProUGUI text)
    {
        int level = int.Parse(text.text) + 1;
        if(level > maxLevel || level > PlayerStats.instance.level)
        {
            return;
        }

        float cost = CalculateLevelCost(level);
        if(tempCost + cost <= PlayerStats.instance.coins)
        {
            tempCost += cost;
            text.text = level.ToString();
            UpdateInfo();
        }
    }

    // Remove level of a given attribute
    public void RemoveLevelOfAttribute(TextMeshProUGUI attribute)
    {
        TextMeshProUGUI text;
        int minLevel = 0;
        if(attribute == basicArmorLvlText)
        {
            text = basicArmorLvlText;
            minLevel = PlayerStats.instance.basicArmorLvl;
        }
        else if(attribute == magicArmorLvlText)
        {
            text = magicArmorLvlText;
            minLevel = PlayerStats.instance.magicArmorLvl;
        }
        else if(attribute == basicDamageLvlText)
        {
            text = basicDamageLvlText;
            minLevel = PlayerStats.instance.basicDamageLvl;
        }
        else if(attribute == magicDamageLvlText)
        {
            text = magicDamageLvlText;
            minLevel = PlayerStats.instance.magicDamageLvl;
        }
        else
        {
            return;
        }
            
        int level = int.Parse(text.text) - 1;
        if(level >= minLevel)
        {
            tempCost -= CalculateLevelCost(level + 1);
            text.text = level.ToString();
            UpdateInfo();
        }
        
    }

    // Open store
    public void OpenStore()
    {
        if(isFirstTime)
        {
            OpenFirstTimeInfo();
            return;
        }

        // Reset all level texts
        basicArmorLvlText.text = PlayerStats.instance.basicArmorLvl.ToString();
        magicArmorLvlText.text = PlayerStats.instance.magicArmorLvl.ToString();
        basicDamageLvlText.text = PlayerStats.instance.basicDamageLvl.ToString();
        magicDamageLvlText.text = PlayerStats.instance.magicDamageLvl.ToString();

        // Enable UI
        storeCanvas.SetActive(true);
        storeFader.FadeIn(0.5f);

        // Update costs
        UpdateInfo();
    }

    // Open first time info
    private void OpenFirstTimeInfo()
    {
        firstTimeCanvas.SetActive(true);
        firstTimeFader.FadeIn(0.5f);
        isFirstTime = false;
        PlayerStats.instance.isStoreFirstTime = false;
    }

    // Reset all info
    public void ResetAllInfo()
    {
        // Reset all level texts
        basicArmorLvlText.text = PlayerStats.instance.basicArmorLvl.ToString();
        magicArmorLvlText.text = PlayerStats.instance.magicArmorLvl.ToString();
        basicDamageLvlText.text = PlayerStats.instance.basicDamageLvl.ToString();
        magicDamageLvlText.text = PlayerStats.instance.magicDamageLvl.ToString();

        // Update costs
        UpdateInfo();
    }

    // Close store
    public void CloseStore()
    {
        StartCoroutine(WaitForDeactivateCanvas());
    }

    // Wait for fade out to disable store canvas
    IEnumerator WaitForDeactivateCanvas()
    {
        storeFader.FadeOut(0.5f);
        yield return new WaitForSeconds(0.6f);
        storeCanvas.SetActive(false);
    }

    // Setter for isFirstTime
    public void SetIsFirstTime(bool isFirstTime)
    {
        this.isFirstTime = isFirstTime;
    }

    // Calculate the cost for a given level
    private float CalculateLevelCost(int level)
    {
        return initialCost * (level - 1) * costPerLevel;
    }

    // Update all costs
    private void UpdateInfo()
    {
        int basicArmorLvl = int.Parse(basicArmorLvlText.text);
        int magicArmorLvl = int.Parse(magicArmorLvlText.text);
        int basicDamageLvl = int.Parse(basicDamageLvlText.text);
        int magicDamageLvl = int.Parse(magicDamageLvlText.text);

        basicArmorCostText.text = basicArmorLvl == maxLevel ? "MAX OUT" : "$ " + CalculateLevelCost(basicArmorLvl).ToString();
        magicArmorCostText.text = magicArmorLvl == maxLevel ? "MAX OUT" : "$ " + CalculateLevelCost(magicArmorLvl).ToString();
        basicDamageCostText.text = basicDamageLvl == maxLevel ? "MAX OUT" : "$ " + CalculateLevelCost(basicDamageLvl).ToString();
        magicDamageCostText.text = magicDamageLvl == maxLevel ? "MAX OUT" : "$ " + CalculateLevelCost(magicDamageLvl).ToString();

        tempCostText.text = tempCost == 0f ? "" : "$ " + tempCost.ToString();
        coinsText.text = "$ " + PlayerStats.instance.coins.ToString();

        levelText.text = "Level: " + PlayerStats.instance.level.ToString();
    }
}
