using UnityEngine;

public class TouchInput : MonoBehaviour
{
    [SerializeField] private float _speed = 5;

    private bool _isTouch = false;
    private Vector2 _startTouchPosition;

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
                    break;
            }
        }
    }

    private void Move(Vector2 touchPosition)
    {
        if (!_isTouch) return;

        Vector2 direction = touchPosition - _startTouchPosition;

        direction.Normalize();

        transform.Translate(new Vector3(direction.x, 0, direction.y) * (_speed * Time.deltaTime));
    }
}