using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpaceForComponents : MonoBehaviour
{
    [Serializable]
    public class OptionsRequirement
    {
        [SerializeField] private string name;
        public string min;
        public string max;
    }

    [HideInInspector] public PCcomponent.TYPE_OF_COMPONENT TypeOfComponent => _typeOfComponent;

    [SerializeField] private PCcomponent.TYPE_OF_COMPONENT _typeOfComponent;

    public bool IsFull { private set; get; } = false;

    [SerializeField] private OptionsRequirement[] _optionsRequirements;


}
