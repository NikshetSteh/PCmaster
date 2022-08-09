using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputPC : PlayerInput
{
    private PlayerControl _inputController;

    private bool _isMoving;

    private const float MouseScrollSensity = -0.0005f;

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
        
        shift.Invoke(_inputController.Player.shift.ReadValue<float>() * MouseScrollSensity);
    }


    private void OnDestroy()
    {
        _inputController.Disable();
    }
}
