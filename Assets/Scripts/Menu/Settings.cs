using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TextMeshProUGUI masterText;
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI uiText;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider uiSlider;

    public float halfVolumeValue = -40f;


    void Start()
    {
        masterText.text = Mathf.RoundToInt(masterSlider.normalizedValue * 100f) + "%";
        musicText.text = Mathf.RoundToInt(musicSlider.normalizedValue * 100f) + "%";
        uiText.text = Mathf.RoundToInt(uiSlider.normalizedValue * 100f) + "%";
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        masterText.text = CalculatePercent(volume) + "%";
    }

    public void SetMusicAndFXVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        musicText.text = CalculatePercent(volume) + "%";
    }

    public void SetUIVolume(float volume)
    {
        audioMixer.SetFloat("UIVolume", volume);
        uiText.text = CalculatePercent(volume) + "%";
    }

    private float CalculatePercent(float value)
    {
        return Mathf.RoundToInt((1f - (value * 0.5f) / halfVolumeValue) * 100);
    }
}
