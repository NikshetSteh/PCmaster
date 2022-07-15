using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputPC : PlayerInput
{
    private PlayerControl _inputControler;

    private bool _isMoving = false;

    private void Awake()
    {
        _inputControler = new PlayerControl();
    }

    private void Start()
    {
        _inputControler.Enable();

        _inputControler.Player.jump.performed += contex => Jump();
        _inputControler.Player.click.performed += contex => Click();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector3 buffer = _inputControler.Player.move.ReadValue<Vector3>();

        if (buffer != Vector3.zero)
        {
            move.Invoke(buffer);
            _isMoving = true;
        }else if (_isMoving)
        {
            move.Invoke(buffer);
        }

         turn.Invoke(Mouse.current.delta.ReadValue());
    }


    private void OnDestroy()
    {
        _inputControler.Disable();
    }

    private void Jump()
    {
        jump.Invoke();
    }

    private void Click()
    {
        click.Invoke();
    }
}
