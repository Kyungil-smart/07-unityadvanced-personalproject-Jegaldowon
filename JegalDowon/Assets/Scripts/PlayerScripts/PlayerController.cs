using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    private PlayerInput _playerInput;
    private Collider2D _collider;

    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _groundCheckDistance = 0.1f;


    // 키 입력 되고 이제 rb 로 움직이게 할 거임
    private Rigidbody2D _rigidbody;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _jumpForce = 12f;

    // 입력값 데드존 
    // Idle 상태에서
    [SerializeField] float _moveInputDeadZone = 0.1f;

    // 애니메이션
    [SerializeField] Animator _animator;

    // 방향 전환 (Flip X)
    [SerializeField] SpriteRenderer _spriteRenderer;



    public void SetSpeed(float speed)
    {
        if (_animator == null) return;
        _animator.SetFloat("Speed", Mathf.Abs(speed));
    }

    public void SetJumping(bool value)
    {
        if (_animator == null) return;
        _animator.SetBool("isJumping", value);
    }

    public void SetFalling(bool value)
    {
        if (_animator == null) return;
        _animator.SetBool("isFalling", value);
    }

    // 입력 방향에 따라 스프라이트 Flip X 적용 
    public void UpdateFlip()
    {
        if (_spriteRenderer == null) return;
        if (HasMoveInput)
            _spriteRenderer.flipX = MoveInput < 0;
    }

    public bool IsFacingLeft => _spriteRenderer != null && _spriteRenderer.flipX;

    // 입력값을 저장하는 프로퍼티
    public float MoveInput { get; private set; }
    // MoveInput이 일정 값 이상일 때만 이동하도록 하는 프로퍼티
    public bool HasMoveInput => Mathf.Abs(MoveInput) >= _moveInputDeadZone;
    // 점프 중인지 여부를 저장하는 프로퍼티
    public float VelocityY
    {
        get
        {
            if (_rigidbody == null) return 0f;
            return _rigidbody.linearVelocity.y;
        }
    }
    // 점프 입력이 있는지 여부를 저장하는 프로퍼티
    public bool JumpInput { get; private set; }
    // 공격 입력
    public bool AttackInput { get; private set; }
    // 플레이어가 땅에 닿아있는지 여부를 저장하는 프로퍼티
    public bool IsGrounded { get; private set; }

    // 콤보 인덱스 (0=Attack1, 1=Attack2, 2=Attack3) - 모듈로 순환
    public int ComboIndex { get; private set; }
    private const int ComboLength = 3;

    public void ConsumeAttack() => AttackInput = false;

    //다음 콤보로 진행 (Attack1->2->3->1) 이런식으로 진행
    public void AdvanceCombo() => ComboIndex = (ComboIndex + 1) % ComboLength;

    // 콤보 리셋 (애니메이션 종료 시)
    public void ResetCombo() => ComboIndex = 0;

    [Header("Combat")]
    [SerializeField] int _maxHp = 6;
    [SerializeField] PlayerHpUI _hpUI;
    [SerializeField] float _attackDamage = 1f;
    [SerializeField] float _attackDamageCombo3 = 2f;
    [Tooltip("공격 판정 박스 가로(앞뒤)")]
    [SerializeField] float _attackRange = 1.5f;
    [Tooltip("공격 판정 박스 세로(위아래)")]
    [SerializeField] float _attackHeight = 1.2f;
    [Tooltip("플레이어 중심에서 판정 박스 시작 오프셋")]
    [SerializeField] float _attackOffsetX = 0.2f;
    [SerializeField, Range(0f, 1f)] float _hitWindowStart = 0.3f;
    [SerializeField, Range(0f, 1f)] float _hitWindowEnd = 0.55f;

    private bool _isDead;
    private bool _inputLocked; // 사망하면 입력 잠금
    private Vector3 _spawnPosition;



    public float GetAttackDamage()
    {
        if (ComboIndex == 2)
            return _attackDamageCombo3;
        return _attackDamage;
    }

    // 앞쪽 박스 범위 내 IDamageable에게 데미지. 플레이어 제외.
    public bool TryHitEnemies()
    {
        Vector2 pos = transform.position;
        float dir;
        if (IsFacingLeft)
            dir = -1f;
        else
            dir = 1f;
        Vector2 center = pos + Vector2.right * (dir * (_attackOffsetX + _attackRange * 0.5f));
        Vector2 size = new Vector2(_attackRange, _attackHeight);
        float angle = 0f;

        var hits = Physics2D.OverlapBoxAll(center, size, angle);
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            if (hit.TryGetComponent<IDamageable>(out var d))
            {
                d.TakeDamage(GetAttackDamage());
                return true;
            }
        }
        return false;
    }

    // 공격 기능 추가 할 거임 이제
    public void TriggerAttack()
    {
        if (_animator == null) return;
        _animator.SetInteger("AttackIndex", ComboIndex);
        _animator.SetTrigger("DoAttack");
    }

    // 피격 시 Hit 애니메이션 재생
    public void TriggerHitAnim()
    {
        if (_animator != null)
            _animator.SetTrigger("DoHit");
    }

    public bool IsAttackComplete()
    {
        if (_animator == null) return true;
        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);

        if (!info.IsName("Attack1") && !info.IsName("Attack2") && !info.IsName("Attack3"))
            return true;
        return info.normalizedTime >= 0.95f;
    }

    // 데미지 들어가는 구간
    public bool IsInAttackHitWindow()
    {
        if (_animator == null) return false;
        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        if (!info.IsName("Attack1") && !info.IsName("Attack2") && !info.IsName("Attack3"))
            return false;
        return info.normalizedTime >= _hitWindowStart && info.normalizedTime <= _hitWindowEnd;
    }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    // 바닥 확인
    private void GroundCheck()
    {
        if (_collider == null) return;

        Vector2 origin = (Vector2)transform.position + Vector2.down * _collider.bounds.extents.y;
        IsGrounded = Physics2D.Raycast(origin, Vector2.down, _groundCheckDistance, _groundLayer);
    }

    private void OnEnable()
    {
        _playerInput.onActionTriggered += HandleInput;
    }

    private void OnDisable()
    {
        _playerInput.onActionTriggered -= HandleInput;
    }

    private void Update()
    {
        // Dead 면 입력 잠금
        if (_isDead || _inputLocked) return;

        // Move는 Value 타입이라 폴링이 더 안정적 (onActionTriggered는 값 변경 시에만 호출됨)
        if (_playerInput?.actions != null)
        {
            var moveAction = _playerInput.actions["Move"];
            if (moveAction != null)
                MoveInput = moveAction.ReadValue<Vector2>().x;
        }
    }

    private void HandleInput(InputAction.CallbackContext context)
    {
        if (_isDead || _inputLocked) return;

        switch (context.action.name)
        {
            case "Jump":
                if (context.performed)
                    JumpInput = true;
                if (context.canceled) JumpInput = false;
                break;

            case "Attack":
                if (context.performed)
                    AttackInput = true;
                break;
        }
    }

    // 점프가 1번 실행 되면 JumpInput을 false로 바꿔서 점프가 1번만 실행 
    // 더블 점프도 넣어? 일단 점프는 1번만 하게 하고
    // 더블 점프도 가능하게 하고 싶으면 JumpInput을 false로 바꾸는 시점을 음,,,  어ㅓㅓ 
    // 점프가 끝난 후로 바꿔주면 됨
    // Enter 에서 넣어주면 될 
    public void ConsumeJump() => JumpInput = false;

    // Move()와 Jump()는 State에서 호출해서 실제로 플레이어를 움직이는 함수
    public void Move(float direction)
    {
        if (_isDead || _inputLocked) return;
        if (_rigidbody == null) return;
        _rigidbody.linearVelocity = new Vector2(direction * _moveSpeed, _rigidbody.linearVelocity.y);
    }

    public void Jump()
    {
        if (_isDead || _inputLocked) return;
        if (_rigidbody == null) return;
        _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    // 리지드바디 속도를 0으로 만들고
    // 플레이어가 미끄러지는거 방지
    public void Stop()
    {
        if (_rigidbody == null) return;
        _rigidbody.linearVelocity = new Vector2(0, _rigidbody.linearVelocity.y);
    }

    public int CurrentHp { get; private set; }
    public bool IsDead
    {
        get
        {
            return CurrentHp <= 0;
        }
    }


    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        CurrentHp = Mathf.Max(0, CurrentHp - Mathf.CeilToInt(damage));
        _hpUI?.SetHp(CurrentHp);

        if (CurrentHp <= 0)
        {
            Die();
        }
        else
        {
            TriggerHitAnim();
        }
    }

    public void SetDeadAnim(bool value)
    {
        if (_animator != null)
            _animator.SetBool("IsDead", value);
    }

    public void LockInput(bool value)
    {
        _inputLocked = value;

        if (value)
        {
            MoveInput = 0;
            JumpInput = false;
            AttackInput = false;
        }
    }

    public void StopAllMotion()
    {
        if (_rigidbody == null) return;

        _rigidbody.linearVelocity = Vector2.zero;
    }

    public void SetColliderEnabled(bool value)
    {
        if (_collider != null)
            _collider.enabled = value;
    }

    // Rigidbody를 Kinematic으로 전환. 사망 시 중력 영향 없음.
    public void SetBodyKinematic(bool kinematic)
    {
        if (_rigidbody == null) return;
        if (kinematic)
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        else
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        if (kinematic)
            _rigidbody.linearVelocity = Vector2.zero;
    }

    private void Die()
    {
        if (_isDead) return;

        _isDead = true;
        _inputLocked = true;

        SetBodyKinematic(true);
        if (_collider != null)
            _collider.enabled = false;

        if (_animator != null)
            _animator.SetBool("IsDead", true);
    }

    // 리스폰: 처음 위치로 이동, HP 회복, 사망 상태 해제.
    public void Respawn()
    {
        _isDead = false;
        _inputLocked = false;
        CurrentHp = _maxHp;
        _hpUI?.SetHp(CurrentHp);

        transform.position = _spawnPosition;
        SetBodyKinematic(false);
        if (_collider != null)
            _collider.enabled = true;
        if (_animator != null)
        {
            _animator.SetBool("IsDead", false);
            _animator.Play("Idle", 0, 0f); // Dead -> Idle 강제 전환 
        }
        StopAllMotion();
    }

    // 스폰 위치로만 이동 (HP 변경 없음). 낙하 데미지 등에서 사용.
    public void TeleportToSpawn()
    {
        transform.position = _spawnPosition;
        SetBodyKinematic(false);
        if (_collider != null)
            _collider.enabled = true;
        StopAllMotion();
    }

    private void Start()
    {
        _spawnPosition = transform.position;
        CurrentHp = _maxHp;
        _hpUI?.SetHp(CurrentHp);
    }

    /*
 private void OnDrawGizmosSelected()
 {
     // 공격 판정 박스 (빨강)
     float dir;
     if (_spriteRenderer != null && _spriteRenderer.flipX)
         dir = -1f;
     else
         dir = 1f;

     Vector2 center = (Vector2)transform.position + 
                      Vector2.right * (dir * (_attackOffsetX + _attackRange * 0.5f));

     Gizmos.color = Color.red;
     Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.identity, Vector3.one);
     Gizmos.DrawWireCube(Vector3.zero, new Vector3(_attackRange, _attackHeight, 0.01f));
     Gizmos.matrix = Matrix4x4.identity;

     // 지면 체크 (시안)
     Collider2D col;
     if (_collider != null)
         col = _collider;
     else
         col = GetComponent<Collider2D>();

     if (col != null)
     {
         Vector2 origin = (Vector2)transform.position + Vector2.down * col.bounds.extents.y;
         Gizmos.color = Color.cyan;
         Gizmos.DrawLine(origin, origin + Vector2.down * _groundCheckDistance);
     }
 }
 */
}