// 플레이어가 점프할 때의 상태

public class JumpState : IState
{
    PlayerController _player;
    StateMachine _stateMachine;

    bool _isGround;


    public JumpState(PlayerController player, StateMachine stateMachine)
    {
        _player = player;
        _stateMachine = stateMachine;
    }
    public void Enter()
    {
        _player.Jump();
        _player.ConsumeJump();
    }

    public void Exit()
    {
    }

    public void Update()
    {
        // 점프 중에도 이동 ㄱㄴ
        _player.Move(_player.MoveInput);

        // 공중에 떠있는 동안에는 계속 점프 상태 유지 
        if (!_player.IsGrounded)
        {
            _isGround = true;
        }

        // 착지하면 Idle 또는 Move로 전환
        if (_player.IsGrounded && _isGround)
        {
            // 이동 입력이 있으면 Move 상태로, 없으면 Idle 상태로 전환 
            // 아 머리 아프다 크으아각
            if (_player.MoveInput != 0)
                _stateMachine.ChangeState(new MoveState(_player, _stateMachine));
            else
                _stateMachine.ChangeState(new IdleState(_player, _stateMachine));
        }
    }
}
