using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerInput : MonoBehaviour
{
    public readonly UnityEvent<Vector3> move = new();
    public readonly UnityEvent jump = new();
    public readonly UnityEvent<Vector2> turn = new();

    public readonly UnityEvent<float> shift = new();

    public readonly UnityEvent click = new();
}
