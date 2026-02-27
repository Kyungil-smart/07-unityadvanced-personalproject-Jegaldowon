// FallState.cs
public class FallState : IState
{
    private PlayerController _player;
    private StateMachine _stateMachine;

    public FallState(PlayerController playerController, StateMachine stateMachine)
    {
        _player = playerController;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        //  Fall 들어오면 점프 플래그 정리
        _player.SetJumping(false);
        _player.SetFalling(true);
    }

    public void Exit()
    {
        _player.SetFalling(false);
    }

    public void Update()
    {
        _player.Move(_player.MoveInput);

        //  착지하면 Idle/Move
        if (_player.IsGrounded)
        {
            if (_player.HasMoveInput)
                _stateMachine.ChangeState(new MoveState(_player, _stateMachine));
            else
                _stateMachine.ChangeState(new IdleState(_player, _stateMachine));
        }
    }
}