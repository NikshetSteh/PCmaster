using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputPC : PlayerInput
{
    private PlayerControl _inputController;

    private bool _isMoving;

    private void Awake()
    {
        _inputController = new PlayerControl();
    }

    private void Start()
    {
        _inputController.Enable();

        _inputController.Player.jump.performed += _ => jump.Invoke();
        _inputController.Player.click.performed += _ => click.Invoke();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector3 buffer = _inputController.Player.move.ReadValue<Vector3>();

        if (buffer != Vector3.zero)
        {
            move.Invoke(buffer);
            _isMoving = true;
        }
        else if (_isMoving)
        {
            move.Invoke(buffer);
        }

        turn.Invoke(Mouse.current.delta.ReadValue());
    }


    private void OnDestroy()
    {
        _inputController.Disable();
    }
}
