using UnityEngine;

public class EnemyIdleState : IState
{
    private readonly EnemyStateManager _enemy;
    private readonly StateMachine _stateMachine;

    public EnemyIdleState(EnemyStateManager enemy, StateMachine stateMachine)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
    }

    public void Enter() { }

    public void Exit() { }

    public void Update()
    {
        if (_enemy.IsPlayerRange(_enemy.ChaseRange))
            _stateMachine.ChangeState(new EnemyChaseState(_enemy, _stateMachine));
    }
}
