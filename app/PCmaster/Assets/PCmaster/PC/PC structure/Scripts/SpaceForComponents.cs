using System;
using UnityEngine;
using UnityEngine.Events;

public class SpaceForComponents : MonoBehaviour
{
    [Serializable]
    public class OptionsRequirement
    {
        [SerializeField] private string _name;
        [SerializeField] private string _min;
        [SerializeField] private string _max;
    }

    [SerializeField] private OptionsRequirement[] _optionsRequirements;

    [SerializeField] private bool _autoFull;

    [SerializeField] private MeshRenderer _meshRenderer;

    [SerializeField] private PcComponent.TypeOfComponent _typeOfComponent;
    public PcComponent.TypeOfComponent TypeOfComponent => _typeOfComponent;

    public readonly UnityEvent lookToThis = new UnityEvent();
    public readonly UnityEvent dontLookToThis = new UnityEvent();
    public bool IsFull { private set; get; }

    private GameObject _nowComponent;

    public SpaceForComponents()
    {
        lookToThis.AddListener(Looked);
        dontLookToThis.AddListener(DontLooked);
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

    public void RemoveNowComponent()
    {
        _nowComponent.gameObject.transform.parent = transform.root;

        IsFull = false;
    }

    public bool TrySetComponent(GameObject component)
    {
        if (IsFull)
            return false;
        
        _nowComponent = component;
        IsFull = true;

        _nowComponent.GetComponent<Rigidbody>().isKinematic = true;
        _nowComponent.GetComponent<Rigidbody>().useGravity = false;

        _nowComponent.transform.parent = transform;

        _nowComponent.transform.localPosition = new Vector3(0, 0, 0);
        _nowComponent.transform.localEulerAngles = new Vector3(0, 0, 0);

        return true;
    }
}
