using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // Audio
    public AudioSource track01;
    public AudioSource track02;
    public AudioClip defaultClip;

    // Time to fade tracks
    public float timeToFade = 1f;
    public float timeToFadeOut = 2f;
    public float timeToFadeIn = 2f;


    // Track controller
    private bool isPlayingTrack01;


    // Start method
    void Start()
    {
        isPlayingTrack01 = false;
        SwapTrack(defaultClip);
    }

    // Set new clip
    public void SwapTrack(AudioClip newClip, bool loop = true)
    {
        StopAllCoroutines();
        SetLoop(loop);
        if(isPlayingTrack01)
            StartCoroutine(FadeTrack(newClip, track02, track01));
        else
            StartCoroutine(FadeTrack(newClip, track01, track02));
        isPlayingTrack01 = !isPlayingTrack01;
    }

    // Return to default clip
    public void ReturnToDefault()
    {
        SwapTrack(defaultClip);
    }

    // Smoothly turn off the sound
    public void TurnOffSound()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutStopTracks());
    }

    // Smoothly turn on the sound
    public void TurnOnSound()
    {
        StopAllCoroutines();
        if(isPlayingTrack01)
        {
            StartCoroutine(FadeInPlayTrack(track01));
        }
        else
        {
            StartCoroutine(FadeInPlayTrack(track02));
        }
    }

    // Pause all sounds
    public void PauseSound()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutPauseTracks());
    }

    // Unpause sound
    public void UnPauseSound()
    {
        StopAllCoroutines();
        if(isPlayingTrack01)
        {
            track01.UnPause();
        }
        else
        {
            track02.UnPause();
        }
    }

    // Set loop value
    public void SetLoop(bool loop)
    {
        track01.loop = loop;
        track02.loop = loop;
    }

    // Fade between tracks
    IEnumerator FadeTrack(AudioClip newClip, AudioSource turnOnTrack, AudioSource turnOffTrack)
    {
        float timeElapsed = 0f;

        turnOnTrack.clip = newClip;
        turnOnTrack.Play();

        while(timeElapsed < timeToFade)
        {
            turnOnTrack.volume = Mathf.Lerp(0f, 1f, timeElapsed / timeToFade);
            turnOffTrack.volume = Mathf.Lerp(1f, 0f, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        turnOnTrack.volume = 1f;
        turnOffTrack.volume = 0f;
        turnOffTrack.Stop();
    }

    // Fade out and stop both tracks
    IEnumerator FadeOutStopTracks()
    {
        float timeElapsed = 0f;
        while(timeElapsed < timeToFadeOut)
        {
            track01.volume = Mathf.Lerp(1f, 0f, timeElapsed / timeToFadeOut);
            track02.volume = Mathf.Lerp(1f, 0f, timeElapsed / timeToFadeOut);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        track01.volume = 0f;
        track02.volume = 0f;
        track01.Stop();
        track02.Stop();
    }

    // Fade out and pause both tracks
    IEnumerator FadeOutPauseTracks()
    {
        float timeElapsed = 0f;
        while(timeElapsed < timeToFadeOut)
        {
            track01.volume = Mathf.Lerp(1f, 0f, timeElapsed / timeToFadeOut);
            track02.volume = Mathf.Lerp(1f, 0f, timeElapsed / timeToFadeOut);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        track01.volume = 0f;
        track02.volume = 0f;
        track01.Pause();
        track02.Pause();
    }

    // Fade in and play a given track
    IEnumerator FadeInPlayTrack(AudioSource track)
    {
        float timeElapsed = 0f;
        track.Play();
        while(timeElapsed < timeToFadeIn)
        {
            track.volume = Mathf.Lerp(0f, 1f, timeElapsed / timeToFadeIn);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        track.volume = 1f;
    }
}
