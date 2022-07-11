using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovent : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;

    [Space(1.0f)]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    [Space(1.0f)]
    [SerializeField] private Transform _camera;

    [Header("Rotation settings")]
    [SerializeField] private float _rotationSensity;
    [SerializeField] private float _minRotationAngle;
    [SerializeField] private float _maxRotationAngle;

    private Rigidbody _rigidbody;

    private float _cameraPitch = 0f;

    private bool _isGround = false;

    private int _collisonsNumber = 0;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _input.move.AddListener(Move);
        _input.jump.AddListener(Jump);
        _input.turn.AddListener(Turn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            _collisonsNumber++;
            _isGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            _collisonsNumber--;
            if (_collisonsNumber == 0)
            {
                _isGround = false;
            }
        }
    }

    private void Move(Vector3 direction)
    {
        if (_isGround)
        {
            Quaternion t = new(direction.x, direction.y, direction.z, 0);

            t = transform.rotation * t;
            t *= Quaternion.Inverse(transform.rotation);

            direction = new Vector3(t.x, t.y, t.z) * _speed;

            _rigidbody.velocity = new Vector3(direction.x, _rigidbody.velocity.y, direction.z);
        }
    }


    private void Jump()
    {
        if (_isGround)
        {
            _rigidbody.AddForce(_jumpForce * Vector3.up);
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
