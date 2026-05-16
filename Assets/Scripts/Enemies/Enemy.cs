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

    [SerializeField]
    private string currentState;

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
            Vector3 targetPosition = Player.transform.position + Vector3.up * eyeHeight;
            Vector3 targetDirection = Player.transform.position - transform.position - Vector3.up * eyeHeight;

            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

            if (angleToPlayer <= fieldOfView)
            {
                Ray ray = new Ray(startPosition, targetDirection.normalized);

                RaycastHit hitInfo = new RaycastHit();

                if (Physics.Raycast(ray, out hitInfo, sightDistance) && hitInfo.transform.gameObject == Player)
                {
                    Debug.DrawRay(ray.origin, ray.direction * sightDistance);

                    return true;
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
        // Vector3 startPos = transform.position + Vector3.up * eyeHeight;
        //
        // if (Physics.Raycast(startPos, transform.forward, out RaycastHit hit, attackDistance + 0.5f))
        // {
        //     Debug.Log($"Enemy raycast struck object: '{hit.transform.name}' with tag: '{hit.transform.tag}'");
        //     
        //     if (hit.transform.CompareTag("Player") && hit.transform.TryGetComponent<PlayerHealth>(out var playerHealth))
        //     {
        //         playerHealth.TakeDamage(Random.Range(5, 12));
        //         
        //         Debug.Log(playerHealth.Health);
        //     }
        // }
        //
        // else
        // {
        //     Debug.Log("Enemy raycast missed entirely! Struck nothing.");
        // }
        
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, attackDistance))
        {
            PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(Random.Range(2, 7));
                
                Debug.Log(playerHealth.Health);
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
}
