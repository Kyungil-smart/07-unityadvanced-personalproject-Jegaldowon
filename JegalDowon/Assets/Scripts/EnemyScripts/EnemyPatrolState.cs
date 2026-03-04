using UnityEngine;

/// <summary>
/// 필드에서 돌아다니다가 플레이어가 범위 안 들어오면 Chase로.
/// 멈출지 움직일지 랜덤 결정.
/// </summary>
public class EnemyPatrolState : IState
{
    private readonly EnemyStateManager _enemy;
    private readonly StateMachine _stateMachine;

    private float _moveDirection;
    private bool _isMoving;
    private float _nextDecisionTime;

    public EnemyPatrolState(EnemyStateManager enemy, StateMachine stateMachine)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        DecideNextAction();
    }

    public void Exit() { }

    public void Update()
    {
        // 플레이어 범위 안 들어오면 Chase
        if (_enemy.IsPlayerRange(_enemy.ChaseRange))
        {
            _stateMachine.ChangeState(new EnemyChaseState(_enemy, _stateMachine));
            return;
        }

        // 랜덤 결정 시간 되면 다시 결정
        if (Time.time >= _nextDecisionTime)
            DecideNextAction();

        if (!_isMoving)
        {
            _enemy.SetAnimatorSpeed(0);
            return;
        }

        _enemy.SetAnimatorSpeed(0.5f);
        _enemy.SetFacing(_moveDirection < 0);

        // 낭떨어지 앞이면 방향 전환
        if (!_enemy.IsGroundAhead(_moveDirection))
        {
            _moveDirection *= -1f;
        }

        // 패트롤 반경 밖이면 방향 전환
        if (_enemy.PatrolRadius > 0)
        {
            float dist = Vector3.Distance(_enemy.transform.position, _enemy.PatrolOrigin);
            if (dist >= _enemy.PatrolRadius)
                _moveDirection = Mathf.Sign(_enemy.PatrolOrigin.x - _enemy.transform.position.x);
        }

        // 이동
        Vector3 target = _enemy.transform.position + Vector3.right * (_moveDirection * _enemy.PatrolSpeed * Time.deltaTime);
        _enemy.transform.position = target;
    }

    private void DecideNextAction()
    {
        _isMoving = Random.value > 0.5f;
        if (_isMoving)
            _moveDirection = Random.value > 0.5f ? 1f : -1f;

        _nextDecisionTime = Time.time + Random.Range(_enemy.PatrolDecisionMin, _enemy.PatrolDecisionMax);
    }
}
