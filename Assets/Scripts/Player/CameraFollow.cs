using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    
    private Vector2 _leftTopCorner;
    private Vector2 _rightBottomCorner;
    
    private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position - _target.position;
    }

    private void LateUpdate()
    {
        Vector3 position = _target.position + _offset;

        position.x = Mathf.Clamp(position.x, _leftTopCorner.x, _rightBottomCorner.x);
        position.z = Mathf.Clamp(position.z, _rightBottomCorner.y, _leftTopCorner.y);

        transform.position = position;
    }

    public void SetCorners(Vector2 leftTopCorner, Vector2 rightBottomCorner)
    {
        _leftTopCorner = leftTopCorner;
        _rightBottomCorner = rightBottomCorner;
    }
}