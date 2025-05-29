using UnityEngine;

public class SmoothCameraZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 10f; // Speed of zooming
    [SerializeField] private float minZoom = 5f; // Minimum orthographic size
    [SerializeField] private float maxZoom = 20f; // Maximum orthographic size
    [SerializeField] private float smoothTime = 0.2f; // Smoothing time for zoom

    private Camera _camera;
    private float _targetZoom;
    private float _zoomVelocity;

    private void Awake()
    {
        _camera = Camera.main;
        if (_camera == null)
        {
            Debug.LogError("Main Camera not found!");
            enabled = false;
        }
    }

    private void Start()
    {
        _targetZoom = _camera.orthographicSize;
    }

    private void Update()
    {
        HandleZoom();
    }

    private void HandleZoom()
    {
        // Get scroll input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Adjust target zoom based on scroll input
        if (scrollInput != 0)
        {
            _targetZoom -= scrollInput * zoomSpeed;
            _targetZoom = Mathf.Clamp(_targetZoom, minZoom, maxZoom); // Clamp zoom
        }

        // Smoothly interpolate the camera's zoom to the target zoom
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _targetZoom, ref _zoomVelocity, smoothTime);
    }
}