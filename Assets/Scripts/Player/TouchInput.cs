using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TouchInput : MonoBehaviour
{
    [SerializeField] private float _speed = 15;

    private bool _isTouch;
    private Vector2 _startTouchPosition;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _isTouch = true;
                    _startTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    Move(touch.position);
                    break;

                case TouchPhase.Canceled:
                case TouchPhase.Ended:
                    _isTouch = false;
                    _rigidbody.linearVelocity = Vector3.zero;
                    break;
            }
        }
    }

    private void Move(Vector2 touchPosition)
    {
        if (!_isTouch) return;
        
        Vector2 direction = touchPosition - _startTouchPosition;

        direction.Normalize();

        _rigidbody.linearVelocity = new Vector3(direction.x, 0, direction.y) * _speed;
    }
}