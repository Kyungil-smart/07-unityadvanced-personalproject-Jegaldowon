using UnityEngine;

// 사망 상태. 애니메이션 재생만 하고 아무것도 안 함.
//
//
// 해결 완료

public class EnemyDeadState : IState
{
    private readonly EnemyStateManager _enemy;
    private readonly StateMachine _stateMachine;

    public EnemyDeadState(EnemyStateManager enemy, StateMachine stateMachine)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
    }

    public void Enter() { }
    public void Exit() { }
    public void Update() { }
}
