using System;
using UnityEngine;
using UnityEngine.XR;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;
    
    public readonly PatrolState patrolState = new PatrolState();
    public readonly AttackState attackState = new AttackState();
    public readonly SearchState searchState = new SearchState();

    public void Initialise(Enemy enemy)
    {
        // Giving each state access to the state machine and enemy script
        patrolState.Setup(this, enemy);
        attackState.Setup(this, enemy);
        searchState.Setup(this, enemy);
        

        // The enemy starts the game in the patrol state
        ChangeState(patrolState);
    }

    void Update()
    {
        // Running the logic for the current state
        if(activeState != null)
            activeState.Perform();
    }

    public void ChangeState(BaseState newState)
    {
        if (activeState == newState)
            return;
        
        if(activeState != null)
            activeState.Exit();

        activeState = newState;
        
        if(activeState != null)
            activeState.Enter();
    }
}
