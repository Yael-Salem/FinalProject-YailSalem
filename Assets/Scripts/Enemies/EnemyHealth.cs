using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
   public float maxHealth = 100f;
   
   public float currentHealth;

   private bool isDead = false;

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

      if (currentHealth <= 0)
      {
         isDead = true;
         Destroy(gameObject, 0.5f);
      }
   }
}
