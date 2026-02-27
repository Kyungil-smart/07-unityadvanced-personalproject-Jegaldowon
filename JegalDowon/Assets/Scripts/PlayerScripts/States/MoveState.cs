// 플레이어가 이동할 때의 상태

public class MoveState : IState
{
    PlayerController _player;
    StateMachine _stateMachine;

    public MoveState(PlayerController player, StateMachine stateMachine)
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
        // 이동 입력이 없으면 IdleState로 전환
        if (_player.MoveInput == 0)
        {
            _stateMachine.ChangeState(new IdleState(_player, _stateMachine));
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
