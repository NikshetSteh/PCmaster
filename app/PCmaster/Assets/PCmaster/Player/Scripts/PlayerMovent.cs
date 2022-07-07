using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovent : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private float _speed;

    [Space(1.0f)]
    [SerializeField] private float _gravity;

    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        _input.move.AddListener(Move);
    }

    private void FixedUpdate()
    {
        if (!_characterController.isGrounded)
        {
            _characterController.Move(new Vector3(0, -_gravity, 0));
        }
    }

    private void Move(Vector3 direction)
    {
        Quaternion t = new(direction.x, direction.y, direction.z, 0);

        t = transform.rotation * t;
        t *= Quaternion.Inverse(transform.rotation);

        _characterController.Move(new Vector3(t.x, t.y, t.z) * _speed);
    }
}
