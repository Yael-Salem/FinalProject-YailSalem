using System;
using UnityEngine;

public class AutoSaveTrigger : MonoBehaviour
{
   private bool hasTriggered = false;

   private void OnTriggerEnter(Collider other)
   {
      if (hasTriggered || !other.CompareTag("Player"))
         return;

      hasTriggered = true;
      
      if(SaveManager.Instance != null)
         SaveManager.Instance.SaveGame(other.transform.position);
   }
}
