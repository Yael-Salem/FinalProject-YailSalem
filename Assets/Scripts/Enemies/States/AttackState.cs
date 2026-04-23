using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    
    
    public override void Enter()
    {
        
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0f;

            moveTimer += Time.deltaTime;

            if (moveTimer > Random.Range(2, 8))
            {
                enemy.Agent.SetDestination(enemy.transform.position + Random.insideUnitSphere * 5);

                moveTimer = 0;
            }
        }

        else
        {
            losePlayerTimer += Time.deltaTime;

            if (losePlayerTimer > 5)
            {
                // change to search state if the enemy has lost the player
                stateMachine.ChangeState(new PatrolState());
            }
        }
        
        enemy.Attack();
    }

    public override void Exit()
    {
        
    }
    
    
}
