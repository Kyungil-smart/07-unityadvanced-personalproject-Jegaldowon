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
        _player.SetSpeed(_player.MoveInput);
        _player.SetJumping(false);
        _player.SetFalling(false);
    }

    public void Exit()
    {
    }

    public void Update()
    {
        // 공격 입력 (최우선)
        if (_player.AttackInput)
        {
            _player.ConsumeAttack();
            _player.ResetCombo();
            _stateMachine.ChangeState(new AttackState(_player, _stateMachine));
            return;
        }

        _player.Move(_player.MoveInput);

        // 이동 입력이 없으면 IdleState로 전환 (데드존으로 스틱 드리프트 방지)
        if (!_player.HasMoveInput)
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
