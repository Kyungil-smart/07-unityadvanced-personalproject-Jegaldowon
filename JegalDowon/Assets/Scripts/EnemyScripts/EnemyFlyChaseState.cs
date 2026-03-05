using UnityEngine;

/// <summary>비행 몬스터 추적. 플레이어를 향해 이동하며 수직 진동.</summary>
public class EnemyFlyChaseState : IState
{
    private readonly EnemyStateManager _enemy;
    private readonly StateMachine _stateMachine;

    public EnemyFlyChaseState(EnemyStateManager enemy, StateMachine stateMachine)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
    }

    public void Enter() { }
    public void Exit() { }

    public void Update()
    {
        if (_enemy.FlyData == null)
        {
            _stateMachine.ChangeState(new EnemyPatrolState(_enemy, _stateMachine));
            return;
        }

        if (_enemy.Player == null) return;

        float dirX = Mathf.Sign(_enemy.Player.position.x - _enemy.transform.position.x);
        _enemy.SetAnimatorSpeed(1f);
        _enemy.SetFacing(dirX < 0);

        float flyY = _enemy.StartY + Mathf.Sin(Time.time * _enemy.FlyData.FlySpeed) * _enemy.FlyData.FlyRange;
        Vector3 targetPos = Vector3.MoveTowards(
            _enemy.transform.position,
            _enemy.Player.position,
            _enemy.Speed * Time.deltaTime);
        targetPos.y = flyY;
        _enemy.transform.position = targetPos;

        if (_enemy.IsPlayerRange(_enemy.AttackRange))
            _stateMachine.ChangeState(new EnemyAttackState(_enemy, _stateMachine));
        else if (!_enemy.IsPlayerRange(_enemy.ChaseRange))
            _stateMachine.ChangeState(new EnemyFlyPatrolState(_enemy, _stateMachine));
    }
}
