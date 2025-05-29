using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private const float DEFAULT_CAMERA_Z = -10f;

    [CustomHeader("Settings")]
    [SerializeField] private float _movementSpeedModifier;
    [SerializeField] private float _smoothTime;

    private float _horizontalAxisInput;
    private float _verticalAxisInput;
    private Vector3 _movementInput;
    private Vector3 _targetMovementVector;
    private Vector2 _currentVelocity;
    private Vector2 _smoothedPosition;


    private void Update()
    {
        GatherInput();

        if (_movementInput != Vector3.zero)
            MoveCamera();
    }

    private void GatherInput()
    {
        _horizontalAxisInput = Input.GetAxisRaw("Horizontal");
        _verticalAxisInput = Input.GetAxisRaw("Vertical");

        _movementInput = new Vector3(_horizontalAxisInput, _verticalAxisInput, DEFAULT_CAMERA_Z);
    }

    private void MoveCamera()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, DEFAULT_CAMERA_Z);
        _targetMovementVector = (Vector2)transform.position + ((Vector2)_movementInput * _movementSpeedModifier);
        _smoothedPosition = Vector2.SmoothDamp((Vector2)transform.position, _targetMovementVector, ref _currentVelocity, _smoothTime);
        transform.position = new Vector3(_smoothedPosition.x, _smoothedPosition.y, DEFAULT_CAMERA_Z);
    }
}
