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
        
        if(mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }
    
    private void OnLowGraphicsClicked()
    {
        Debug.Log("Low graphics selected");
    }
    
    private void OnMediumGraphicsClicked()
    {
        Debug.Log("Medium graphics selected");
    }
    
    private void OnHighGraphicsClicked()
    {
        Debug.Log("High graphics selected");
    }
    
    private void OnVSyncToggled(bool isEnabled)
    {
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
