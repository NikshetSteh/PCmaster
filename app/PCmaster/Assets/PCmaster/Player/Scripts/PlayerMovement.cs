using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;

    [Space]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    [Space]
    [SerializeField] private Transform _camera;

    [Header("Rotation settings")]
    [SerializeField] private float _rotationSensitive;
    [SerializeField] private float _minRotationAngle;
    [SerializeField] private float _maxRotationAngle;

    private Rigidbody _rigidbody;

    private float _cameraPitch;

    private bool _isGround;

    private int _collisionNumber;

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
            _collisionNumber++;
            _isGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            _collisionNumber--;
            if (_collisionNumber == 0)
            {
                _isGround = false;
            }
        }
    }

    private void OnDisable()
    {
        _input.move.RemoveListener(Move);
        _input.jump.RemoveListener(Jump);
        _input.turn.RemoveListener(Turn);
    }

    private void Move(Vector3 direction)
    {
        if (_isGround)
        {
            Quaternion t = new(direction.x, direction.y, direction.z, 0);

            var rotation = transform.rotation;
            t = rotation * t;
            t *= Quaternion.Inverse(rotation);

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
        transform.Rotate(_rotationSensitive * delta.x * Vector3.up);

        _cameraPitch -= delta.y * _rotationSensitive;

        _cameraPitch = Mathf.Clamp(_cameraPitch, _maxRotationAngle, _minRotationAngle);

        _camera.localEulerAngles = new Vector3(_cameraPitch, 0, 0);
    }
}
