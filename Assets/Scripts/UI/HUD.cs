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
    public GameObject gameOver;
    public Animation gameOverAnimation;
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
    public RectTransform stoneButtonRectTrasform;
    public GameObject stoneSelectionPrefab;

    // Animation
    [Header("Get stone")]
    public Animation getStoneAnimation;
    public AnimationClip getStoneFadeIn;
    public AnimationClip getStoneFadeOut;


    // Private attributes
    private float basePlayerHealth;
    private float baseEnemyHealth;
    private bool isPlayerTurn;
    private int stonesNumber = 0;
    private List<GameObject> stoneSelections = new List<GameObject>();
    

    // Start method
    void Start()
    {
        InitialSetup();
        isPlayerTurn = Bossfight.instance.IsPlayerTurn();
    }

    // Update method
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

    // Update player actions interactable status based on current character turn
    private void UpdateActionsInteractable()
    {
        isPlayerTurn = Bossfight.instance.IsPlayerTurn();
        foreach(Button action in playerActions)
        {
            action.interactable = Bossfight.instance.IsPlayerTurn();
        }
    }

    // Initial HUD setup
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

    public void ResetForNewFight()
    {
        // Reset texts
        turnInfoText.text = "";
        turnText.text = "1";

        baseEnemyHealth = Bossfight.instance.characters[1].health;
        enemyHealth.value = 1;
    }

    // Get stone handler
    public void GetStone()
    {
        StartCoroutine(GetStoneAnimation());
    }

    // Give stone to player animation
    IEnumerator GetStoneAnimation()
    {
        getStone.SetActive(true);
        getStoneAnimation.clip = getStoneFadeIn;
        getStoneAnimation.Play();

        yield return new WaitForSeconds(13f);

        if(!GameFlow.instance.isFinalLevel)
        {
            GameFlow.instance.Map();
        }
        else
        {
            getStoneAnimation.clip = getStoneFadeOut;
            getStoneAnimation.Play();

            yield return new WaitForSeconds(getStoneFadeOut.length);
            DialogueManager.instance.SetCanNextSentence(true);
            getStone.SetActive(false);
        }
    }

    // Game over handler
    public void GameOver()
    {
        gameOver.SetActive(true);
        gameOverAnimation.Play();
    }

    // Set turn info text
    public void SetInfo(string info)
    {
        turnInfoText.text = info;
    }

    // Set turn text
    public void SetTurnText(string turn)
    {
        turnText.text = turn;
    }

    // Set HUD visibility
    public void SetHUDVisibility(bool visibility)
    {
        hudCanvas.SetActive(visibility);
        foreach(GameObject healthObject in healthObjects)
        {
            healthObject.SetActive(visibility);
        }
    }

    // Disable all player actions
    public void DisablePlayerActions()
    {
        CloseStonesSelection();
        foreach(Button action in playerActions)
        {
            action.interactable = false;
        }
    }

    // Open stone selection submenu
    private void OpenStonesSelection()
    {
        // Loop stones
        List<Stone> stones = Bossfight.instance.GetPlayer().stones;
        foreach(Stone stone in stones)
        {
            // Instantiate the stone selection button and disable it for it's setup
            stonesNumber++;
            GameObject stoneSelection = Instantiate(stoneSelectionPrefab, stoneButtonRectTrasform.transform.parent);
            stoneSelection.SetActive(false);

            // Set local position and button text
            stoneSelection.GetComponent<RectTransform>().localPosition = stoneButtonRectTrasform.localPosition
                + new Vector3(0f, stonesNumber * stoneButtonRectTrasform.rect.height, 0f);
            stoneSelection.GetComponentInChildren<TextMeshProUGUI>().text = stone.name;

            // Add listener to UseStone method. Because the method have a param, we need to use a delegate
            // to make the method callback when the button is clicked
            int stoneIndex = stonesNumber - 1;
            Button stoneButton = stoneSelection.GetComponent<Button>();
            stoneButton.onClick.AddListener(
                delegate { Bossfight.instance.GetPlayer().UseStone(stoneIndex); });

            // Set stone button interactable
            stoneButton.interactable = stone.CanUseStone();

            // Activate the button and add it to stone selection list
            stoneSelection.SetActive(true);
            stoneSelections.Add(stoneSelection);
        }
    }

    // Close stone selection submenu
    private void CloseStonesSelection()
    {
        // Destroy all stone selection button, clear buttons list and reset stones number
        foreach(GameObject stoneSelection in stoneSelections)
        {
            Destroy(stoneSelection);
        }
        stoneSelections.Clear();
        stonesNumber = 0;
    }

    // Stone selection submenu handler
    public void StoneSelectionHandler()
    {
        // If there are stone buttons, close stone selection submenu
        if(stoneSelections.Count > 0)
        {
            CloseStonesSelection();
        }

        // Else, open stone selection submenu
        else
        {
            OpenStonesSelection();
        }
    }
}
