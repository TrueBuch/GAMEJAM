using UnityEngine;
using UnityEngine.Events;

public class Input : MonoBehaviour, ISingleton
{
    private PlayerInput _playerInput;
    public readonly UnityEvent LeftClickStarted = new();
    public readonly UnityEvent LeftClickPressed = new();
    public readonly UnityEvent LeftClickCancelled = new();

    public readonly UnityEvent RightClickStarted = new();
    public readonly UnityEvent RightClickPressed = new();
    public readonly UnityEvent RightClickCancelled = new();

    public readonly UnityEvent ReloadPerformed = new();

    private Vector2 _scroll;
    public Vector2 Scroll => _scroll;
    private Vector2 _mousePosition;
    public Vector2 MousePosition => _mousePosition;

    private Vector2 _mouseDelta;
    public Vector2 MouseDelta => _mouseDelta;

    public void Awake()
    {
        _playerInput = new();
        _playerInput.Enable();

        _playerInput.Player.LeftClick.started += ctx => LeftClickStarted.Invoke();
        _playerInput.Player.LeftClick.canceled += ctx => LeftClickCancelled.Invoke();

        _playerInput.Player.RightClick.started += ctx => RightClickStarted.Invoke();
        _playerInput.Player.RightClick.canceled += ctx => RightClickCancelled.Invoke();

        _playerInput.Player.Reload.performed += ctx => ReloadPerformed.Invoke();
    }

    public void Initialize()
    {
        
    }

    private void Update()
    {
        _scroll = _playerInput.Player.Scroll.ReadValue<Vector2>();
        _mousePosition = _playerInput.Player.MousePosition.ReadValue<Vector2>();

        _mouseDelta = _playerInput.Player.MouseDelta.ReadValue<Vector2>();
        
        if (_playerInput.Player.LeftClick.IsPressed()) LeftClickPressed?.Invoke();
        if (_playerInput.Player.RightClick.IsPressed()) RightClickPressed?.Invoke();
    }

    private void OnDisable()
    {
        _playerInput.Player.LeftClick.started -= ctx => LeftClickStarted?.Invoke();
        _playerInput.Player.LeftClick.canceled -= ctx => LeftClickCancelled?.Invoke();

        _playerInput.Player.RightClick.started -= ctx => RightClickStarted?.Invoke();
        _playerInput.Player.RightClick.canceled -= ctx => RightClickCancelled?.Invoke();

        _playerInput.Disable();
    }
}