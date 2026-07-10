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
        
        if(vSyncToggle != null)
            vSyncToggle.onValueChanged.AddListener(OnVSyncToggled);
    }
    
    private void OnBackBtnClicked()
    {
        // Switching to main menu
        if(optionsPanel != null)
            optionsPanel.SetActive(false);
        
        OnBackAction?.Invoke();
    }
    
    private void OnLowGraphicsClicked()
    {
        QualitySettings.SetQualityLevel(0, true);
        
        Debug.Log("Low graphics selected");
    }
    
    private void OnMediumGraphicsClicked()
    {
        QualitySettings.SetQualityLevel(2, true);
        
        Debug.Log("Medium graphics selected");
    }
    
    private void OnHighGraphicsClicked()
    {
        QualitySettings.SetQualityLevel(4, true);
        
        Debug.Log("High graphics selected");
    }
    
    private void OnVSyncToggled(bool isEnabled)
    {
        QualitySettings.vSyncCount = isEnabled ? 1 : 0;
        
        Debug.Log($"V-Sync is on: {isEnabled}");
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
    }
}
