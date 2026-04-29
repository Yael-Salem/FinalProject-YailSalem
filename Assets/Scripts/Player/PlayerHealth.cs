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
    
    [Header("UI Elements")]
    public float maxHealth = 100f;
    private float healthFraction;

    public Image frontHealthBar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;

        playerCombat = GetComponent<PlayerCombat>();
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
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        
        healthFraction = health / maxHealth;

        frontHealthBar.fillAmount = healthFraction;
    }
}