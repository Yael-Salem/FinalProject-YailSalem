using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionsPanel;
    
    [Header("Pause menu buttons")]
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button loadLastSaveBtn;
    [SerializeField] private Button optionsBtn;
    [SerializeField] private Button mainMenuBtn;
    
    
    // Main Menu scene name
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    
    [SerializeField] private InputManager inputManager;

    [SerializeField] private OptionsMenuManager optionsMenuManager;

    private bool isPaused = false;

    private void Start()
    {
        if(continueBtn != null) continueBtn.onClick.AddListener(ContinueGame);
        if(loadLastSaveBtn != null) loadLastSaveBtn.onClick.AddListener(LoadLastSave);
        if(optionsBtn != null) optionsBtn.onClick.AddListener(OpenOptions);
        if(mainMenuBtn != null) mainMenuBtn.onClick.AddListener(ReturnToMainMenu);

        if (optionsPanel != null)
        {
            optionsMenuManager.OnBackAction += CloseOptions;
        }
    }
    
    public void TogglePause()
    {
        if(optionsPanel.activeSelf)
            CloseOptions();
        
        else if (isPaused)
            ContinueGame();

        else
            PauseGame();
    }

    private void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
        Time.timeScale = 0f;

        if (inputManager != null)
        {
            inputManager.onFoot.Disable();
            inputManager.uiActions.Enable();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ContinueGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        Time.timeScale = 1f;

        if (inputManager != null)
        {
            inputManager.onFoot.Enable();
            inputManager.uiActions.Disable();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void OpenOptions()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    
    private void CloseOptions()
    {
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    private void LoadLastSave()
    {
        Time.timeScale = 1f;

        if (SaveManager.Instance != null)
        {
            bool loadSuccessful = SaveManager.Instance.LoadGame();

            if (loadSuccessful)
            {
                Debug.Log("Loaded successful");
                ContinueGame();
            }

            else
            {
                Debug.Log("Load failed");
                PauseGame();
            }
        }
    }

    private void OnDestroy()
    {
        if(continueBtn != null) continueBtn.onClick.RemoveListener(ContinueGame);
        if(loadLastSaveBtn != null) loadLastSaveBtn.onClick.RemoveListener(LoadLastSave);
        if(optionsBtn != null) optionsBtn.onClick.RemoveListener(OpenOptions);
        if(mainMenuBtn != null) mainMenuBtn.onClick.RemoveListener(ReturnToMainMenu);

        if (optionsPanel != null)
        {
            optionsMenuManager.OnBackAction -= CloseOptions;
        }
    }
}
