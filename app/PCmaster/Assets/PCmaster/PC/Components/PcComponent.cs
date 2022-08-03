using System.Collections.Generic;
using UnityEngine;

public abstract class PcComponent : MonoBehaviour
{
    public enum TypeOfComponent { MotherBoard, SideCover };

    public Dictionary<string, string> Options { private set; get; } = new Dictionary<string, string>();
}
