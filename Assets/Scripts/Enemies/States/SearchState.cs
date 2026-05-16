using UnityEngine;

public class SearchState : BaseState
{
  private float searchTimer;
  private float lookTimer;
  private float lookDirection;

  public override void Enter()
  {
    enemy.Agent.stoppingDistance = 0.2f;

    enemy.Agent.SetDestination(enemy.LastKnownPosition);

    searchTimer = 0f;
    lookTimer = 0f;
    lookDirection = Random.value > 0.5f ? 1f : -1f;
  }

  public override void Perform()
  {
    if (enemy.CanSeePlayer())
    {
      stateMachine.ChangeState(stateMachine.attackState);
      return;
    }
    
    // Waiting until arriving at the player's last known spot before scanning
    if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= 0.6f)
    {
      searchTimer += Time.deltaTime;
      lookTimer += Time.deltaTime;
      
      // Making the enemy pan their heads in search
      if (lookTimer > 1.5f)
      {
        lookDirection *= -1f;
        lookTimer = 0f;
      }
      
      enemy.transform.Rotate(Vector3.up * lookDirection * 60f * Time.deltaTime);
      
      // Changing back to patrol state if 5 seconds have passed and the enemy hasn't found the player
      if(searchTimer > 5.0f)
        stateMachine.ChangeState(stateMachine.patrolState);
      
    }
  }

  public override void Exit()
  {
    enemy.Agent.stoppingDistance = 0f;
  }
}
