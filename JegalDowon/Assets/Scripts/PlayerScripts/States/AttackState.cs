public class AttackState : IState
{

    // 공격 콤보 상태 - Attack1 -> Attack2 -> Attack3 -> Attack1 순환
    private readonly PlayerController _player;
    private readonly StateMachine _stateMachine;
    private bool _hasHit;

    public AttackState(PlayerController player, StateMachine stateMachine)
    {
        _player = player;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        _hasHit = false;
        _player.TriggerAttack();
        _player.ConsumeAttack();
    }

    public void Exit()
    {
    }

    public void Update()
    {
        // 히트 윈도우에 한 번만 데미지
        if (!_hasHit && _player.IsInAttackHitWindow())
        {
            _hasHit = _player.TryHitEnemies();
        }

        // 공격 중에도 이동 가능
        _player.Move(_player.MoveInput);

        // 점프 입력 시 공격 캔슬 -> 점프
        if (_player.JumpInput)
        {
            _player.ResetCombo();
            _player.ConsumeJump();
            _stateMachine.ChangeState(new JumpState(_player, _stateMachine));
            return;
        }

        // X 키 누를 때마다 다음 공격으로
        if (_player.AttackInput)
        {
            _hasHit = false; // 다음 공격도 타격 가능하도록 리셋
            _player.AdvanceCombo();
            _player.TriggerAttack();
            _player.ConsumeAttack();
            return;
        }

        // 애니메이션 완료 시 Idle/Move로 복귀
        if (_player.IsAttackComplete())
        {
            _player.ResetCombo();
            if (_player.HasMoveInput)
                _stateMachine.ChangeState(new MoveState(_player, _stateMachine));
            else
                _stateMachine.ChangeState(new IdleState(_player, _stateMachine));
           
        }
    }
}
