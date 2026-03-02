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
        _player.SetSpeed(0f);
        _player.SetJumping(false);
        _player.SetFalling(false);

    }

    public void Exit()
    {

    }



    public void Update()
    {
        _player.Stop();

        // 공격 입력 (최우선)
        if (_player.AttackInput)
        {
            _player.ConsumeAttack();
            _player.ResetCombo();
            _stateMachine.ChangeState(new AttackState(_player, _stateMachine));
            return;
        }

        // 이동 입력이 있으면
        if (_player.HasMoveInput)
        {
            bool inputLeft = _player.MoveInput < 0;
            bool facingLeft = _player.IsFacingLeft;

            // 반대 방향 입력 시 IdleTurn 애니메이션 재생
            if (inputLeft != facingLeft)
            {
                _stateMachine.ChangeState(new IdleTurnState(_player, _stateMachine, inputLeft));
                return;
            }

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
