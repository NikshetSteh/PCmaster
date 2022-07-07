using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovent : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;

    [Space(1.0f)]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    [Space(1.0f)]
    [SerializeField] private float _gravity;

    [SerializeField] private Vector3 _velosity = new();

    private CharacterController _characterController;
    private bool _isJumping = false;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        _input.move.AddListener(Move);
        _input.jump.AddListener(Jump);
    }

    private void FixedUpdate()
    {
        if (!_characterController.isGrounded)
        {
            _velosity.y += _gravity * Time.fixedDeltaTime;
        }
        else if (!_isJumping)
        { 
            _velosity.y = 0;
        }
        else
        {
            _isJumping = false;
        }

        _characterController.Move(_velosity);
    }

    private void Move(Vector3 direction)
    {
        if (_characterController.isGrounded)
        {
            Quaternion t = new(direction.x, direction.y, direction.z, 0);

            t = transform.rotation * t;
            t *= Quaternion.Inverse(transform.rotation);

            direction = new Vector3(t.x, t.y, t.z) * _speed;

            //_characterController.Move(new Vector3(t.x, t.y, t.z) * _speed);

            _velosity = new Vector3(direction.x, _velosity.y, direction.z);
        }
    }

    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            _velosity.y = _jumpForce;
            _isJumping = true;
        }
    }
}
