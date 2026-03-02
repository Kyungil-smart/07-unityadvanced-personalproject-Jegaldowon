using UnityEngine;

public class AttackState : IState
{
    PlayerController _player;

    StateMachine _stateMachine;


    public AttackState(PlayerController playerController, StateMachine stateMachine)
    {
        _player = playerController;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public void Update()
    {

    }
}
