using UnityEngine;

public class EnemyFlyChaseState : IState
{
    private readonly EnemyStateManager _enemy;
    private readonly StateMachine _state;

    public EnemyFlyChaseState(EnemyStateManager enemy, StateMachine stateMachine)
    {
        _enemy = enemy;
        _state = stateMachine;
    }

    public void Enter() { }
    public void Exit() { }

    public void Update()
    {
        if (!IsFlyEnemy())
        {
            _state.ChangeState(new EnemyPatrolState(_enemy, _state));
            return;
        }

        if (_enemy.Player == null) return;

        FacePlayer();

        Vector3 nextPos = GetNextChasePosition();
        _enemy.transform.position = nextPos;

        TryChangeState();
    }

    private bool IsFlyEnemy()
    {
        return _enemy.FlyData != null;
    }

    private void FacePlayer()
    {
        float dirX = _enemy.Player.position.x - _enemy.transform.position.x;
        _enemy.SetFacing(dirX < 0f);
        _enemy.SetAnimatorSpeed(1f);
    }

    private Vector3 GetNextChasePosition()
    {
        // 플레이어 쪽으로 추적
        // 포기 그냥 Y 축만
        Vector3 pos = _enemy.transform.position;
        Vector3 playerPos = _enemy.Player.position;

        float step = _enemy.Speed * Time.deltaTime;
        float nextX = Mathf.MoveTowards(pos.x, playerPos.x, step);

        // 날아다니는 적은 Y축도 움직임
        float flyY = _enemy.StartY
                     + Mathf.Sin(Time.time * _enemy.FlyData.FlySpeed) * _enemy.FlyData.FlyRange;

        return new Vector3(nextX, flyY, pos.z);
    }

    private void TryChangeState()
    {
        if (_enemy.IsPlayerRange(_enemy.AttackRange))
        {
            _state.ChangeState(new EnemyAttackState(_enemy, _state));
            return;
        }

        if (!_enemy.IsPlayerRange(_enemy.ChaseRange))
        {
            _state.ChangeState(new EnemyFlyPatrolState(_enemy, _state));
        }
    }
}