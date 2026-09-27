using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DemoEndMenuManager : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [SerializeField] private Button returnToMainBtn;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if(returnToMainBtn != null)
            returnToMainBtn.onClick.AddListener(OnReturnToMainMenuClicked);
    }

    private void OnReturnToMainMenuClicked()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void OnDestroy()
    {
        if(returnToMainBtn != null)
            returnToMainBtn.onClick.RemoveListener(OnReturnToMainMenuClicked);
    }
}
