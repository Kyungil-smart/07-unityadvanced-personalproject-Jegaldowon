using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string EnemyName;

    [TextArea]
    public string EnemyDescription;

    [Header("전투")]
    public float MaxHp = 10f;
    public float Damage = 1f;

    [Header("사거리")]
    [Tooltip("플레이어 감지 시 추적 시작 거리")]
    public float ChaseRange = 5f;
    [Tooltip("공격 시작 거리")]
    public float AttackRange = 1.5f;

    [Header("이동")]
    [Tooltip("플레이어 추적 시 이동 속도")]
    public float MoveSpeed = 3f;
    [Tooltip("패트롤 시 이동 속도")]
    public float PatrolSpeed = 1.5f;
    [Tooltip("패트롤 반경 (0이면 제한 없음)")]
    public float PatrolRadius = 0f;

    [Header("패트롤 행동")]
    [Tooltip("멈출지/움직일지 재결정 최소 간격(초)")]
    public float PatrolDecisionMin = 1f;
    [Tooltip("멈출지/움직일지 재결정 최대 간격(초)")]
    public float PatrolDecisionMax = 3f;
}
