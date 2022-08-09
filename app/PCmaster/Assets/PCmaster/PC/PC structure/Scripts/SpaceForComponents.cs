using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SpaceForComponents : MonoBehaviour
{
    [Serializable]
    public class OptionsRequirement
    {
        [SerializeField] private PcComponent.PcComponentOptionsName _name;
        [SerializeField] private bool _isInt;
        [SerializeField] private string[] _suitableValues;
        [SerializeField] private double _min;
        [SerializeField] private double _max;

        public PcComponent.PcComponentOptionsName Name => _name;

        public bool Test(PcComponent.PcComponentOption option)
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

    [Header("Component data")]
    [SerializeField] private OptionsRequirement[] _minOptionsRequirements; // option for component can be state to this space(NOT work!)

    [SerializeField] private PcComponent.TypeOfComponent _typeOfComponent;
    
    [SerializeField] private PcComponent _autoFull;

    [Header("Requirement components")]
    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _rigidbodyOfPc;
    [SerializeField] private MeshRenderer _meshRenderer;
    
    [Space]
    [SerializeField] private Fastening[] _fastenings;
    
    public readonly UnityEvent lookToThis = new UnityEvent();
    public readonly UnityEvent dontLookToThis = new UnityEvent();
    public bool IsFull { private set; get; }
    
    public readonly UnityEvent<PcComponent> setNewComponent = new UnityEvent<PcComponent>();
    public readonly UnityEvent<PcComponent> removeComponent = new UnityEvent<PcComponent>();

    private GameObject _nowComponent;
    private FixedJoint _nowFixedJoint;

    public SpaceForComponents()
    {
        lookToThis.AddListener(Looked);
        dontLookToThis.AddListener(DontLooked);
    }

    private void Start()
    {
        if (_autoFull)
        {
            if (!TrySetComponent(_autoFull.gameObject))
            {
                Debug.LogError("PcComponent is cannot be state(auto full)", this);
            }
        }
    }

    public bool TryRemoveNowComponent()
    {
        if (CheckFastening())
        {
            return false;
        }
        
        _nowComponent.gameObject.transform.parent = transform.root;

        _collider.enabled = true;
        
        IsFull = false;
        
        removeComponent.Invoke(_nowComponent.GetComponent<PcComponent>());
        
        DisconnectedComponent();

        return true;
    }

    public bool TrySetComponent(GameObject component)
    {
        PcComponent pcComponent = component.GetComponent<PcComponent>();
        
        if (IsFull)
            return false;

        if (CheckFastening())
        {
            return false;
        }

        if (pcComponent.Type != _typeOfComponent)
        {
            return false;
        }

        if (_minOptionsRequirements.Any(i => !i.Test(pcComponent.GetOptions()[i.Name])))
        {
            return false;
        }

        _nowComponent = component;
        IsFull = true;

        _nowComponent.transform.position = transform.position;
        _nowComponent.transform.localEulerAngles = new Vector3(0, 0, 0);

        _nowComponent.GetComponent<PcComponent>().PutToPc(this);

        _meshRenderer.enabled = false;
        _collider.enabled = false;
        
        setNewComponent.Invoke(_nowComponent.GetComponent<PcComponent>());
        
        ConnectComponent(component.gameObject.GetComponent<Rigidbody>());
        
        return true;
    }

    private bool CheckFastening()
    {
        foreach (Fastening i in _fastenings)
        {
            if (i.IsLocked)
            {
                return true;
            }
        }

        return false;
    }

    private void DontLooked()
    {
        _meshRenderer.enabled = false;
    }

    private void Looked()
    {
        if (IsFull)
        {
            return;
        }
        _meshRenderer.enabled = true;
    }

    private void ConnectComponent(Rigidbody component)
    {
        _nowFixedJoint = component.gameObject.AddComponent<FixedJoint>();

        _nowFixedJoint.connectedBody = _rigidbodyOfPc;
    }

    private void DisconnectedComponent()
    {
        Destroy(_nowFixedJoint);
    }
}
