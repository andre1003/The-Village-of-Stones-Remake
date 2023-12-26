using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCharacters : MonoBehaviour
{
    [Header("References")]
    public List<Character> enemies;
    public Sprite heroSprite;
    public List<Sprite> enemySprites;

    [Header("Main UI")]
    public GameObject displayCharactersCanvas;
    public Fader displayCharacterFader;

    [Header("Hero UI")]
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI heroDamageText;
    public TextMeshProUGUI heroArmorText;
    public TextMeshProUGUI heroHealText;
    public Image heroImage;

    [Header("Enemy UI")]
    public TextMeshProUGUI enemyNameText;
    public TextMeshProUGUI enemyDamageText;
    public TextMeshProUGUI enemyArmorText;
    public TextMeshProUGUI enemyHealText;
    public Image enemyImage;


    public void Display()
    {
        ActivateMainCanvas();

        heroNameText.text = PlayerStats.instance.name;
        heroNameText.text += "\n<size=50%><color=#8C8C8C>" + PlayerStats.instance.description + "</color></size>";
        heroDamageText.text = "Basic damage: " + PlayerStats.instance.basicDamage + "\nMagic damage: " + PlayerStats.instance.magicDamage;
        heroArmorText.text = "Basic armor: " + PlayerStats.instance.basicArmor + "\nMagic armor: " + PlayerStats.instance.magicArmor;
        heroHealText.text = "Heal: " + PlayerStats.instance.basicHeal;
        heroImage.sprite = heroSprite;

        Character enemy = enemies[MapMenu.instance.index];
        enemyNameText.text = enemy.name;
        enemyNameText.text += "\n<size=50%><color=#8C8C8C>" + enemy.description + "</color></size>";
        enemyDamageText.text = "Basic damage: " + enemy.basicDamage + "\nMagic damage: " + enemy.magicDamage;
        enemyArmorText.text = "Basic armor: " + enemy.basicArmor + "\nMagic armor: " + enemy.magicArmor;
        enemyHealText.text = "Heal: " + enemy.basicHeal;
        enemyImage.sprite = enemySprites[MapMenu.instance.index];
    }

    public void Hide()
    {
        StartCoroutine(WaitForHide());
    }

    IEnumerator WaitForHide()
    {
        displayCharacterFader.FadeOut();
        yield return new WaitForFade(displayCharacterFader);
        displayCharactersCanvas.SetActive(false);
    }

    private void ActivateMainCanvas()
    {
        if(displayCharactersCanvas.activeSelf)
            return;

        displayCharactersCanvas.SetActive(true);
        displayCharacterFader.FadeIn();

    }
}
