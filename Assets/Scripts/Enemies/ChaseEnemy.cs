using UnityEngine;
using UnityEngine.AI;

public class ChaseEnemy : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float catchDistance = 1.5f;

    private NavMeshAgent agent;
    private Transform player;
    private bool isChasing = false;

    private PlayerHealth playerHealth;
    private bool hasCaughtPlayer = false;
    
    // Assigning all the components for the enemy
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
            agent.speed = chaseSpeed;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();
            
            if(playerHealth == null)
                Debug.LogWarning("ChaseEnemy: No health componenet found on player");
        }
            
    }

    // Making sure the enemy stays locked on the player once the chase starts
    void Update()
    {
        if (!isChasing || agent == null || player == null)
            return;

        agent.SetDestination(player.position);

        if (!hasCaughtPlayer && playerHealth != null &&
            Vector3.Distance(transform.position, player.position) <= catchDistance)
        {
            hasCaughtPlayer = true;
            playerHealth.Die();
        }
    }
    
    // Function used to start the chase
    public void StartChase()
    {
        isChasing = true;
    }
}
