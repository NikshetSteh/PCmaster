using UnityEngine;

public class PlayerSight : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    
    [SerializeField] private float _maxLookDistance;

    private SpaceForComponents _lastSpaceForComponentsInView;
    private Fastening _lastFasteningInView;
    
    private Component _lastComponent;

    private void Update()
    {
        Ray ray = new Ray(_camera.position, _camera.forward);
        
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.distance > _maxLookDistance) return;
            
            if (raycastHit.collider.gameObject.CompareTag(Tags.SpaceForComponent))
            {
                SpaceForComponents nowLookedObject = raycastHit.collider.gameObject.GetComponent<SpaceForComponents>();
                
                if (nowLookedObject != _lastSpaceForComponentsInView && _lastSpaceForComponentsInView)
                {
                    nowLookedObject.dontLookToThis.Invoke();
                    return;
                }
                
                if (nowLookedObject == _lastSpaceForComponentsInView) return;

                _lastSpaceForComponentsInView = nowLookedObject;
                
                _lastSpaceForComponentsInView.lookToThis.Invoke();
            }
            else if (raycastHit.collider.gameObject.CompareTag(Tags.Fastening))
            {
                Fastening nowLookedObject = raycastHit.collider.gameObject.GetComponent<Fastening>();

                if (nowLookedObject != _lastFasteningInView && _lastSpaceForComponentsInView)
                {
                    nowLookedObject.DontLook();
                    return;
                }
                
                if (nowLookedObject == _lastFasteningInView) return;

                _lastFasteningInView = nowLookedObject;

                _lastFasteningInView.Look();
            }
            else if (_lastSpaceForComponentsInView)
            {
                _lastSpaceForComponentsInView.dontLookToThis.Invoke();
                
                _lastSpaceForComponentsInView = null;
            }
            else if (_lastFasteningInView)
            {
                _lastFasteningInView.DontLook();

                _lastFasteningInView = null;
            }
        }
        else if(_lastSpaceForComponentsInView)
        {
            _lastSpaceForComponentsInView.dontLookToThis.Invoke();
            _lastSpaceForComponentsInView = null;
        }
    }
}
