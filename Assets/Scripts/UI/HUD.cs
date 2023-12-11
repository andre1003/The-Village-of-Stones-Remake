using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    #region Singleton
    public static HUD instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion

    // HUD canvas
    [Header("HUD")]
    public GameObject hudCanvas;

    // Fight end canvases
    [Header("Fight outcomes")]
    public Canvas gameOver;
    public GameObject getStone;

    // Turn info
    [Header("Turn info")]
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI currentCharacterText;
    public TextMeshProUGUI turnInfoText;

    // Health
    [Header("Health")]
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI enemyHealthText;
    public Slider playerHealth;
    public Slider enemyHealth;
    public List<GameObject> healthObjects;

    // Player actions
    [Header("Player actions")]
    public List<Button> playerActions;

    // Animation
    [Header("Get stone")]
    public Animation getStoneAnimation;
    public AnimationClip getStoneFadeIn;
    public AnimationClip getStoneFadeOut;


    private float basePlayerHealth;
    private float baseEnemyHealth;
    private bool isPlayerTurn;
    

    void Start()
    {
        InitialSetup();
        isPlayerTurn = Bossfight.instance.IsPlayerTurn();
    }

    void Update()
    {
        currentCharacterText.text = Bossfight.instance.GetCurrentTurnCharacterName();
        playerHealthText.text = Bossfight.instance.characters[0].health.ToString();
        enemyHealthText.text = Bossfight.instance.characters[1].health.ToString();
        playerHealth.value = Bossfight.instance.characters[0].health / basePlayerHealth;
        enemyHealth.value = Bossfight.instance.characters[1].health / baseEnemyHealth;

        if(isPlayerTurn != Bossfight.instance.IsPlayerTurn())
        {
            UpdateActionsInteractable();
        }
    }

    private void UpdateActionsInteractable()
    {
        isPlayerTurn = Bossfight.instance.IsPlayerTurn();
        foreach(Button action in playerActions)
        {
            action.interactable = Bossfight.instance.IsPlayerTurn();
        }
    }

    private void InitialSetup()
    {
        // Reset texts
        turnInfoText.text = "";
        turnText.text = "1";

        // Setup player and enemy health
        basePlayerHealth = Bossfight.instance.characters[0].health;
        baseEnemyHealth = Bossfight.instance.characters[1].health;
        playerHealth.value = 1;
        enemyHealth.value = 1;
    }

    public void GetStone()
    {
        StartCoroutine(GetStoneAnimation());
    }

    IEnumerator GetStoneAnimation()
    {
        getStone.SetActive(true);
        getStoneAnimation.clip = getStoneFadeIn;
        getStoneAnimation.Play();
        yield return new WaitForSeconds(13f);
        GameFlow.instance.Map();
    }

    public void GameOver()
    {
        gameOver.enabled = true;
    }

    public void SetInfo(string info)
    {
        turnInfoText.text = info;
    }

    public void SetTurnText(string turn)
    {
        turnText.text = turn;
    }

    public void SetHUDVisibility(bool visibility)
    {
        hudCanvas.SetActive(visibility);
        foreach(GameObject healthObject in healthObjects)
        {
            healthObject.SetActive(visibility);
        }
    }

    public void DisablePlayerActions()
    {
        foreach(Button action in playerActions)
        {
            action.interactable = false;
        }
    }
}
