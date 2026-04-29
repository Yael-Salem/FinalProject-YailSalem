using UnityEngine;
using UnityEngine.AI;

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
    
    [SerializeField]
    private float attackDamage = 5f;
    
    private float attackDelay = 1f;
    private float attackDistance = 1.5f;
    private float attackSpeed = 1f;
    private bool attacking = false;
    private bool readyToAttack = true;

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
    
}
