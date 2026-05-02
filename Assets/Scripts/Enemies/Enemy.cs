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

    private GameObject player;
    
    public GameObject Player
    {
        get => player;
    }
    
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;

    [Header("Combat variables")]
    
    // Attacking Variables
    private float attackDelay = 1f;
    private float attackDistance = 1.5f;
    private float attackSpeed = 1f;
    private bool attacking = false;
    private bool readyToAttack = true;
    
    // Doding variables
    private bool dodging = false;
    private bool canDodge = true;
    private float dodgeCoolDown = 5f;
    private float dodgeSpeed = 15f;
    private float dodgeDuration = 0.2f;

    private float dodgeTimer;
    private float cooldownTimer;
    private Vector3 currentDodgeDirection;

    public float AttackDelay => attackDelay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();

        agent = GetComponent<NavMeshAgent>();
        
        stateMachine.Initialise();

        player = GameObject.FindGameObjectWithTag("Player");

        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();

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
        // Checking if the player is close enough to be seen
        if (player != null && Vector3.Distance(transform.position, player.transform.position) < sightDistance)
        {
            Vector3 targetDirection = player.transform.position - transform.position - Vector3.up * eyeHeight;

            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

            if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
            {
                Ray ray = new Ray(transform.position + Vector3.up * eyeHeight, targetDirection);

                RaycastHit hitInfo = new RaycastHit();

                if (Physics.Raycast(ray, out hitInfo, sightDistance) && hitInfo.transform.gameObject == player)
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
        if (!readyToAttack || attacking)
            return;

        readyToAttack = false;
        attacking = true;
        
        Invoke(nameof(EnemyAttackRayCast), attackDelay);
        Invoke(nameof(EnemyAttackReset), attackSpeed);
        
        animator.Play("enemy_swing");
    }

    public void EnemyAttackReset()
    {
        attacking = false;
        readyToAttack = true;
    }

    public void EnemyAttackRayCast()
    {
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
