using UnityEngine;

// MonoBehaviour - 유니티 생명주기 담당
// StateMachine.Update()를 유니티 Update에서 호출해줌

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    StateMachine _stateMachine;

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
        _stateMachine.Update();
    }
}
