using UnityEngine;

public class HealItem : Interactable
{
    private PlayerHealth playerHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    protected override void Interact()
    {
        if (playerHealth != null)
        {
            if (playerHealth.Health < 100)
            {
                int healAmount = Random.Range(1, 11);
            
                playerHealth.RestoreHealth(healAmount);

                Debug.Log($"Player healed {healAmount} points");
                
                Destroy(gameObject);
            }

            else
            {
                Debug.Log("Player already at full health");
                this.promptMessage = "Health already full";
            }
        }
    }
}
