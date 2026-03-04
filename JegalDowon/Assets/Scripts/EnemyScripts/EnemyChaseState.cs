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

        float dirX = Mathf.Sign(_enemy.Player.position.x - _enemy.transform.position.x);

        _enemy.SetAnimatorSpeed(1f);
        _enemy.SetFacing(dirX < 0);

        // 낭떨어지 앞에 바닥 없으면 이동 안 함 (끝에서 멈춤)
        if (!_enemy.IsGroundAhead(dirX))
        {
            _enemy.SetAnimatorSpeed(0);
            if (!_enemy.IsPlayerRange(_enemy.AttackRange))
                _stateMachine.ChangeState(new EnemyPatrolState(_enemy, _stateMachine));
            return;
        }
        // 플레이어를 향해 이동 (플레이어가 공격 범위 안에 들어오면 공격 상태로 전환)

        _enemy.transform.position = Vector3.MoveTowards(
            _enemy.transform.position,
            _enemy.Player.position,
            _enemy.Speed * Time.deltaTime);

        // 플레이어가 공격 범위 안에 들어오면 공격 상태로 전환, 플레이어가 추적 범위를 벗어나면 대기 상태로 전환
        // 근데 이거 그냥 필드에서 돌아다니게 할까? 플레이어가 추적 범위를 벗어나면 대기 상태로 전환하는 게 맞는 것 같기도 하고

        if (_enemy.IsPlayerRange(_enemy.AttackRange))
            _stateMachine.ChangeState(new EnemyAttackState(_enemy, _stateMachine));


        // 플레이어가 추적 범위를 벗어나면 다시 패트롤
        else if (!_enemy.IsPlayerRange(_enemy.ChaseRange))
            _stateMachine.ChangeState(new EnemyPatrolState(_enemy, _stateMachine));
    }
}
