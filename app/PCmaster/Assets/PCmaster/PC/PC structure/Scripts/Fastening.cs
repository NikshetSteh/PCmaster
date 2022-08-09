using System;
using System.Linq;
using UnityEngine;

public abstract class Fastening : MonoBehaviour
{
    public enum FasteningTypes
    {
        
    }
    
    [Serializable]
    public class ToolOptionsRequirement
    {
        [SerializeField] private Tool.ToolCharacteristicsNames _name;
        [SerializeField] private bool _isInt;
        [SerializeField] private string[] _suitableValues;
        [SerializeField] private double _min;
        [SerializeField] private double _max;

        public Tool.ToolCharacteristicsNames Name => _name;

        public bool Test(Tool.ToolCharacteristic option)
        {
            if (option.Name != _name)
                throw new Exception("PcComponent option name is not name of option requirement");
            
            if(option.IsInt != _isInt)
                throw new Exception("PcComponent type is not type of option requirement");

            if (_isInt)
            {
                return _min <= int.Parse(option.Value) && _max >= int.Parse(option.Value);
            }
            else
            {
                return _suitableValues.Contains(option.Value);
            }
        }
    }

    [SerializeField] protected Tool.ToolsTypes _type;

    [SerializeField] protected ToolOptionsRequirement[] _minToolCharacteristics;
    
    //TODO: remove Serializable
    [SerializeField] protected bool _isLocked;

    [SerializeField] protected SpaceForComponents _spaceForComponents;
    [SerializeField] protected Collider _collider;

    public bool IsLocked => _isLocked;

    private void Awake()
    {
        _spaceForComponents.setNewComponent.AddListener(SetNewComponent);
        _spaceForComponents.removeComponent.AddListener(RemoveComponent);
    }

    public virtual bool TryUnpin(Tool tool)
    {
        if (!TryChangeState(tool) || !_spaceForComponents.IsFull)
        {
            return false;
        }
        
        _isLocked = false;
        return true;
    }
    
    public virtual bool TryPin(Tool tool)
    {
        if (!TryChangeState(tool) || !_spaceForComponents.IsFull)
        {
            return false;
        }
        
        _isLocked = true;
        return true;
    }

    public virtual void Look()
    {
        
    }

    public virtual void DontLook()
    {
        
    }

    protected bool TryChangeState(Tool tool)
    {
        if (tool.Type != _type)
        {
            return false;
        }
        
        if (_minToolCharacteristics.Any(i => !i.Test(tool.ToolCharacteristics[i.Name])))
        {
            return false;
        }

        return true;
    }

    protected void SetNewComponent(PcComponent component)
    {
        _collider.enabled = true;
    }

    protected void RemoveComponent(PcComponent component)
    {
        _collider.enabled = false;
    }
}
