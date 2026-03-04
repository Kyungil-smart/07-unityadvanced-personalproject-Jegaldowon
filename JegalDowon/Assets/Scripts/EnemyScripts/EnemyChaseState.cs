using UnityEngine;

public class EnemyChaseState : IState
{
    private readonly EnemyStateManager _enemy;
    private readonly StateMachine _stateMachine;

    public EnemyChaseState(EnemyStateManager enemy, StateMachine stateMachine)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
    }

    public void Enter() 
    {

    }

    public void Exit() 
    {

    }

    public void Update()
    {
        if (_enemy.Player == null) return;

        _enemy.transform.position = Vector3.MoveTowards(
            _enemy.transform.position,
            _enemy.Player.position,
            _enemy.Speed * Time.deltaTime);

        if (_enemy.IsPlayerRange(_enemy.AttackRange))
            _stateMachine.ChangeState(new EnemyAttackState(_enemy, _stateMachine));
        else if (!_enemy.IsPlayerRange(_enemy.ChaseRange))
            _stateMachine.ChangeState(new EnemyIdleState(_enemy, _stateMachine));
    }
}
