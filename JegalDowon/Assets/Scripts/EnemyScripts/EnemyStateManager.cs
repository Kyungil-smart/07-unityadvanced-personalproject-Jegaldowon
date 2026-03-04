using UnityEngine;

/// <summary>
/// 몬스터 상태 머신. StateMachine + IState 재사용 (PlayerStateMachine과 동일 패턴)
/// </summary>
public class EnemyStateManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] Transform _player;
    [SerializeField] float _chaseRange = 5f;
    [SerializeField] float _attackRange = 1.5f;
    [SerializeField] float _speed = 3f;

    private StateMachine _stateMachine;

    public Transform Player => _player;
    public float ChaseRange => _chaseRange;
    public float AttackRange => _attackRange;
    public float Speed => _speed;

    void Start()
    {
        _stateMachine = new StateMachine();
        _stateMachine.ChangeState(new EnemyIdleState(this, _stateMachine));
    }

    void Update()
    {
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

    }
}
