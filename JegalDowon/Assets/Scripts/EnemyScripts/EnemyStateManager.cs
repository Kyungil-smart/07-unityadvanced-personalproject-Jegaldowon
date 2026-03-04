using UnityEngine;
using System.Collections;


// 몬스터 상태 머신. StateMachine + IState 재사용 (PlayerStateMachine과 동일 패턴)
public class EnemyStateManager : MonoBehaviour, IDamageable
{
    [Header("적 설정")]
    [Tooltip("HP/사거리/속도 등 (필수)")]
    [SerializeField] EnemyData _enemyData;
    [Tooltip("추적할 플레이어")]
    [SerializeField] Transform _player;

    [Header("애니메이션")]
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [Tooltip("스프라이트 방향이 반대로 보이면 체크")]
    [SerializeField] bool _invertFacing;

    [Header("사망")]
    [Tooltip("Dead 애니메이션 재생 시간(초) 후 Destroy")]
    [SerializeField] float _deadAnimDuration = 1f;

    [Header("낭떨어지 감지")]
    [SerializeField] LayerMask _groundLayer;
    [Tooltip("발 앞쪽 체크 거리")]
    [SerializeField] float _edgeCheckDistance = 0.4f;
    [Tooltip("아래로 쏘는 레이 길이")]
    [SerializeField] float _edgeCheckDown = 0.5f;

    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private StateMachine _stateMachine;
    private Vector3 _patrolOrigin;
    private bool _isDead;

    public float CurrentHp { get; private set; }
    public bool IsDead => _isDead;
    public Vector3 PatrolOrigin => _patrolOrigin;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_animator == null) _animator = GetComponent<Animator>();
        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Transform Player
    {
        get
        {
            return _player;
        }
    }


    public EnemyData EnemyDataForGizmo
    {
        get
        {
            return _enemyData;
        }
    }

    public float ChaseRange
    {
        get
        {
            if (_enemyData != null)
            {
                return _enemyData.ChaseRange;
            }
            else
            {
                return 5f;
            }
        }
    }
    public float AttackRange
    {
        get
        {
            if (_enemyData != null)
            {
                return _enemyData.AttackRange;
            }
            else
            {
                return 1.5f;
            }
        }
    }
    public float Speed
    {
        get
        {
            if (_enemyData != null)
            {
                return _enemyData.MoveSpeed;
            }
            else
            {
                return 3f;
            }
        }
    }
    public float PatrolSpeed
    {
        get
        {
            if (_enemyData != null)
            {
                return _enemyData.PatrolSpeed;
            }
            else
            {
                return 1.5f;
            }
        }
    }
    public float PatrolDecisionMin
    {
        get
        {
            if (_enemyData != null)
            {
                return _enemyData.PatrolDecisionMin;
            }
            else
            {
                return 1f;
            }
        }
    }

    public float PatrolDecisionMax
    {
        get
        {
            if (_enemyData != null)
            {
                return _enemyData.PatrolDecisionMax;
            }
            else
            {
                return 3f;
            }
        }
    }

    public float PatrolRadius
    {
        get
        {
            if (_enemyData != null)
            {
                return _enemyData.PatrolRadius;
            }
            else
            {
                return 0f;
            }
        }
    }
    public void SetAnimatorSpeed(float speed)
    {
        if (_animator != null)
            _animator.SetFloat("Speed", speed);
    }

    public void TriggerAttackAnim()
    {
        if (_animator != null)
            _animator.SetTrigger("DoAttack");
    }

    public void TriggerHitAnim()
    {
        if (_animator != null)
            _animator.SetTrigger("DoHit");
    }

    public void SetDead(bool value)
    {
        if (_animator != null)
            _animator.SetBool("IsDead", value);
    }

    public void SetFacing(bool faceLeft)
    {
        if (_spriteRenderer != null)
            _spriteRenderer.flipX = _invertFacing ? !faceLeft : faceLeft;
    }

    public bool IsInAttackAnim()
    {
        if (_animator == null) return false;
        return _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }

    // 공격 애니메이션 중 데미지 판정 구간인지
    public bool IsInAttackHitWindow()
    {
        if (_animator == null) return false;
        var info = _animator.GetCurrentAnimatorStateInfo(0);
        if (!info.IsName("Attack")) return false;
        return info.normalizedTime >= 0.15f && info.normalizedTime <= 0.85f;
    }

    public float Damage => _enemyData != null ? _enemyData.Damage : 1f;

    void Start()
    {
        _patrolOrigin = transform.position;
        CurrentHp = _enemyData != null ? _enemyData.MaxHp : 10f;
        _stateMachine = new StateMachine();
        _stateMachine.ChangeState(new EnemyPatrolState(this, _stateMachine));
    }

    // IDamageable 인터페이스 구현 
    // 코루틴 사용해서 사망 애니메이션 재생 후 Destroy 하도록
    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        CurrentHp -= damage;
        if (CurrentHp <= 0)
        {
            _isDead = true;
            if (_collider != null) _collider.enabled = false;
            if (_rigidbody != null)
            {
                _rigidbody.bodyType = RigidbodyType2D.Kinematic;
                _rigidbody.linearVelocity = Vector2.zero;
            }
            SetDead(true);
            StartCoroutine(PlayDeadAndDestroy());
        }
        // 죽을 때 까지(?) 맞는 애니메이션은 계속 나와야 함 
        else
        {
            TriggerHitAnim();
        }
    }

    IEnumerator PlayDeadAndDestroy()
    {
        if (_stateMachine != null)
            _stateMachine.ChangeState(new EnemyDeadState(this, _stateMachine));

        yield return new WaitForSeconds(_deadAnimDuration);
        Destroy(gameObject);
    }

    void Update()
    {
        if (!_isDead && _stateMachine != null)
            _stateMachine.Update();
    }

    public void ChangeState(IState newState)
    {
        _stateMachine.ChangeState(newState);
    }

    public bool IsPlayerRange(float range)
    {
        if (_player == null)
            return false;

        float distance = Vector3.Distance(transform.position, _player.position);

        if (distance < range)
            return true;

        return false;
    }


    // moveDirection: -1(왼쪽), 0(정지), 1(오른쪽)
    public bool IsGroundAhead(float moveDirection)
    {
        if (moveDirection == 0)
            return true;

        float footY;

        if (_collider != null)
        {
            footY = transform.position.y - _collider.bounds.extents.y;
        }
        else
        {
            footY = transform.position.y;
        }

        // 발 앞쪽 위치 계산 
        Vector2 origin = new Vector2(
            transform.position.x + moveDirection * _edgeCheckDistance,
            footY
        );
        // 낭떠러지 감지 레이 쏴서 감지 
        return Physics2D.Raycast(origin, Vector2.down, _edgeCheckDown, _groundLayer);
    }


    // 다 만들었으면 여기 지울 것
    private void OnDrawGizmosSelected()
    {
        float chaseR = _enemyData != null ? _enemyData.ChaseRange : 5f;
        float attackR = _enemyData != null ? _enemyData.AttackRange : 1.5f;
        float patrolR = _enemyData != null ? _enemyData.PatrolRadius : 0f;

        // 추적 범위는 노랑으라 하고
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseR);
        // 공격 범위는 빨강 으로 하고
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackR);
        // 패트롤 범위 (초록, 반경 > 0일 때만)
        if (patrolR > 0)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, patrolR);
        }
        // 낭떨어지 감지 (시안)
        Gizmos.color = Color.cyan;
        var col = _collider != null ? _collider : GetComponent<Collider2D>();
        float footY = col != null ? transform.position.y - col.bounds.extents.y : transform.position.y;
        Gizmos.DrawLine(new Vector3(transform.position.x + _edgeCheckDistance, footY), new Vector3(transform.position.x + _edgeCheckDistance, footY - _edgeCheckDown));
        Gizmos.DrawLine(new Vector3(transform.position.x - _edgeCheckDistance, footY), new Vector3(transform.position.x - _edgeCheckDistance, footY - _edgeCheckDown));
    }
}
