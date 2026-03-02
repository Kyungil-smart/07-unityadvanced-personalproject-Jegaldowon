using UnityEngine;

/// <summary>
/// 카메라가 타겟(캐릭터)을 따라가게 합니다.
/// Main Camera에 붙이고 Target에 플레이어를 할당하세요.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _smoothSpeed = 5f;
    [SerializeField] Vector3 _offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (_target == null) return;

        Vector3 desired = _target.position + _offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, desired, _smoothSpeed * Time.deltaTime);
        transform.position = smoothed;
    }
}
