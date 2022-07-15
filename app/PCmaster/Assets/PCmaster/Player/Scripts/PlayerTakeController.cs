using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float _maxHoldDIstance;
    [SerializeField] private float _minHoldDIstance;

    [Header("Taker objects tags")]
    [SerializeField] private string _PCcomponentTag;

    private bool _hasObjectInHand = false;
    private GameObject _objectInHand = null;

    private void Awake()
    {
        _input.click.AddListener(OnClick);
    }

    private void Update()
    {
        if (_hasObjectInHand)
        {

        }
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
        if (!raycastHit.transform.gameObject.CompareTag(_PCcomponentTag))
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

        _objectInHand.transform.localPosition = Vector3.forward * _maxHoldDIstance;
    }

    private void PutObject(RaycastHit raycastHit)
    {
        //TODO: put to place for component

        _objectInHand.GetComponent<Rigidbody>().isKinematic = false;
        _objectInHand.GetComponent<Rigidbody>().useGravity = true;

        _objectInHand.transform.parent = transform.parent;

        _hasObjectInHand = false;
        _objectInHand = null;
    }
}
