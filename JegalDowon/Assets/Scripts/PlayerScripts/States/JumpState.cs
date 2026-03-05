using UnityEngine;

public class JumpState : IState // 플레이어가 점프할 때의 상태
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
        _isGround = false;
        _player.Jump();
        _player.ConsumeJump();
        _player.SetJumping(true);
        _player.SetFalling(false);
    }

    public void Exit()
    {
    }

    public void Update()
    {
        // 점프 중 공격 입력 시 AirAttackState로
        if (_player.AttackInput)
        {
            _player.ConsumeAttack();
            _stateMachine.ChangeState(new AirAttackState(_player, _stateMachine));
            return;
        }

        _player.Move(_player.MoveInput);

        // 공중에 떠있는 동안에는 계속 점프 상태 유지 
        // 공중에서 Falling 상태로 전환 추가 And 연산자 
        if (!_player.IsGrounded)
            _isGround = true;
        // 착지하면 Idle 또는 Move로 전환
        // 
        if (_player.IsGrounded && _isGround)
        {
            if (_player.HasMoveInput)
                _stateMachine.ChangeState(new MoveState(_player, _stateMachine));
            else
                _stateMachine.ChangeState(new IdleState(_player, _stateMachine));
        }
    }
}
