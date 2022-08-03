using System;
using UnityEngine;

public class SpaceForComponents : MonoBehaviour
{
    [Serializable]
    public class OptionsRequirement
    {
        [SerializeField] private string _name;
        [SerializeField] private string _min;
        [SerializeField] private string _max;
    }

    [SerializeField] private PcComponent.TypeOfComponent _typeOfComponent;
    public PcComponent.TypeOfComponent TypeOfComponent => _typeOfComponent;

    public bool IsFull { private set; get; } = false;

    [SerializeField] private OptionsRequirement[] _optionsRequirements;

    [SerializeField] private bool _autoFull;

    private GameObject _nowComponent;

    public bool TrySetComponent(GameObject component)
    {
        if (IsFull)
            return false;
        
        _nowComponent = component;
        IsFull = true;

        _nowComponent.GetComponent<Rigidbody>().isKinematic = true;
        _nowComponent.GetComponent<Rigidbody>().useGravity = false;

        _nowComponent.transform.parent = transform;
        
        return true;
    }

}
