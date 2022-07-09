using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public UnityEvent<Vector3> move = new();
    [HideInInspector] public UnityEvent jump = new();
    [HideInInspector] public UnityEvent<Vector2> turn = new();
}
