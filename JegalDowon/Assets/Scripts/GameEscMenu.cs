using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameEscMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _escPanel;
    [SerializeField] private Button _quitButton;

    [Header("Input")]
    [SerializeField] private InputActionAsset _inputActions;

    private InputAction _escAction;
    private bool _isOpen;

    private void Awake()
    {
        if (_escPanel != null)
            _escPanel.SetActive(false);

        if (_quitButton != null)
            _quitButton.onClick.AddListener(OnQuitClick);

        if (_inputActions != null)
            _escAction = _inputActions.FindAction("PlayerActions/ESC");
    }

    private void OnEnable()
    {
        if (_escAction == null) return;

        _escAction.performed += OnEscPerformed;
        _escAction.Enable();
    }

    private void OnDisable()
    {
        if (_escAction == null) return;

        _escAction.performed -= OnEscPerformed;
        _escAction.Disable();
    }

    private void OnEscPerformed(InputAction.CallbackContext ctx)
    {
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