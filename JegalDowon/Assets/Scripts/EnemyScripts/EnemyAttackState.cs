using UnityEngine;

public class EnemyAttackState : IState
{
    private readonly EnemyStateManager _enemy;
    private readonly StateMachine _stateMachine;
    private bool _triggeredNext;
    private bool _hasDealtDamage;

    public EnemyAttackState(EnemyStateManager enemy, StateMachine stateMachine)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        _triggeredNext = false;
        _hasDealtDamage = false;
        _enemy.TriggerAttackAnim();
    }

    public void Exit() { }

    public void Update()
    {
        if (_enemy.FlyData != null)
        {
            float flyY = _enemy.StartY + Mathf.Sin(Time.time * _enemy.FlyData.FlySpeed) * _enemy.FlyData.FlyRange;
            Vector3 pos = _enemy.transform.position;
            pos.y = flyY;
            _enemy.transform.position = pos;
        }

        if (!_enemy.IsPlayerRange(_enemy.AttackRange))
        {
            if (_enemy.FlyData != null)
                _stateMachine.ChangeState(new EnemyFlyChaseState(_enemy, _stateMachine));
            else
                _stateMachine.ChangeState(new EnemyChaseState(_enemy, _stateMachine));
            return;
        }

        // 공격 애니메이션 타격 구간에 플레이어에게 데미지 (공격당 1회)
        if (_enemy.IsInAttackHitWindow() && !_hasDealtDamage)
        {
            _hasDealtDamage = true;
            if (_enemy.Player != null)
            {
                var damageable = _enemy.Player.GetComponentInChildren<IDamageable>();
                damageable?.TakeDamage(_enemy.Damage);
            }
        }

        // 공격 애니메이션 중이면 다음 공격 대기
        if (_enemy.IsInAttackAnim())
        {
            _triggeredNext = false;
            return;
        }

        // 공격 끝나고 Idle일 때 다음 공격
        if (!_triggeredNext)
        {
            _triggeredNext = true;
            _hasDealtDamage = false; // 다음 공격에서 데미지 가능하도록 리셋
            _enemy.TriggerAttackAnim();
        }
    }
}
