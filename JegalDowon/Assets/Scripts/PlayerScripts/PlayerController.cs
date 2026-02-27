using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Collider2D _collider;

    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _groundCheckDistance = 0.1f;


    // 키 입력 되고 이제 rb 로 움직이게 할 거임
    private Rigidbody2D _rigidbody;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _jumpForce = 12f;


    public float MoveInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void GroundCheck()
    {
        if (_collider == null) return;

        Vector2 origin = (Vector2)transform.position + Vector2.down * _collider.bounds.extents.y;
        IsGrounded = Physics2D.Raycast(origin, Vector2.down, _groundCheckDistance, _groundLayer);
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
                if (context.canceled) JumpInput = false;


                break;
        }
    }

    // 점프가 1번 실행 되면 JumpInput을 false로 바꿔서 점프가 1번만 실행 
    // 더블 점프도 넣어? 일단 점프는 1번만 하게 하고
    // 더블 점프도 가능하게 하고 싶으면 JumpInput을 false로 바꾸는 시점을 음,,,  어ㅓㅓ 
    // 점프가 끝난 후로 바꿔주면 됨
    // Enter 에서 넣어주면 될 
    public void ConsumeJump() => JumpInput = false;

    // Move()와 Jump()는 State에서 호출해서 실제로 플레이어를 움직이는 함수
    public void Move(float direction)
    {
        if (_rigidbody == null) return;
        _rigidbody.linearVelocity = new Vector2(direction * _moveSpeed, _rigidbody.linearVelocity.y);
    }

    public void Jump()
    {
        if (_rigidbody == null) return;
        _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    // 리지드바디 속도를 0으로 만들고
    // 플레이어가 미끄러지는거 방지
    public void Stop()
    {
        if (_rigidbody == null) return;
        _rigidbody.linearVelocity = new Vector2(0, _rigidbody.linearVelocity.y);
    }

}