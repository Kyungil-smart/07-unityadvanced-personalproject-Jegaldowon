// 플레이어가 가만히 있을 때의 상태

public class IdleState : IState
{
    PlayerController _player;
    StateMachine _stateMachine;

    public IdleState(PlayerController player, StateMachine stateMachine)
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
        _player.Stop();

        // 이동 입력이 있으면 MoveState로 전환
        if (_player.MoveInput != 0)
        {
            _stateMachine.ChangeState(new MoveState(_player, _stateMachine));
            return;
        }

        // 점프 입력이 있으면 JumpState로 전환
        if (_player.JumpInput)
        {
            _player.ConsumeJump();
            _stateMachine.ChangeState(new JumpState(_player, _stateMachine));
        }
    }
}
