using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PCcomponent : MonoBehaviour
{
    public enum TYPE_OF_COMPONENT { MOTHER_BOARD, SIDE_CIVER };

    public Dictionary<string, string> Options { private set; get; } = new Dictionary<string, string>();
}
