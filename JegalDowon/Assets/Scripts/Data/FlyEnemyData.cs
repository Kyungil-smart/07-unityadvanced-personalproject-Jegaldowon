using UnityEngine;

[CreateAssetMenu(fileName = "New Fly Enemy Data", menuName = "Enemy Data/Fly Enemy Data")]
public class FlyData : EnemyData
{
    [Header("비행")]
    [Tooltip("수직 진동 범위")]
    public float FlyRange = 1f;
    [Tooltip("수직 진동 속도")]
    public float FlySpeed = 2f;
}
