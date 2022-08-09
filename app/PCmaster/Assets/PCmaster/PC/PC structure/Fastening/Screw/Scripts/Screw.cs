using UnityEngine;

public class Screw : Fastening
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public override void Look()
    {
        if (_spaceForComponents.IsFull)
        {
            _meshRenderer.enabled = true;
        }
    }

    public override void DontLook()
    {
        _meshRenderer.enabled = false;
    }
}