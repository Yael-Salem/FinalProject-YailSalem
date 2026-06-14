using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;

    [Header("Buttons")]
    [SerializeField] private Button newGameBtn;

    [SerializeField] private Button continueBtn;

    [SerializeField] private Button optionsBtn;

    [SerializeField] private Button quitBtn;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if(mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        
        if(optionsPanel != null)
            optionsPanel.SetActive(false);
        
        if(newGameBtn != null)
            newGameBtn.onClick.AddListener(OnNewGameClicked);
        
        if(continueBtn != null)
            continueBtn.onClick.AddListener(OnContinueClicked);
        
        if(optionsBtn != null)
            optionsBtn.onClick.AddListener(OnOptionsClicked);
        
        if(quitBtn != null)
            quitBtn.onClick.AddListener(OnQuitClicked);
    }
    
    private void OnNewGameClicked()
    {
        Debug.Log("New game started");
        
        // TODO: Implement new game start function
        // StartNewGame();
    }
    
    private void OnContinueClicked()
    {
        Debug.Log("Game continued");
        
        // TODO: Implement continue game function
        // ContinueGame();
    }
    
    private void OnOptionsClicked()
    {
        // Switching to options menu
        if(mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        
        if(optionsPanel != null)
            optionsPanel.SetActive(true);
    }
    
    private void OnQuitClicked()
    {
        Application.Quit();

        Debug.Log("Game closed successfully");
    }
    
    private void OnDestroy()
    {
        
        if(newGameBtn != null)
            newGameBtn.onClick.RemoveListener(OnNewGameClicked);
        
        if(continueBtn != null)
            continueBtn.onClick.RemoveListener(OnContinueClicked);
        
        if(optionsBtn != null)
            optionsBtn.onClick.RemoveListener(OnOptionsClicked);
        
        if(quitBtn != null)
            quitBtn.onClick.RemoveListener(OnQuitClicked);
    }
    
}
