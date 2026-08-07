using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Header("State machine and path variables")]
    private StateMachine stateMachine;

    private NavMeshAgent agent;
    private Animator animator;

    // Getter for the agent
    public NavMeshAgent Agent
    {
        get => agent;
    }

    [SerializeField] private string currentState;

    public Path path;

    public GameObject Player { get; private set; }


    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;

    [Header("Combat variables")]

    // Attacking Variables
    private readonly float attackDelay = 0.4f;

    private readonly float attackDistance = 2f;
    private readonly float attackCooldown = 1.5f;
    private bool attacking = false;
    private float nextAttackTime = 0f;

    // Dodging variables
    private bool dodging = false;
    private bool canDodge = true;
    private readonly float dodgeCoolDown = 5f;
    private readonly float dodgeSpeed = 15f;
    private readonly float dodgeDuration = 0.2f;

    private float dodgeTimer;
    private float cooldownTimer;
    private Vector3 currentDodgeDirection;

    public float AttackDelay => attackDelay;

    public float AttackDistance => attackDistance;


    // Tracking last known position of the player to use in the SearchState
    public Vector3 LastKnownPosition { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();

        agent = GetComponent<NavMeshAgent>();

        stateMachine.Initialise(this);

        Player = GameObject.FindGameObjectWithTag("Player");

        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSeePlayer())
            LastKnownPosition = Player.transform.position;

        currentState = stateMachine.activeState.ToString();

        // Dodging logic
        if (dodging)
        {
            transform.Translate(currentDodgeDirection * dodgeSpeed * Time.deltaTime, Space.World);
            dodgeTimer -= Time.deltaTime;

            if (dodgeTimer <= 0)
            {
                dodging = false;
                cooldownTimer = dodgeCoolDown;
            }
        }


        if (!canDodge)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0)
                canDodge = true;
        }
    }

    public bool CanSeePlayer()
    {
        if (Player == null)
            return false;

        // Checking if the player is close enough to be seen
        if (Vector3.Distance(transform.position, Player.transform.position) < sightDistance)
        {
            Vector3 startPosition = transform.position + Vector3.up * eyeHeight;
            Vector3 targetPosition = Player.GetComponent<Collider>().bounds.center;
            Vector3 targetDirection = targetPosition - startPosition;
            
            // Flattening the height so the enemy sees the player regardless of height
            Vector3 flatDirection = targetDirection;
            flatDirection.y = 0;

            float angleToPlayer = Vector3.Angle(flatDirection, transform.forward);

            if (angleToPlayer <= fieldOfView)
            {
                Ray ray = new Ray(startPosition, targetDirection.normalized);

                RaycastHit hitInfo = new RaycastHit();

                // Checking if there is a wall in front of the enemy before checking if they hit the player
                if (Physics.Raycast(ray, out hitInfo, sightDistance))
                {
                    if (hitInfo.collider.CompareTag("Player"))
                    {
                        Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.green);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void EnemyAttack()
    {
        if (Time.time < nextAttackTime || attacking || dodging)
            return;

        attacking = true;
        nextAttackTime = Time.time + attackCooldown;

        Invoke(nameof(EnemyAttackRayCast), attackDelay);
        Invoke(nameof(EnemyAttackReset), attackCooldown * 0.8f);

        animator.Play("enemy_swing");
    }

    public void EnemyAttackReset()
    {
        attacking = false;
    }

    public void EnemyAttackRayCast()
    {
        if (Physics.Raycast(transform.position + Vector3.up * (eyeHeight / 2f), transform.forward, out RaycastHit hit, attackDistance))
        {
            Debug.Log($"Enemy attack hit: '{hit.transform.name}' with Tag: '{hit.transform.tag}'");
            
            if (hit.transform.gameObject == Player || hit.collider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(Random.Range(2, 7));

                    Debug.Log(playerHealth.Health);
                }
            }
        }
    }

    public bool TryDodge()
    {
        if (!canDodge || dodging)
            return false;

        // The enemy will successfully dodge player's attack 25% of the time
        if (Random.value < 0.25f)
        {
            Dodge();
            
            // Forcing the enemy to attack the player even if they do dodge the attack
            ForceAggro();
            
            return true;
        }

        return false;
    }

    private void Dodge()
    {
        dodging = true;
        canDodge = false;
        dodgeTimer = dodgeDuration;

        currentDodgeDirection = Random.value > 0.5f ? transform.right : -transform.right;
    }

    public void ForceAggro()
    {
        if (stateMachine != null && Player != null)
        {
            LastKnownPosition = Player.transform.position;

            stateMachine.ChangeState(stateMachine.attackState);
        }
    }
}