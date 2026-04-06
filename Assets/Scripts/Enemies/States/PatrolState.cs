using UnityEngine;

public class PatrolState : BaseState
{
   public int waypointIndex;
   public float waitTimer;
   
   
   
   public override void Enter()
   {
      throw new System.NotImplementedException();
   }

   public override void Perform()
   {
      PatrolCycle();
   }

   public override void Exit()
   {
      throw new System.NotImplementedException();
   }

   public void PatrolCycle()
   {
      if (enemy.Agent.remainingDistance < 0.2)
      {
         waitTimer += Time.deltaTime;

         if (waitTimer > 3)
         {
            if (waypointIndex < enemy.path.waypoints.Count - 1)
               waypointIndex++;

            else
               waypointIndex = 0;

            enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);

            waitTimer = 0f;
         }
        
      }
   }
}
