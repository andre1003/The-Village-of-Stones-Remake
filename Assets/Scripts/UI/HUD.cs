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
    public GameObject hudCanvas;

    // Fight end canvases
    public Canvas gameOver;
    public Canvas playerWin;

    // Turn info
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI currentCharacterText;
    public TextMeshProUGUI turnInfoText;

    // Health
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI enemyHealthText;

    // Player actions
    public List<Button> playerActions;

    // Healths
    public Slider playerHealth;
    public Slider enemyHealth;
    public List<GameObject> healthObjects;


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

    public void PlayerWins()
    {
        playerWin.enabled = true;
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
}
