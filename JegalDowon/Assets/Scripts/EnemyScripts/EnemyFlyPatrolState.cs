using UnityEngine;

/// <summary>비행 몬스터 패트롤. 수직 진동하며 대기, 플레이어 감지 시 FlyChase로.</summary>
public class EnemyFlyPatrolState : IState
{
    private readonly EnemyStateManager _enemy;
    private readonly StateMachine _stateMachine;

    public EnemyFlyPatrolState(EnemyStateManager enemy, StateMachine stateMachine)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
    }

    public void Enter() { }
    public void Exit() { }

    public void Update()
    {
        if (_enemy.FlyData == null) return;

        if (_enemy.IsPlayerRange(_enemy.ChaseRange))
        {
            _stateMachine.ChangeState(new EnemyFlyChaseState(_enemy, _stateMachine));
            return;
        }

        float flyY = _enemy.StartY + Mathf.Sin(Time.time * _enemy.FlyData.FlySpeed) * _enemy.FlyData.FlyRange;
        Vector3 pos = _enemy.transform.position;
        pos.y = flyY;
        _enemy.transform.position = pos;

        _enemy.SetAnimatorSpeed(0.5f);
    }
}
