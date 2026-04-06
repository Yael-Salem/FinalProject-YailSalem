using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    
    // Getter for the agent
    public NavMeshAgent Agent
    {
        get => agent;
    }

    [SerializeField]
    private string currentState;

    public Path path;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();

        agent = GetComponent<NavMeshAgent>();
        
        stateMachine.Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
