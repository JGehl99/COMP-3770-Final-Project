using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Resources.Code.Scripts
{
    public class GameManager : MonoBehaviour
    {
        //**********************
        // Modifiable Values
        //**********************

        [Range(1, 99)] public int maxMapX;

        [Range(1, 99)] public int maxMapZ;

        [Range(2, 6)] public int maxMoveDistance;

        [Range(2, 4)] public int numberOfPlayers;

        [Range(2, 4)] public int numberOfEnemies;


        //**********************
        // GameObjects
        //**********************

        private GameObject _mapManagerGameObject;
        private GameObject _playerManagerGameObject;
        private GameObject _cameraGameObject;
        private GameObject _canvasGameObject;
        private GameObject _attackInfoGameObject;
        private GameObject _leftPanel;
        private GameObject _rightPanel;
        private GameObject _title;
        private TextMeshProUGUI _titleText;
        private GameObject _fireButton;
        private GameObject _moveButton;
        private GameObject _shot1Button;
        private GameObject _shot2Button;
        private GameObject _shot3Button;
        


        //**********************
        // Scripts
        //**********************

        private MapManager _mapManager;
        private PlayerManager _playerManager;
        private Camera _camera;


        //*********************************************
        // Variables for Currently Selected Tank & Tile
        //*********************************************

        private GameObject _selectedTank;
        private GameObject _selectedTile;


        //Variables
        private int _attackType;
        private bool _isAttacking = false;
        private bool _isMoving = false;
        private Vector3 _coordinates;


        private void Start()
        {
            //**********************
            // Set up GameManager
            //**********************


            //**********************
            // Set up Managers
            //**********************

            // Set up MapManager
            _mapManagerGameObject = CreateMapManagerGameObject();
            _mapManager = _mapManagerGameObject.GetComponent<MapManager>();

            // Set up PlayerManager
            _playerManagerGameObject = CreatePlayerManagerGameObject();
            _playerManager = _playerManagerGameObject.GetComponent<PlayerManager>();

            //TODO: Create EnemyManager
            // // Set up EnemyManager
            // _enemyManagerGameObject = CreateEnemyManagerGameObject();
            // _enemyManager = _enemyManagerGameObject.GetComponent<EnemyManager>();

            //Setup canvas
            _canvasGameObject = GameObject.Find("Canvas");

            //Get UI object for AttackInfo
            _attackInfoGameObject = _canvasGameObject.transform.GetChild(0).gameObject;
            _attackInfoGameObject.SetActive(false);

            _leftPanel = _attackInfoGameObject.transform.GetChild(0).gameObject;
            _rightPanel = _attackInfoGameObject.transform.GetChild(1).gameObject;
            _title = _attackInfoGameObject.transform.GetChild(2).gameObject;

            _shot1Button = _leftPanel.transform.GetChild(0).gameObject;
            _shot1Button.SetActive(true);

            _shot2Button = _leftPanel.transform.GetChild(1).gameObject;
            _shot2Button.SetActive(true);
            
            _shot3Button = _leftPanel.transform.GetChild(2).gameObject;
            _shot3Button.SetActive(true);

            _moveButton = _rightPanel.transform.GetChild(0).gameObject;
            _moveButton.SetActive(true);
            
            _fireButton = _rightPanel.transform.GetChild(1).gameObject;
            _fireButton.SetActive(false);

            _titleText = _title.GetComponent<TextMeshProUGUI>();



            //**********************
            // Load Models
            //**********************

            // Load Map Models
            _mapManager.LoadModels();

            // Load Tank Models
            _playerManager.LoadModels();

            //TODO: Create EnemyManager
            // // Load Enemy Tank Models
            // _enemyManager.LoadModels();


            //********************************
            // Generate Map, Players, Enemies
            //********************************

            // Generate Map
            _mapManager.GenerateMap(maxMapX, maxMapZ, maxMoveDistance);

            //TODO: Get selection list from Setup Scene
            // Spawn Characters on map
            _playerManager.SpawnTanks(new List<int> { 0, 1, 2 }, _mapManager.MapArray);


            //**********************
            // Set up Camera
            //**********************

            //TODO: Clamp Camera to size of map

            _cameraGameObject = CreateCameraGameObject();

            var lowerLeftTile = _mapManager.MapArray[0, 0].GetComponent<MapTile>().GetTop();
            var topRightTile = _mapManager.MapArray[maxMapX - 1, maxMapZ - 1].GetComponent<MapTile>().GetTop();


            _cameraGameObject.GetComponentInChildren<CameraController>().Setup(
                lowerLeftTile.x,
                lowerLeftTile.z,
                topRightTile.x,
                topRightTile.z,
                _playerManager.tankList
            );

            _camera = Camera.main;
        }

        private void Update()
        {
        }

        //***************************
        // If the player left clicks
        //***************************
        private void OnLeftClick()
        {
            Debug.Log("Left Clicked!");
            if (EventSystem.current.IsPointerOverGameObject()) return;
            //***************************************
            // If the player left clicks on anything
            //***************************************
            if (Physics.Raycast(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit))
            {
                // Get clicked GameObjects
                var go = hit.transform.gameObject;

                // If tile is clicked
                if (go.CompareTag("Tile"))
                {
                    var tile = go.GetComponent<MapTile>();

                    Debug.Log(tile.id);

                    // If a tank is already selected
                    if (_selectedTank != null)
                    {
                        // If a highlighted tile is clicked
                        if (tile.isHighlighted && _isMoving)
                        {
                            // Unhighlight currently highlighted tiles
                            var tankScript = _selectedTank.GetComponent<Tank>();
                            tankScript.currentTile.GetComponent<MapTile>().Unhighlight(tankScript.moveDistance);

                            if (!tankScript.hasMoved)
                            {
                                // Move tank then unselect tank
                                _playerManager.MoveTank(_selectedTank, go);
                                tankScript.hasMoved = true;
                                _isMoving = false;
                                UnselectTank();
                            }
                        }
                        else if (_selectedTank != null && _isAttacking)
                        {
                            SelectTile(go);
                        }
                    }
                }

                // If friendly tank is clicked
                else if (go.CompareTag("Tank"))
                {
                    // If another tank was already selected, unselect the current selected tank
                    if (_selectedTank != go && _selectedTank != null) UnselectTank();

                    // Select new tank
                    SelectTank(go);
                }
            }
        }

        //****************************
        // If the player right clicks
        //****************************
        private void OnRightClick()
        {
            Debug.Log("Right Clicked!");

            // When the user right clicks anywhere, deselect the currently selected tank and tile
            UnselectTile();
            UnselectTank();
        }

        private void SelectTank(GameObject go)
        {
            UnselectTile();
            _selectedTank = go;
            var tank = _selectedTank.GetComponent<Tank>();
            tank.currentTile.GetComponent<MapTile>().HighlightSelect();
            if (!tank.hasMoved)
            {
                _moveButton.SetActive(true);
            }
            else
            {
                _moveButton.SetActive(false);
            }


            if (!tank.hasAttacked)
            {
                _shot1Button.SetActive(true);
                _shot2Button.SetActive(true);
                _fireButton.SetActive(false);
            }
            else
            {
                _fireButton.SetActive(false);
                _shot1Button.SetActive(false);
                _shot2Button.SetActive(false);
            }

            _attackInfoGameObject.SetActive(true);
        }

        private void UnselectTank()
        {
            if (_selectedTank == null) return;

            var tank = _selectedTank.GetComponent<Tank>();
            var currentTile = tank.currentTile.gameObject.GetComponent<MapTile>();
            currentTile.Unhighlight(tank.moveDistance);

            _selectedTank = null;
            tank.currentTile.GetComponent<MapTile>().UnhighlightSelect();
            _attackInfoGameObject.SetActive(false);
        }

        private void SelectTile(GameObject go)
        {
            if (_selectedTile != null) UnselectTile();
            if (_selectedTank.GetComponent<Tank>().currentTile == _selectedTile) return;


            _fireButton.SetActive(true);

            _selectedTile = go;
            _selectedTile.GetComponent<MapTile>().HighlightAttack();
            _attackInfoGameObject.SetActive(true);

            _selectedTank.GetComponent<Tank>().transform.LookAt(_selectedTile.GetComponent<MapTile>().GetTop());
        }

        private void UnselectTile()
        {
            if (_selectedTile == null) return;

            _selectedTile.GetComponent<MapTile>().UnhighlightAttack();

            _selectedTile = null;
            _fireButton.SetActive(false);
        }

        private static GameObject CreateMapManagerGameObject()
        {
            var go = new GameObject();
            go.AddComponent<MapManager>();
            go.gameObject.name = "MapManager";
            return go;
        }

        private static GameObject CreatePlayerManagerGameObject()
        {
            var go = new GameObject();
            go.AddComponent<PlayerManager>();
            go.gameObject.name = "PlayerManager";
            return go;
        }

        private static GameObject CreateCameraGameObject()
        {
            var go = UnityEngine.Resources.Load<GameObject>("Prefabs/MapCamera");
            var go1 = Instantiate(go);
            go1.gameObject.name = "Camera";
            return go1;
        }

        public void Recoilless()
        {
            _isMoving = false;
            _isAttacking = true;
            _attackType = 0;
        }

        public void Move()
        {
            var tank = _selectedTank.GetComponent<Tank>();
            var currentTile = tank.currentTile.gameObject.GetComponent<MapTile>();

            // If tank hasn't moved, highlight movement tiles
            if (!tank.hasMoved)
            {
                currentTile.Highlight(tank.moveDistance);
            }

            _isAttacking = false;
            _isMoving = true;

            _moveButton.SetActive(false);
        }

        public void Fire()
        {
            _selectedTank.GetComponent<Tank>().hasAttacked = true;

            switch (_attackType)
            {
                case 0:
                    _selectedTank.GetComponent<Tank>().Recoilless(_selectedTile.GetComponent<MapTile>().GetTop());
                    break;
                case 1:
                    _selectedTank.GetComponent<Tank>().Special(_selectedTile.GetComponent<MapTile>().GetTop());
                    break;
            }

            _shot1Button.SetActive(false);
            _shot2Button.SetActive(false);

            UnselectTile();
            UnselectTank();
        }


        // private static GameObject CreateEnemyManagerGameObject()
        // {
        //     var go = new GameObject(); 
        //     go.AddComponent<EnemyManager>();
        //     go.gameObject.name = "EnemyManager";
        //     return go;
        // }
    }
}