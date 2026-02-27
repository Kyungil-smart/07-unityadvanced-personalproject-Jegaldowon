using UnityEngine;

// IdleState, MoveState, JumpState, AttackState

public interface IState
{
    // 이 놈이 가질 주기
    void Enter();

    void Update();

    void Exit();
}
