using UnityEngine;

public class EnemyDataManager : MonoBehaviour
{
    [SerializeField] EnemyData _enemyData;


    private void Start()
    {
        GetEnemyData();
    }



    void GetEnemyData()
    {
        print($"Enemy Name: {_enemyData.EnemyName}");
        print($"Enemy Description: {_enemyData.EnemyDescription}");
        print($"Max HP: {_enemyData.MaxHp}");
        print($"Damage: {_enemyData.Damage}");
        print($"Move Speed: {_enemyData.MoveSpeed}");
    }


}
