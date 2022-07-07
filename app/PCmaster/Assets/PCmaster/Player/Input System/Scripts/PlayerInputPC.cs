using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    }


    private void OnDestroy()
    {
        _inputControler.Disable();
    }

    private void Jump()
    {
        jump.Invoke();
    }
}
