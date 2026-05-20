using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;

    public float Health
    {
        get => health;
    }

    private PlayerCombat playerCombat;
    
    [Header("Health UI Elements")]
    public float maxHealth = 100f;
    private float healthFraction;

    public Image frontHealthBar;

    [Header("Game Over UI")] 
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private Button loadGameBtn;
    [SerializeField] private Button quitGameBtn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;

        playerCombat = GetComponent<PlayerCombat>();
        
        // Making the game over screen invisible when the game start
        if(gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public void TakeDamage(float damageTaken)
    {
        if (playerCombat.Blocking)
            return;
        
        health = Mathf.Clamp(health - damageTaken, 0, maxHealth);

        // Updating health UI
        healthFraction = health / maxHealth;

        frontHealthBar.fillAmount = healthFraction;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        if (gameOverScreen != null)
        {
            if (loadGameBtn != null)
            {
                loadGameBtn.onClick.RemoveAllListeners();
                
                loadGameBtn.onClick.AddListener(() =>
                {
                    OnLoadButtonPressed();
                } );
            }

            if (quitGameBtn != null)
            {
                quitGameBtn.onClick.RemoveAllListeners();
                
                quitGameBtn.onClick.AddListener(() =>
                {
                    OnQuitButtonPressed();
                } );
            }
            
            gameOverScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;
        }
    }

    private void OnLoadButtonPressed()
    {
        Time.timeScale = 1f;

        if (SaveManager.Instance != null)
        {
            if (SaveManager.Instance.LoadGame())
            {
                Debug.Log("Load Successful");
                
                gameOverScreen.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            else
            {
                Debug.Log("Load failed");
                gameOverScreen.SetActive(false);
                
                RestoreHealth(maxHealth);
            }
        }
    }
    
    private void OnQuitButtonPressed()
    {
        Debug.Log("Quitting game");
    }

    public void RestoreHealth(float healAmount)
    {
        health = Mathf.Clamp(health + healAmount, 0, maxHealth);
        
        healthFraction = health / maxHealth;

        frontHealthBar.fillAmount = healthFraction;
    }
    
    // Function for the SaveManager to set the player's health based on the value inside the save file
    public void SetCurrentHealth(float loadedHealthValue)
    {
        health = Mathf.Clamp(loadedHealthValue, 0, maxHealth);
        healthFraction = health / maxHealth;

        if (frontHealthBar != null)
            frontHealthBar.fillAmount = healthFraction;

    }
}