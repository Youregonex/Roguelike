using UnityEngine;

namespace Y.CameraControl
{
    public class CameraZoom : MonoBehaviour
    {
        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 10f;
        [SerializeField] private float minZoom = 5f; 
        [SerializeField] private float maxZoom = 20f;
        [SerializeField] private float smoothTime = 0.2f;

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

            _targetZoom = _camera.orthographicSize;
        }

        private void Update()
        {
            HandleZoom();
        }

        private void HandleZoom()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (scrollInput != 0)
            {
                _targetZoom -= scrollInput * zoomSpeed;
                _targetZoom = Mathf.Clamp(_targetZoom, minZoom, maxZoom);
            }

            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _targetZoom, ref _zoomVelocity, smoothTime);
        }
    }
}