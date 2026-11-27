using UnityEngine;

public abstract class BaseState
{
    [SerializeField] protected Enemy enemy;
    
    [SerializeField] protected StateMachine stateMachine;
    
    public void Setup(StateMachine machine, Enemy enemyRef)
    {
        this.stateMachine = machine;
        this.enemy = enemyRef;
    }


    public abstract void Enter();

    public abstract void Perform();

    public abstract void Exit();

}
