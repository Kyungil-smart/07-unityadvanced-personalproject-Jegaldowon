// 공중 공격 상태 - 점프/낙하 중 Attack1, Attack2, Attack3 콤보 (지상과 동일)

public class AirAttackState : IState
{
    private readonly PlayerController _player;
    private readonly StateMachine _stateMachine;

    public AirAttackState(PlayerController player, StateMachine stateMachine)
    {
        _player = player;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        _player.SetJumping(false);
        _player.SetFalling(true);
        _player.ResetCombo(); // 공중에서는 Attack1부터 시작
        _player.TriggerAttack();
        _player.ConsumeAttack();
    }

    public void Exit()
    {
    }

    public void Update()
    {
        // 공중 공격 중에도 이동 가능 (에어 컨트롤)
        _player.Move(_player.MoveInput);

        // X 키 누를 때마다 다음 공격으로 (지상 콤보와 동일)
        if (_player.AttackInput)
        {
            _player.AdvanceCombo();
            _player.TriggerAttack();
            _player.ConsumeAttack();
            return;
        }

        // 공격 애니메이션 완료 시 Fall로 복귀
        if (_player.IsAttackComplete())
        {
            _player.ResetCombo();
            _stateMachine.ChangeState(new FallState(_player, _stateMachine));
            return;
        }

        // 착지 시 Idle/Move로
        if (_player.IsGrounded)
        {
            _player.ResetCombo();
            if (_player.HasMoveInput)
                _stateMachine.ChangeState(new MoveState(_player, _stateMachine));
            else
                _stateMachine.ChangeState(new IdleState(_player, _stateMachine));
        }
    }
}
