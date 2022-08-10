using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PcComponent : MonoBehaviour
{
    public enum PcComponentOptionsName
    {
        Width, Height, Long
    }

    [Serializable]
    public class PcComponentOption
    {
        [SerializeField] private PcComponentOptionsName _name;
        [SerializeField] private bool _isInt;
        [SerializeField] private string _value;
        
        public PcComponentOptionsName Name => _name;
        public bool IsInt => _isInt;
        public string Value => _value;
    }

    public enum TypeOfComponent
    {
        MotherBoard, SideCover
    }

    [SerializeField] private PcComponentOption[] _options;
    private readonly Dictionary<PcComponentOptionsName, PcComponentOption> _optionsDict = new();

    [SerializeField] private TypeOfComponent _type;
    public TypeOfComponent Type => _type;
    
    [SerializeField] private Collider _collider;

    private Rigidbody _rigidbody;

    private bool _isTaken;
    private SpaceForComponents _nowSpaceForComponents;

    private void Awake()
    {
        if (_options != null)
            foreach (PcComponentOption i in _options)
            {
                _optionsDict.Add(i.Name, i);
            }
        
        _rigidbody = GetComponent<Rigidbody>();
    }

    public Dictionary<PcComponentOptionsName, PcComponentOption> GetOptions()
    {
        return _optionsDict;
    }
    
    public SpaceForComponents.ErrorRemovePcComponents TryTake()
    {
        if (_nowSpaceForComponents)
        {
            SpaceForComponents.ErrorRemovePcComponents error = _nowSpaceForComponents.TryRemoveNowComponent();
            
            if (error == SpaceForComponents.ErrorRemovePcComponents.Null)
            {
                Take();
                
                return SpaceForComponents.ErrorRemovePcComponents.Null;
            }
            else
            {
                return error;
            }
        }

        Take();

        return SpaceForComponents.ErrorRemovePcComponents.Null;
    }

    public void Put()
    {
        _collider.enabled = true;

        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;

        _rigidbody.velocity = new Vector3();
    }

    public void PutToPc(SpaceForComponents spaceForComponents)
    {
        _collider.enabled = true;

        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = false;

        _nowSpaceForComponents = spaceForComponents;
    }

    private void Take()
    {
        _collider.enabled = false;
        
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;

        if (_nowSpaceForComponents)
        {
            _nowSpaceForComponents = null;
        }
    }
}
