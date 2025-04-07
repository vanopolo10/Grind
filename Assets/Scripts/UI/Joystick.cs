using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    [SerializeField] private Image _joystickMiddle;
    [SerializeField] private TouchInput _touchInput;

    private void OnEnable()
    {
        _touchInput.MoveStarted += MoveJoystick;
        _touchInput.MoveContinued += MoveJoystickMiddle;
        _touchInput.MoveEnded += RestoreJoystickMiddlePosition;
    }
    
    private void OnDisable()
    {
        _touchInput.MoveStarted -= MoveJoystick;
        _touchInput.MoveContinued -= MoveJoystickMiddle;
        _touchInput.MoveEnded -= RestoreJoystickMiddlePosition;
    }

    private void MoveJoystick(Vector2 position)
    {
        transform.position = position;
    }

    private void MoveJoystickMiddle(Vector2 position)
    {
        if (!_touchInput.IsTouchCorrect)
            return;

        Vector2 center = transform.position;

        Vector2 direction = position - center;

        float maxRadius = ((RectTransform)transform).sizeDelta.x * 2;

        direction = Vector2.ClampMagnitude(direction, maxRadius);

        _joystickMiddle.transform.position = center + direction;
    }


    private void RestoreJoystickMiddlePosition()
    {
        _joystickMiddle.transform.localPosition = Vector2.zero;
    }
}
