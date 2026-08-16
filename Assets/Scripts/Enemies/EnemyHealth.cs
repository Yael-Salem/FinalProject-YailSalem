using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
   public float maxHealth = 100f;
   
   public float currentHealth;

   private bool isDead = false;
   
   // Event to fire when an enemy dies
   public static event Action<GameObject> onEnemyDied;

   private void Awake()
   {
      currentHealth = maxHealth;
   }

   public void TakeDamage(float damage)
   {
      if (isDead)
         return;

      currentHealth -= damage;

      currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

      Debug.Log($"Enemy health: {currentHealth}");
      
      // Forcing the enemy to aggro when hit
      if(TryGetComponent<Enemy>(out var enemyAI))
         enemyAI.ForceAggro();
      
      if (currentHealth <= 0)
      {
         isDead = true;
         
         onEnemyDied?.Invoke(gameObject);
         
         Destroy(gameObject);
      }
   }
}
