// Idle 상태에서 반대 방향 입력 시 재생되는 턴 애니메이션 상태

using UnityEngine;

public class IdleTurnState : IState
{
    private readonly PlayerController _player;
    private readonly StateMachine _stateMachine;
    private readonly bool _turnLeft; // true = 왼쪽으로 턴, false = 오른쪽으로 턴
    private float _elapsedTime;

    public IdleTurnState(PlayerController player, StateMachine stateMachine, bool turnLeft)
    {
        _player = player;
        _stateMachine = stateMachine;
        _turnLeft = turnLeft;
    }

    public void Enter()
    {
        _elapsedTime = 0f;
        _player.BeginIdleTurn(_turnLeft);
        Debug.Log($"[IdleTurn] 애니메이션 재생 시작 - {(_turnLeft ? "왼쪽" : "오른쪽")}으로 턴");
    }

    public void Exit()
    {
        _player.EndIdleTurn();
    }

    public void Update()
    {
        _elapsedTime += Time.deltaTime;
        _player.Stop();

        // IdleTurn 애니메이션 완료 대기 (타임아웃 1초로 안전장치)
        if (_player.IsIdleTurnComplete() || _elapsedTime >= 1f)
        {
            bool wasTimeout = _elapsedTime >= 1f;
            if (wasTimeout) Debug.Log("[IdleTurn] 타임아웃으로 종료 (Animator IdleTurn 상태 확인 필요)");
            _player.ApplyIdleTurnResult(_turnLeft);

            if (_player.HasMoveInput)
                _stateMachine.ChangeState(new MoveState(_player, _stateMachine));
            else
                _stateMachine.ChangeState(new IdleState(_player, _stateMachine));
        }
    }
}
