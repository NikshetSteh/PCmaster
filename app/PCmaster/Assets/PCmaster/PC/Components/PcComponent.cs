using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PcComponent : MonoBehaviour
{
    public enum TypeOfComponent { MotherBoard, SideCover };

    [SerializeField] private Dictionary<string, string> _options;
    [SerializeField] Collider _collider;

    private Rigidbody _rigidbody;

    private bool _isTaken;
    private SpaceForComponents _nowSpaceForComponents;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public Dictionary<string, string> GetOptions()
    {
        return _options;
    }
    
    public bool TryTake()
    {
        if (_nowSpaceForComponents != null)
        {
            _nowSpaceForComponents.RemoveNowComponent();
            
            Take();
        }

        return true;
    }

    public void Put()
    {
        _collider.enabled = true;

        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
    }

    public void PutToPc(SpaceForComponents spaceForComponents)
    {
        _collider.enabled = true;

        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
    }

    private void Take()
    {
        _collider.enabled = false;
        
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
    }
}
