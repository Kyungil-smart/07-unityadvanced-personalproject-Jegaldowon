using UnityEngine;

// 얘가 해야할 책임?
// State를 교체 관리
// 얘는 순수 C# 스크립트임
// 유니티에서 MonoBehaviour 의 상속을 받지 않음

public class StateMachine
{
    IState _currentState;

    // 전환 하고 실행 시킬 객체
    // 객체가 실행을 하는 거임 무말알?
    // 객체가 전환하고 실행 하는 거임

    public void ChangeState(IState iState)
    {
        _currentState?.Exit();
        _currentState = iState;
        _currentState.Enter();
    }

    // 상태를 바꿈 뭐가 필요함 -> Update
    // MonoBehaviour 상속 안받음 C# 스크립트임
    // Update 함수는 유니티 Update 랑 다름
    public void Update()
    {
        _currentState?.Update();
    }
}
