using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// ESC 누르면 EscPanel 토글, 게임 정지/재개. Button 누르면 게임 종료.
/// Input System ESC 액션 사용. PlayerControllers 에셋 할당.
/// </summary>
public class GameEscMenu : MonoBehaviour
{
    [SerializeField] GameObject _escPanel;
    [SerializeField] Button _quitButton;
    [SerializeField] InputActionAsset _inputActions;

    private InputAction _escAction;
    private bool _isOpen;

    private void OnEnable()
    {
        if (_inputActions != null)
        {
            _escAction = _inputActions.FindActionMap("PlayerActions").FindAction("ESC");
            _escAction?.Enable();
        }
    }

    private void OnDisable()
    {
        _escAction?.Disable();
    }

    private void Start()
    {
        if (_escPanel != null)
            _escPanel.SetActive(false);

        if (_quitButton != null)
            _quitButton.onClick.AddListener(OnQuitClick);
    }

    private void Update()
    {
        if (_escAction != null && _escAction.WasPressedThisFrame())
            Toggle();
    }

    private void Toggle()
    {
        _isOpen = !_isOpen;

        if (_escPanel != null)
            _escPanel.SetActive(_isOpen);

        Time.timeScale = _isOpen ? 0f : 1f;
    }

    private void OnQuitClick()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
