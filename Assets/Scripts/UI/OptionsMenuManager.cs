using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;

    [Header("Buttons")]
    [SerializeField] private Button backBtn;
    [SerializeField] private Button lowGraphicsBtn;
    [SerializeField] private Button mediumGraphicsBtn;
    [SerializeField] private Button highGraphicsBtn;

    [Header("Toggles")]
    [SerializeField] private Toggle vSyncToggle;
    
    [Header("Sliders")]
    [SerializeField] private Slider sensitivitySlider;
    
    // Event to know when the back button was clicked
    public event Action OnBackAction;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(backBtn != null)
            backBtn.onClick.AddListener(OnBackBtnClicked);
        
        if(lowGraphicsBtn != null)
            lowGraphicsBtn.onClick.AddListener(OnLowGraphicsClicked);
        
        if(mediumGraphicsBtn != null)
            mediumGraphicsBtn.onClick.AddListener(OnMediumGraphicsClicked);
        
        if(highGraphicsBtn != null)
            highGraphicsBtn.onClick.AddListener(OnHighGraphicsClicked);

        if (vSyncToggle != null)
        {
            vSyncToggle.isOn = QualitySettings.vSyncCount > 0;
            vSyncToggle.onValueChanged.AddListener(OnVSyncToggled);
        }

        ApplyFrameRateSetting(QualitySettings.vSyncCount > 0);

        if (sensitivitySlider != null)
        {
            float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 50f);
            sensitivitySlider.value = savedSensitivity;
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        }

    }
    
    private void OnBackBtnClicked()
    {
        // Switching to main menu
        if(optionsPanel != null)
            optionsPanel.SetActive(false);
        
        if(mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        
        OnBackAction?.Invoke();
    }
    
    private void OnLowGraphicsClicked()
    {
        QualitySettings.SetQualityLevel(0, true);
        
        // Changing the textures to quarter resolution
        QualitySettings.globalTextureMipmapLimit = 2;
        
        // Setting the overall resolution to 720p
        //Screen.SetResolution(1280, 720, Screen.fullScreenMode);
        
        Debug.Log("Low graphics selected");
    }
    
    private void OnMediumGraphicsClicked()
    {
        QualitySettings.SetQualityLevel(1, true);
        
        // Changing the textures to half resolution
        QualitySettings.globalTextureMipmapLimit = 1;
        
        // Setting the overall resolution to 900p
        //Screen.SetResolution(1600, 900, Screen.fullScreenMode);
        
        Debug.Log("Medium graphics selected");
    }
    
    private void OnHighGraphicsClicked()
    {
        QualitySettings.SetQualityLevel(2, true);
        
        // Changing the textures to full resolution
        QualitySettings.globalTextureMipmapLimit = 0;
        
        // Setting the overall resolution to 1080p
        //Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
        
        Debug.Log("High graphics selected");
    }
    
    private void OnVSyncToggled(bool isEnabled)
    {
        QualitySettings.vSyncCount = isEnabled ? 1 : 0;
        
        ApplyFrameRateSetting(isEnabled);
        
        Debug.Log($"V-Sync is on: {isEnabled}");
    }
    
    private void ApplyFrameRateSetting(bool vSyncEnabled)
    {
        Application.targetFrameRate = vSyncEnabled ? -1 : GetMonitorRefreshRate();
    }

    // Function to return the refresh rate of the player's monitor
    private int GetMonitorRefreshRate()
    {
        var rate = Screen.currentResolution.refreshRateRatio;

        return Mathf.RoundToInt((float)rate.numerator / rate.denominator);
    }
    
    private void OnSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);

        PlayerLook playerLook = FindFirstObjectByType<PlayerLook>();
        
        if(playerLook != null)
            playerLook.ApplySensitivity(value);
    }

    private void OnDestroy()
    {
        if(backBtn != null)
            backBtn.onClick.RemoveListener(OnBackBtnClicked);
        
        if(lowGraphicsBtn != null)
            lowGraphicsBtn.onClick.RemoveListener(OnLowGraphicsClicked);
        
        if(mediumGraphicsBtn != null)
            mediumGraphicsBtn.onClick.RemoveListener(OnMediumGraphicsClicked);
        
        if(highGraphicsBtn != null)
            highGraphicsBtn.onClick.RemoveListener(OnHighGraphicsClicked);
        
        if(vSyncToggle != null)
            vSyncToggle.onValueChanged.RemoveListener(OnVSyncToggled);
        
        if(sensitivitySlider != null)
            sensitivitySlider.onValueChanged.RemoveListener(OnSensitivityChanged);
    }
}
