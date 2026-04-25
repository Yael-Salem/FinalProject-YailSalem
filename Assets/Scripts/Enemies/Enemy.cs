using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
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

    public void Attack()
    {
        animator.Play("enemy_swing");
    }
}
