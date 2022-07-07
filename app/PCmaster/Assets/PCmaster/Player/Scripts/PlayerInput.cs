using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public UnityEvent<Vector3> move = new();

    private PlayerControl _inputControler;

    private void Awake()
    {
        _inputControler = new PlayerControl();
    }

    private void Start()
    {
        _inputControler.Enable();
    }

    private void Update()
    {
        Vector3 buffer = _inputControler.Player.move.ReadValue<Vector3>();

        if (buffer != Vector3.zero)
        {
            move.Invoke(buffer);
        }
    }


    private void OnDestroy()
    {
        _inputControler.Disable();
    }
}
