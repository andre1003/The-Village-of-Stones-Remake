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

    public GameObject fadeCanvas;
    public Animation fadeAnimation;
    public AnimationClip fadeIn;
    public AnimationClip fadeOut;

    public List<float> levelPostionsX;
    public RectTransform playerRectTransform;

    public int index = 0;

    public Button startButton;


    private int previousIndex;
    private float time;
    private float timeToReachTarget = 2f;
    private Vector3 origin;
    private Vector3 target;


    void Update()
    {
        if(playerRectTransform == null || playerRectTransform.localPosition == target)
        {
            return;
        }

        time += Time.deltaTime / timeToReachTarget;
        playerRectTransform.localPosition = Vector3.Lerp(origin, target, time);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fadeCanvas.SetActive(true);
        StartCoroutine(FadeInScreen());
        FindPlayerRectTransform();
        FindStartButton();
    }

    private void FindPlayerRectTransform()
    {
        GameObject mapPlayer = GameObject.FindGameObjectWithTag("MapPlayer");
        playerRectTransform = !mapPlayer ? null : mapPlayer.GetComponent<RectTransform>();
        if(!playerRectTransform)
        {
            return;
        }

        float x = levelPostionsX[previousIndex];
        float y = playerRectTransform.localPosition.y;
        float z = playerRectTransform.localPosition.z;
        origin = new Vector3(x, y, z);

        x = levelPostionsX[index];
        target = new Vector3(x, y, z);

        time = 0f;
    }

    private void FindStartButton()
    {
        GameObject mapButton = GameObject.FindGameObjectWithTag("MapStartButton");
        if(mapButton)
        {
            startButton = mapButton.GetComponent<Button>();
            startButton.onClick.AddListener(LoadLevel);
        }
        else
        {
            startButton = null; 
        }
    }

    public void LoadMap()
    {
        fadeCanvas.SetActive(true);
        StartCoroutine(LoadMapAsync());
    }

    public void LoadLevel()
    {
        fadeCanvas.SetActive(true);
        StartCoroutine(FadeOutScreen());
    }

    public void IncrementLevelIndex()
    {
        previousIndex = index;
        index++;
    }

    IEnumerator FadeOutScreen()
    {
        fadeAnimation.clip = fadeOut;
        fadeAnimation.Play();
        yield return new WaitForSeconds(fadeOut.length);
        StartCoroutine(LoadLevelAsync());
    }

    IEnumerator FadeInScreen()
    {
        fadeAnimation.clip = fadeIn;
        fadeAnimation.Play();
        yield return new WaitForSeconds(fadeIn.length);
        fadeCanvas.SetActive(false);
    }

    IEnumerator LoadLevelAsync()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index + 1);
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadMapAsync()
    {
        fadeAnimation.clip = fadeOut;
        fadeAnimation.Play();
        yield return new WaitForSeconds(fadeOut.length);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Map");
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}
