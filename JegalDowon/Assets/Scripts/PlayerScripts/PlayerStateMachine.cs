using UnityEngine;
using System.Collections;

// MonoBehaviour - 유니티 생명주기 담당
// StateMachine.Update()를 유니티 Update에서 호출해줌

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] float _deadAnimDuration = 1.5f;

    StateMachine _stateMachine;
    private bool _isInDeadState;

    private void Awake()
    {
        _stateMachine = new StateMachine();
    }

    private void Start()
    {
        // 시작 상태: Idle
        _stateMachine.ChangeState(new IdleState(_playerController, _stateMachine));
    }

    private void Update()
    {
        if (_playerController.IsDead && !_isInDeadState)
        {
            _isInDeadState = true;
            _stateMachine.ChangeState(new PlayerDeadState(_playerController, _stateMachine));
            StartCoroutine(RespawnAfterDeadAnim());
        }
        _playerController.UpdateFlip();
        _stateMachine.Update();
    }

    private IEnumerator RespawnAfterDeadAnim()
    {
        yield return new WaitForSeconds(_deadAnimDuration);

        _playerController.Respawn();
        _isInDeadState = false;
        _stateMachine.ChangeState(new IdleState(_playerController, _stateMachine));
    }
}
