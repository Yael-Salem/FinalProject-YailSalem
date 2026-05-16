using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;

    private Vector3 movementOffSet;

    public override void Enter()
    {
        enemy.Agent.stoppingDistance = enemy.AttackDistance - 0.2f;

        movementOffSet = Random.insideUnitSphere * 2.0f;
        movementOffSet.y = 0;
    }

    public override void Perform()
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);

        if (enemy.CanSeePlayer() || distanceToPlayer <= enemy.AttackDistance + 1.5f)
        {
            losePlayerTimer = 0f;
            moveTimer += Time.deltaTime;

            if (moveTimer > Random.Range(1.5f, 3.5f))
            {
                movementOffSet = Random.insideUnitSphere * 2.0f;
                movementOffSet.y = 0;
                moveTimer = 0;
            }

            Vector3 targetLook = enemy.Player.transform.position;
            targetLook.y = enemy.transform.position.y;

            enemy.transform.LookAt(targetLook);

            // Chasing the player
            enemy.Agent.SetDestination(enemy.Player.transform.position);


            if (distanceToPlayer <= enemy.AttackDistance + 1.0f)
            {
                enemy.EnemyAttack();
            }
        }

        else
        {
            // changing to the search state if the enemy has lost the player
            stateMachine.ChangeState(stateMachine.searchState);
        }
    }

    public override void Exit()
    {
        enemy.Agent.stoppingDistance = 0f;
    }
}