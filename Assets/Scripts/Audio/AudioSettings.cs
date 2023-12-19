using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    // Audio mixer
    public AudioMixer audioMixer;

    // Audio percentage texts
    public TextMeshProUGUI masterText;
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI uiText;

    // Volume controllers
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider uiSlider;

    // Half volume value
    [Range(-80f, 10f)] public float halfVolumeValue = -40f;


    // Start method
    public void Start()
    {
        // Update audio volumes
        SetMasterVolume(masterSlider.value);
        SetMusicAndFXVolume(musicSlider.value);
        SetUIVolume(uiSlider.value);

        // Set all volume percent texts to current value
        masterText.text = Mathf.RoundToInt(masterSlider.normalizedValue * 100f) + "%";
        musicText.text = Mathf.RoundToInt(musicSlider.normalizedValue * 100f) + "%";
        uiText.text = Mathf.RoundToInt(uiSlider.normalizedValue * 100f) + "%";
    }

    // Set master volume
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        masterText.text = CalculatePercent(volume) + "%";
    }

    // Set music and effects volume
    public void SetMusicAndFXVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        musicText.text = CalculatePercent(volume) + "%";
    }
    
    // Set UI effects volume
    public void SetUIVolume(float volume)
    {
        audioMixer.SetFloat("UIVolume", volume);
        uiText.text = CalculatePercent(volume) + "%";
    }

    // Calculate volume percent
    private float CalculatePercent(float value)
    {
        return Mathf.RoundToInt((1f - (value * 0.5f) / halfVolumeValue) * 100);
    }
}
