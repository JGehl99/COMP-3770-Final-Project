using System.Collections.Generic;
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

    private List<GameObject> _playerTanks;
    private List<GameObject> _enemyTanks;
    private GameManager _gm;
    
    private int _playerCount = 0;
    private int _enemyCount = 0;
    private bool _capsPressed = false;
    private bool _tabPressed = false;
    Vector3 tankPos;

    private float _lowerBoundX;
    private float _lowerBoundZ;
    private float _upperBoundX;
    private float _upperBoundZ;

    private void Awake()
    {
        _cameraControls = new CameraControls();
        _cameraTransform = GetComponentInChildren<Camera>().transform;
    }

    public void Setup(float lowerBoundX, float lowerBoundY, float upperBoundX, float upperBoundY, List<GameObject> playerTanks, List<GameObject> enemyTanks, GameManager gm)
    {
        _lowerBoundX = lowerBoundX;
        _lowerBoundZ = lowerBoundY;
        _upperBoundX = upperBoundX;
        _upperBoundZ = upperBoundY;
        _playerTanks = playerTanks;
        _enemyTanks = enemyTanks;
        _gm = gm;

        tankPos = playerTanks[0].transform.position;

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
        transform.rotation = Quaternion.Euler(0f, input * maxRotSpeed + transform.rotation.eulerAngles.y, 0f);
    }

    private void ZoomCamera(InputAction.CallbackContext obj)
    {
        var input = -obj.ReadValue<Vector2>().y / 100f;

        if (!(Mathf.Abs(input) > 0.1f)) return;

        _zoomDis = _cameraTransform.localPosition.y + input * stepSize;


        if (_zoomDis < minHeight)
        {
            _zoomDis = minHeight;
        }
        else if (_zoomDis > maxHeight)
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
        var t = transform.position;

        t += _targetPos * (maxSpeed * Time.deltaTime);

        t = CheckBounds(t);

        transform.position = t;

        _targetPos = Vector3.zero;
    }

    private void CenterCameraToPlayer(int tankCount)
    {
        tankPos = _playerTanks[tankCount].transform.position;
        
        tankPos = CheckBounds(tankPos);
        
        transform.position = tankPos;
        
    }
    
    private void CenterCameraToEnemy(int tankCount)
    {
        Vector3 tankPos = _enemyTanks[tankCount].transform.position;
        
        tankPos = CheckBounds(tankPos);

        transform.position = tankPos;
    }

    private void UpdaterCameraPosition()
    {
        var localPos = _cameraTransform.localPosition;

        var zoom = new Vector3(localPos.x, _zoomDis, localPos.z);

        _cameraTransform.localPosition = zoom;
        _cameraTransform.LookAt(transform);
    }

    private Vector3 CheckBounds(Vector3 t)
    {
        if (t.x < _lowerBoundX)
        {
            t.x = _lowerBoundX;
        }
        else if (t.x > _upperBoundX)
        {
            t.x = _upperBoundX;
        }

        if (t.z < _lowerBoundZ)
        {
            t.z = _lowerBoundZ;
        }
        else if (t.z > _upperBoundZ)
        {
            t.z = _upperBoundZ;
        }

        return t;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _gm.UnselectTank();
            _tabPressed = true;
            tankPos = _gm.playerManager.tankList[_playerCount].transform.position;
            tankPos = CheckBounds(tankPos);
            _gm.SelectTank(_playerTanks[_playerCount]);
        }
        
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            _capsPressed = true;
            tankPos = _gm.enemyManager.tankList[_enemyCount].transform.position;
            tankPos = CheckBounds(tankPos);
            
        }
        else
        {
            GetMovement();

            UpdatePosition();
        }

        
        if (transform.position != tankPos)
        {
            transform.position = Vector3.Lerp(transform.position, tankPos, Time.deltaTime * 5f);
        }
        
        UpdaterCameraPosition();
        
        if (_tabPressed)
        {
            if (_playerCount < _gm.playerManager.tankList.Count)
            {
                _playerCount++; 
            }
            else
            {
                _playerCount = 0;
            }

            _tabPressed = false;

        }

        if (_capsPressed)
        {
            if (_enemyCount < _gm.enemyManager.tankList.Count)
            {
                _enemyCount++; 
            }
            else
            {
                _enemyCount = 0;
            }

            _capsPressed = false;

        }
        
    }
}