using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapMenu : MonoBehaviour
{
    #region Singleton
    public static MapMenu instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    #endregion

    // Fade
    public GameObject fadeCanvas;
    public Animation fadeAnimation;
    public int fadeInIndex;
    public int fadeOutIndex;

    // Map player
    public List<float> levelPostionsX;
    public RectTransform playerRectTransform;

    // Level index
    public int index = 0;

    // Start level button
    public Button startButton;


    // Previous level index
    private int previousIndex;

    // Map player movement controllers
    private float time;
    private float timeToReachTarget = 2f;
    private Vector3 origin;
    private Vector3 target;


    // Update method
    void Update()
    {
        // If there is no player or player is already in place, exit
        if(playerRectTransform == null || playerRectTransform.localPosition == target)
        {
            return;
        }

        // Update map player position
        time += Time.deltaTime / timeToReachTarget;
        playerRectTransform.localPosition = Vector3.Lerp(origin, target, time);
    }
    // Increment level index
    public void IncrementLevelIndex()
    {
        previousIndex = index;
        index++;
    }

    #region On Scene Load
    // Called on scene load
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Fade screen
        fadeCanvas.SetActive(true);
        StartCoroutine(FadeOutBlackScreen());

        // Try to find map player and start level button
        FindPlayerRectTransform();
        FindStartButton();

        // Call PlayerStats.OnSceneLoaded method
        PlayerStats.instance.OnSceneLoaded();
    }

    // Try to find map player
    private void FindPlayerRectTransform()
    {
        // Try to find map player by tag and get the RectTransform component
        GameObject mapPlayer = GameObject.FindGameObjectWithTag("MapPlayer");
        playerRectTransform = !mapPlayer ? null : mapPlayer.GetComponent<RectTransform>();

        // If the RectTransform does not exist, exit
        if(!playerRectTransform)
        {
            return;
        }

        // Set map player origin
        float x = levelPostionsX[previousIndex];
        float y = playerRectTransform.localPosition.y;
        float z = playerRectTransform.localPosition.z;
        origin = new Vector3(x, y, z);

        // Set map player target
        x = levelPostionsX[index];
        target = new Vector3(x, y, z);

        // Reset map player movement timer
        time = 0f;
    }

    // Trye to find start level button
    private void FindStartButton()
    {
        // Try to find button game object
        GameObject mapButton = GameObject.FindGameObjectWithTag("MapStartButton");

        // If success, add a listener for LoadLevel method
        if(mapButton)
        {
            startButton = mapButton.GetComponent<Button>();
            startButton.onClick.AddListener(LoadLevel);
        }

        // Else, clear start button reference
        else
        {
            startButton = null; 
        }
    }
    #endregion

    #region Level Load
    // Load map level
    public void LoadMap()
    {
        fadeCanvas.SetActive(true);
        StartCoroutine(LoadMapAsync());
    }

    // Load next level to play
    public void LoadLevel()
    {
        fadeCanvas.SetActive(true);
        int levelNumber = index + 1;
        StartCoroutine(FadeInBlackScreen("Level_" + levelNumber));
    }

    public void LoadCredits()
    {
        fadeCanvas.SetActive(true);
        StartCoroutine(FadeInBlackScreen("Credits"));
    }
    #endregion

    #region IEnumerators
    // Fade in black screen and start loading level async
    IEnumerator FadeInBlackScreen(string levelName)
    {
        AnimationClip clip = AnimationHelper.GetAnimationClipByIndex(fadeAnimation, fadeInIndex);
        fadeAnimation.Play(clip.name);
        yield return new WaitForSeconds(clip.length);
        StartCoroutine(LoadLevelAsync(levelName));
    }

    // Fade out black screen
    IEnumerator FadeOutBlackScreen()
    {
        AnimationClip clip = AnimationHelper.GetAnimationClipByIndex(fadeAnimation, fadeOutIndex);
        fadeAnimation.Play(clip.name);
        yield return new WaitForSeconds(clip.length);
        fadeCanvas.SetActive(false);
    }

    // Load level async
    IEnumerator LoadLevelAsync(string levelName)
    {
        
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelName);
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    // Load map async
    IEnumerator LoadMapAsync()
    {
        // Fade in black screen
        AnimationClip clip = AnimationHelper.GetAnimationClipByIndex(fadeAnimation, fadeInIndex);
        fadeAnimation.Play(clip.name);
        yield return new WaitForSeconds(clip.length);

        // Load map async
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Map");
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
    }
    #endregion
}
