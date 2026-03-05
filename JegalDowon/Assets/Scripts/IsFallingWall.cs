using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class IsFallingWall : MonoBehaviour
{

    // 그냥 가라로 만들었음
    // 플레이어가 IsFallingWall 데미지 -1, 다시 스폰
    [SerializeField] float _damage = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponentInParent<PlayerController>();
        if (player == null) return;

        if (player.IsDead) return;

        player.TakeDamage(_damage);
        if (!player.IsDead)
            player.TeleportToSpawn();
    }
}
