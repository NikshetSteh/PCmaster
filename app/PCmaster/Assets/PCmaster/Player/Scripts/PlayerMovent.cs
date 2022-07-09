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

    [Space(1.0f)]
    [SerializeField] private Transform _camera;

    [Header("Rotation settings")]
    [SerializeField] private float _rotationSensity;
    [SerializeField] private float _minRotationAngle;
    [SerializeField] private float _maxRotationAngle;

    private Vector3 _velosity = new();

    private CharacterController _characterController;
    private bool _isJumping = false;

    private float _cameraPitch = 0f;

    private bool _isGround = false;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        _input.move.AddListener(Move);
        _input.jump.AddListener(Jump);
        _input.turn.AddListener(Turn);
    }

    private void FixedUpdate()
    {
        if (!_isGround)
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

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            _isGround = true;
            print(other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isGround = false;
    }

    private void Move(Vector3 direction)
    {
        if (_isGround)
        {
            Quaternion t = new(direction.x, direction.y, direction.z, 0);

            t = transform.rotation * t;
            t *= Quaternion.Inverse(transform.rotation);

            direction = new Vector3(t.x, t.y, t.z) * _speed;

            _velosity = new Vector3(direction.x, _velosity.y, direction.z);
        }
    }


    private void Jump()
    {
        if (_isGround)
        {
            _velosity.y = _jumpForce;
            _isJumping = true;
        }
    }

    private void Turn(Vector2 delta)
    {
        transform.Rotate(_rotationSensity * delta.x * Vector3.up);

        _cameraPitch -= delta.y * _rotationSensity;

        _cameraPitch = Mathf.Clamp(_cameraPitch, _maxRotationAngle, _minRotationAngle);

        _camera.localEulerAngles = new Vector3(_cameraPitch, 0, 0);
    }
}
