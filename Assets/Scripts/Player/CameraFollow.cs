using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    
    [SerializeField] private Vector2 _leftTopCorner;
    [SerializeField] private Vector2 _rightBottomCorner;
    
    private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position - _target.position;
    }

    private void LateUpdate()
    {
        if (!_target) return;
        
        Vector3 position = _target.position + _offset;

        position.x = Mathf.Clamp(position.x, _leftTopCorner.x, _rightBottomCorner.x);
        position.z = Mathf.Clamp(position.z, _rightBottomCorner.y, _leftTopCorner.y);

        transform.position = position;
    }

    public void SetTopEdge(float edge)
    {
        _leftTopCorner.y = edge;
    }
}