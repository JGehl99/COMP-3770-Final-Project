using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    
    private CameraControls _cameraControls;
    private InputAction _cameraMovement;
    private Transform _cameraTransform;

    public float maxSpeed = 5f;

    public float stepSize = 2f;

    public float minHeight = 5f;

    public float maxHeight = 50f;
    
    public float maxRotSpeed = 1f;

    private Vector3 _targetPos;

    private float _zoomDis = 40f;

    private void Awake()
    {
        _cameraControls = new CameraControls();
        _cameraTransform = GetComponentInChildren<Camera>().transform;
    }

    private void OnEnable()
    {
        _cameraTransform.LookAt(transform);
        
        _cameraMovement = _cameraControls.Camera.MoveCamera;
        _cameraControls.Camera.RotateCamera.performed += RotateCamera;
        _cameraControls.Camera.ZoomCamera.performed += ZoomCamera;
        
        _cameraControls.Camera.Enable();
    }

    private void OnDisable()
    {
        _cameraControls.Camera.RotateCamera.performed -= RotateCamera;
        _cameraControls.Camera.ZoomCamera.performed -= ZoomCamera;
        _cameraControls.Camera.Disable();
    }

    private void RotateCamera(InputAction.CallbackContext obj)
    {
        if (!Mouse.current.middleButton.isPressed)
            return;
        
        var input = obj.ReadValue<Vector2>().x;
        transform.rotation = Quaternion.Euler(0f, input*maxRotSpeed+transform.rotation.eulerAngles.y, 0f);
    }

    private void ZoomCamera(InputAction.CallbackContext obj)
    {
        var input = -obj.ReadValue<Vector2>().y / 100f;

        if (!(Mathf.Abs(input) > 0.1f)) return;
        
        _zoomDis = _cameraTransform.localPosition.y + input * stepSize;

        
        if (_zoomDis < minHeight)
        {
            _zoomDis = minHeight;
        } else if (_zoomDis > maxHeight)
        {
            _zoomDis = maxHeight;
        }
    }

    private void GetMovement()
    {
        var xVal = _cameraMovement.ReadValue<Vector2>().x * transform.right;
        var zVal = _cameraMovement.ReadValue<Vector2>().y * transform.forward;

        Vector3 input = xVal + zVal;
        
        input.y = 0;

        _targetPos += input;
    }

    private void UpdatePosition()
    {
        transform.position += _targetPos * (maxSpeed * Time.deltaTime);
        _targetPos = Vector3.zero;
    }

    private void UpdaterCameraPosition()
    {
        var localPos = _cameraTransform.localPosition;
        
        var zoom = new Vector3(localPos.x, _zoomDis, localPos.z);

        _cameraTransform.localPosition = zoom;
        _cameraTransform.LookAt(transform);
    }

    private void Update()
    {
        GetMovement();

        UpdatePosition();
        UpdaterCameraPosition();
    }
}

