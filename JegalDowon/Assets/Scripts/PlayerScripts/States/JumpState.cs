// 플레이어가 점프할 때의 상태

public class JumpState : IState
{
    PlayerController _player;
    StateMachine _stateMachine;
    public JumpState(PlayerController player, StateMachine stateMachine)
    {
        _player = player;
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
        // 착지하면 Idle 또는 Move로 전환
        if (_player.IsGrounded)
        {
            if (_player.MoveInput != 0)
                _stateMachine.ChangeState(new MoveState(_player, _stateMachine));
            else
                _stateMachine.ChangeState(new IdleState(_player, _stateMachine));
        }
    }
}
