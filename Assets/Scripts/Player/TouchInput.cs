using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class TouchInput : MonoBehaviour
{
    [SerializeField] private float _speed = 15;
    [SerializeField] private GraphicRaycaster _raycaster;

    private bool _isCorrectTouch;
    private Vector2 _startTouchPosition;
    private Rigidbody _rigidbody;

    public bool IsTouchCorrect => _isCorrectTouch;

    public event Action<Vector2> MoveStarted;
    public event Action MoveEnded;
    public event Action<Vector2> MoveContinued;

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
                    if (UIBlocker.IsPointerOverUI(touch.position, _raycaster, "Joystick"))
                        return;
                    
                    _isCorrectTouch = true;
                    _startTouchPosition = touch.position;
                    MoveStarted?.Invoke(touch.position);
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    Move(touch.position);
                    MoveContinued?.Invoke(touch.position);
                    break;

                case TouchPhase.Canceled:
                case TouchPhase.Ended:
                    _isCorrectTouch = false;
                    _rigidbody.linearVelocity = Vector3.zero;
                    MoveEnded?.Invoke();
                    break;
            } 
        }
    }

    private void Move(Vector2 touchPosition)
    {
        if (!_isCorrectTouch) return;

        Vector2 direction = touchPosition - _startTouchPosition;
        direction.Normalize();

        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);

        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDirection);

        _rigidbody.linearVelocity = moveDirection * _speed;
    }

}
