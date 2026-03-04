using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string EnemyName;

    [TextArea]
    public string EnemyDescription;


    public float MaxHp;
    public float Damage;
    public float MoveSpeed;

}
