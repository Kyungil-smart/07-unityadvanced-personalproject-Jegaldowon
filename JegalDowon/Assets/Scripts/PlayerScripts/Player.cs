using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInput _playerInput;

    public float MoveInput { get; private set; }
    public bool JumpInput { get; private set; }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _playerInput.onActionTriggered += HandleInput;
    }

    private void OnDisable()
    {
        _playerInput.onActionTriggered -= HandleInput;
    }

    private void HandleInput(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Move":
                MoveInput = context.ReadValue<Vector2>().x;
                Debug.Log($"Move Input: {MoveInput}");
                break;

            case "Jump":
                if (context.performed)
                {
                    JumpInput = true;
                    Debug.Log($"Jump Input: {JumpInput}");
                }
                break;
        }
    }

    public void ConsumeJump() => JumpInput = false;
}