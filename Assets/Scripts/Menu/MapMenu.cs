using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    private int previousIndex;
    private float time;
    private float timeToReachTarget = 2f;
    private Vector3 origin;
    private Vector3 target;


    void Update()
    {
        if(!playerRectTransform || playerRectTransform.localPosition == target)
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

        GameObject[] mapPlayer = GameObject.FindGameObjectsWithTag("MapPlayer");
        playerRectTransform = mapPlayer.Length == 0 ? null : mapPlayer[0].GetComponent<RectTransform>();
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
        previousIndex = index;
        index++;
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
