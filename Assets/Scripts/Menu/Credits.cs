using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    // Fade
    public GameObject fadeCanvas;
    public Animation fade;


    // Load main menu
    public void LoadMainMenu()
    {
        fadeCanvas.SetActive(true);
        StartCoroutine(LoadMainMenuAsync());
    }

    // Load main menu async
    IEnumerator LoadMainMenuAsync()
    {
        fade.Play();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
