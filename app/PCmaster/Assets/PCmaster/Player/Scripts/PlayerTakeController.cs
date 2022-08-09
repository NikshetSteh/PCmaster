using System;
using UnityEngine;

public class PlayerTakeController : MonoBehaviour
{
    [Header("Input controller")]
    [SerializeField] private PlayerInput _input;

    [Header("Require component")]
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _handForPcComponent;
    [SerializeField] private Transform _handForTool;
    [SerializeField] private Tool _hand;

    [Header("Take option")]
    [SerializeField] private float _maxTakeDistance;

    [Space(0.5f)]
    [SerializeField] private float _maxHoldDistance;
    [SerializeField] private float _minHoldDistance;

    private GameObject _objectInHand;
    private float _objectInHandDistance;
    private Tool _nowTool;

    private void Awake()
    {
        _input.click.AddListener(OnClick);
        _input.shift.AddListener(Shift);
    }
    private void OnClick()
    {
        Ray ray = new(_camera.position, _camera.forward);


        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (_objectInHand)
            {
                if (_objectInHand.CompareTag(Tags.PcComponent))
                {
                    TryPutObjectIntoPc(raycastHit);
                    return;
                }

                if (raycastHit.collider.gameObject.CompareTag(Tags.Fastening) && _objectInHand.CompareTag(Tags.Tool))
                {
                    UseTool(raycastHit);
                    return;
                }

                PutObject();
            }
            else
            {
                if (raycastHit.collider.gameObject.CompareTag(Tags.Fastening))
                {
                    UseTool(raycastHit);
                    return;
                }

                TakeObject(raycastHit);
            }
        }
        else if(_objectInHand)
        {
            PutObject();
        }
    }

    private void UseTool(RaycastHit raycastHit)
    {
        Fastening fastening = raycastHit.collider.gameObject.GetComponent<Fastening>();

        Tool tool = !_objectInHand ? _hand : _objectInHand.GetComponent<Tool>();
        
        if (fastening.IsLocked)
        {
            if (fastening.TryUnpin(tool))
            {
                return;
            }
        }
        else
        {
            if (fastening.TryPin(tool))
            {
                return;
            }
        }
        
        PutObject();
    }
    
    private void TakeObject(RaycastHit raycastHit)
    {
        if (raycastHit.distance > _maxTakeDistance)
        {
            return;
        }
        
        if (raycastHit.collider.gameObject.CompareTag(Tags.PcComponent))
        {
            TakeComponent(raycastHit);
            return;
        }

        if (raycastHit.collider.gameObject.CompareTag(Tags.Tool))
        {
            TakeTool(raycastHit);
        }
    }

    private void TakeTool(RaycastHit raycastHit)
    {
        _objectInHand = raycastHit.collider.gameObject;

        _nowTool = _objectInHand.GetComponent<Tool>();
        
        _nowTool.Take();

        _objectInHand.transform.parent = _handForTool;
        _objectInHand.transform.localPosition = new Vector3(0, 0, 0);
        _objectInHand.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    private void TakeComponent(RaycastHit raycastHit)
    {
        _objectInHand = raycastHit.collider.gameObject;

        if (!_objectInHand.GetComponent<PcComponent>().TryTake())
        {
            _objectInHand = null;
            return;
        }
        
        _objectInHand.transform.parent = _handForPcComponent;

        _objectInHand.transform.localPosition = Vector3.forward * _maxHoldDistance;
    }

    private void TryPutObjectIntoPc(RaycastHit raycastHit)
    {
            _objectInHand.transform.parent = transform.parent;
            if (raycastHit.collider.gameObject.TryGetComponent(typeof(SpaceForComponents), out var component))
            {
                SpaceForComponents spaceForComponents = (SpaceForComponents)component;

                if (spaceForComponents.TrySetComponent(_objectInHand))
                {
                    _objectInHand = null;
                    return;
                }
            }

            PutObject();
    }

    private void PutObject()
    {
        if (_objectInHand.CompareTag(Tags.PcComponent))
            _objectInHand.GetComponent<PcComponent>().Put();

        if (_objectInHand.CompareTag(Tags.Tool))
            _objectInHand.GetComponent<Tool>().Put();

        _objectInHand.transform.parent = transform.parent;
        _objectInHand = null;
    }

    private void Shift(float delta)
    {
        if (!_objectInHand) return;
        
        _objectInHandDistance = Math.Clamp(_objectInHandDistance - delta, _minHoldDistance, _maxHoldDistance);

        _objectInHand.transform.localPosition = new Vector3(0, 0, 1) * _objectInHandDistance;
    }
}
