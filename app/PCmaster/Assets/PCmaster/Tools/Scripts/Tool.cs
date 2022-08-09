using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    public enum ToolsTypes
    {
        
    }
    
    public enum ToolCharacteristicsNames
    {
        
    }

    [Serializable]
    public class ToolCharacteristic
    {
        [SerializeField] private ToolCharacteristicsNames _name;
        [SerializeField] private bool _isInt;
        [SerializeField] private string _value;
        
        public ToolCharacteristicsNames Name => _name;
        public bool IsInt => _isInt;
        public string Value => _value;
    }

    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private ToolsTypes _type;
    public ToolsTypes Type => _type;

    [SerializeField] private ToolCharacteristic[] _toolCharacteristics;
    [SerializeField] private Collider _collider;

    public Dictionary<ToolCharacteristicsNames, ToolCharacteristic> ToolCharacteristics
    {
        get;
        protected set;
    }

    private void Awake()
    {
        
        if (_toolCharacteristics != null)
            foreach (ToolCharacteristic i in _toolCharacteristics)
            {
                ToolCharacteristics.Add(i.Name, i);
            }
    }

    public virtual void Take()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        
        _collider.enabled = false;
    }

    public virtual void Put()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;

        _collider.enabled = true;
    }

    public virtual bool Use(Fastening fastening)
    {
        return true;
    } 
}