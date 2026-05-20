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

        losePlayerTimer = 0f;
    }

    public override void Perform()
    {
        // Checking if the player is Hiding and not attacking if they are
        if (enemy.Player.GetComponent<PlayerHiding>().isHiding)
        {
            stateMachine.ChangeState(stateMachine.searchState);
            return;
        }
        
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);

        // Calculate line-of-sight vector from enemy eyes to player eyes
        Vector3 startPos = enemy.transform.position + Vector3.up * enemy.eyeHeight;
        Vector3 targetPos = enemy.Player.transform.position + Vector3.up * enemy.eyeHeight;
        Vector3 targetDirection = targetPos - startPos;

        // Check if a solid object physically blocks the view to the player
        bool wallIsBlocking =
            Physics.Raycast(startPos, targetDirection.normalized, out RaycastHit hit, enemy.sightDistance) &&
            !hit.collider.CompareTag("Player");

        if (!wallIsBlocking)
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
            
            if (distanceToPlayer > enemy.AttackDistance - 0.4f)
                enemy.Agent.SetDestination(enemy.Player.transform.position);
            else
                enemy.Agent.ResetPath();

            if (distanceToPlayer <= enemy.AttackDistance + 1.0f)
                enemy.EnemyAttack();
        }

        else
        {
            losePlayerTimer += Time.deltaTime;
            enemy.Agent.SetDestination(enemy.LastKnownPosition);

            if (losePlayerTimer > 3.0f)
                stateMachine.ChangeState(stateMachine.searchState);
        }
    }


    public override void Exit()
    {
        enemy.Agent.stoppingDistance = 0f;
    }
}