using System;
using UnityEngine;

public class PlayerTakeController : MonoBehaviour
{
    [Header("Input controller")]
    [SerializeField] private PlayerInput _input;

    [Header("Require component")]
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _hand;

    [Header("Take option")]
    [SerializeField] private float _maxTakeDistance;

    [Space(0.5f)]
    [SerializeField] private float _maxHoldDistance;
    [SerializeField] private float _minHoldDistance;

    [Header("Taker objects tags")]
    [SerializeField] private string _pcComponentTag;

    private bool _hasObjectInHand;
    private GameObject _objectInHand;

    private void Awake()
    {
        _input.click.AddListener(OnClick);
    }

    private void OnClick()
    {
        Ray ray = new(_camera.position, _camera.forward);


        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (_hasObjectInHand)
            {
                PutObject(raycastHit);
            }
            else
            {
                TakeObject(raycastHit);
            }
        }
    }

    private void TakeObject(RaycastHit raycastHit)
    {
        if (!raycastHit.transform.gameObject.CompareTag(_pcComponentTag))
        {
            return;
        }

        if (raycastHit.distance > _maxTakeDistance)
        {
            return;
        }

        _hasObjectInHand = true;

        _objectInHand = raycastHit.transform.gameObject;

        _objectInHand.GetComponent<Rigidbody>().isKinematic = true;
        _objectInHand.GetComponent<Rigidbody>().useGravity = false;

        _objectInHand.transform.parent = _hand;

        _objectInHand.transform.localPosition = Vector3.forward * _maxHoldDistance;
    }

    private void PutObject(RaycastHit raycastHit)
    {
        //TODO: put to place for component
        
        print(raycastHit.transform.gameObject.name);

        if (raycastHit.transform.gameObject.TryGetComponent(typeof(SpaceForComponents), out var component))
        {
            print("put into pc");
            SpaceForComponents spaceForComponents = (SpaceForComponents)component;

            if (spaceForComponents.TrySetComponent(_objectInHand))
                return;
        }

        _objectInHand.GetComponent<Rigidbody>().isKinematic = false;
        _objectInHand.GetComponent<Rigidbody>().useGravity = true;

        _objectInHand.transform.parent = transform.parent;

        _hasObjectInHand = false;
        _objectInHand = null;
    }

    private void Update()
    {
        //TODO make light for space for components when player see to it 
    }
}
