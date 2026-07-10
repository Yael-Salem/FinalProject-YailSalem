using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    private const string MIXER_PARAMETER = "MasterVolume";
    private const string PREFS_KEY = "MasterVolumePrefrence";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(PREFS_KEY, 1f);

        volumeSlider.value = savedVolume;

        SetVolume(savedVolume);
        
        // Running our SetVolume function any time the slider is changed
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float sliderValue)
    {
        // If the slider value is 0, setting it to a tiny value to avoid a logic error in Mathf.Log10
        if (sliderValue <= 0)
            sliderValue = 0.0001f;
        
        
        float decibelValue = Mathf.Log10(sliderValue) * 20;

        audioMixer.SetFloat(MIXER_PARAMETER, decibelValue);
        
        PlayerPrefs.SetFloat(PREFS_KEY, sliderValue);

    }

    private void OnDestroy()
    {
        if(volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
    }
}
