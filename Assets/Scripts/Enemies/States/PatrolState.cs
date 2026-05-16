using UnityEngine;

public class PatrolState : BaseState
{
   public int waypointIndex;
   public float waitTimer;
   
   
   public override void Enter()
   {
      if (enemy.path != null && enemy.path.waypoints.Count > 0)
         enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
   }

   public override void Perform()
   {
      PatrolCycle();
      
      if(enemy.CanSeePlayer())
         stateMachine.ChangeState(stateMachine.attackState);
   }

   public override void Exit()
   {
   }

   public void PatrolCycle()
   {
      if (enemy.path == null || enemy.path.waypoints.Count == 0)
         return;

      if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance < 0.2)
      {
         waitTimer += Time.deltaTime;

         if (waitTimer > 3)
         {
            // Making the enemy move to a random waypoint in their path each
            waypointIndex = Random.Range(0, enemy.path.waypoints.Count);

            enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);

            waitTimer = 0f;
         }
      }
        
      
   }
}
