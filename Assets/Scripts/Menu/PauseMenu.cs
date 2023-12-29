using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public Fader pauseMenuFader;

    private bool isPaused = false;
    private bool hudPreviousVisibility = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
                ResumeGame();
            else
                PauseGame();
        }
            
    }

    public void PauseGame()
    {
        StartCoroutine(WaitForPauseGame());
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void ResumeGame()
    {
        StartCoroutine(WaitForUnPauseGame());
    }

    IEnumerator WaitForPauseGame()
    {
        isPaused = true;
        hudPreviousVisibility = HUD.instance ? HUD.instance.hudCanvas.activeSelf : false;
        HUD.instance?.SetHUDVisibility(false);
        pauseMenu.SetActive(true);
        pauseMenuFader.FadeIn();
        yield return new WaitForFade(pauseMenuFader);
        Time.timeScale = 0f;
    }

    IEnumerator WaitForUnPauseGame()
    {
        pauseMenuFader.FadeOut();
        Time.timeScale = 1f;
        yield return new WaitForFade(pauseMenuFader);
        HUD.instance?.SetHUDVisibility(hudPreviousVisibility);
        pauseMenu.SetActive(false);
        isPaused = false;
    }
}
