using UnityEngine;

public class PlayerDeadState : IState
{
    private readonly PlayerController _player;

    public PlayerDeadState(PlayerController player, StateMachine sm)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.SetDeadAnim(true);
        _player.LockInput(true);
        _player.SetBodyKinematic(true);
        _player.StopAllMotion();
        _player.SetColliderEnabled(false);
    }

    public void Exit() { }

    public void Update() { }
}