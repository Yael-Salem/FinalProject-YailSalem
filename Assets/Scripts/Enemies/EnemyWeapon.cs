using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWeapon : MonoBehaviour
{
   private void OnCollisionEnter(Collision collision)
   {
      Transform hitTransform = collision.transform;

      if (hitTransform.CompareTag("Player"))
      {
         Debug.Log("Player hit");
         hitTransform.GetComponent<PlayerHealth>().TakeDamage(Random.Range(1, 3));
      }
   }
}
