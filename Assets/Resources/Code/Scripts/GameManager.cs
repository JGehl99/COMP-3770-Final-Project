using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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




        //**********************
        // GameObjects
        //**********************

        private GameObject _mapManagerGameObject;
        private GameObject _playerManagerGameObject;
        private GameObject _enemyManagerGameObject;

        private GameObject _cameraGameObject;
        private GameObject _canvasGameObject;
        private GameObject _tankActionsPanel;
        private GameObject _leftPanel;
        private GameObject _rightPanel;
        private GameObject _title;
        private TextMeshProUGUI _titleText;
        private GameObject _fireButton;
        private GameObject _moveButton;
        private GameObject _shot1Button;
        private GameObject _shot2Button;

        //Tank list for all in-game tanks and checker for EnemyMove
        private List<GameObject> _tanks;
        private bool _tileContainsTank = false;
        

        //**********************
        // Scripts
        //**********************

        private MapManager _mapManager;
        public PlayerManager playerManager;
        public EnemyManager enemyManager;

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
        
        private int _numberOfEnemies;
        private List<int> _tankSelection;
        
        //Scores
        private int _playerScore = 0;
        private int _enemyScore = 0;
        


        private void Start()
        {
            //**********************
            // Set up GameManager
            //**********************

            _tankSelection = DontDestroyOnLoadScript.instance.selectedTanks.ToList();
            _numberOfEnemies = DontDestroyOnLoadScript.instance.enemyAmountOfTanks;
            

            //**********************
            // Set up Managers
            //**********************

            // Set up MapManager
            _mapManagerGameObject = CreateMapManagerGameObject();
            _mapManager = _mapManagerGameObject.GetComponent<MapManager>();

            // Set up PlayerManager
            _playerManagerGameObject = CreateCharacterManagerGameObject('p');
            playerManager = _playerManagerGameObject.GetComponent<PlayerManager>();

            // Set up EnemyManager
            _enemyManagerGameObject = CreateCharacterManagerGameObject('e');
            enemyManager = _enemyManagerGameObject.GetComponent<EnemyManager>();

            //Setup canvas
            _canvasGameObject = GameObject.Find("Canvas");

            //Get UI object for AttackInfo
            _tankActionsPanel = _canvasGameObject.transform.GetChild(0).gameObject;
            _tankActionsPanel.SetActive(false);
            
            _leftPanel = _tankActionsPanel.transform.GetChild(0).gameObject;
            _rightPanel = _tankActionsPanel.transform.GetChild(1).gameObject;
            
            _shot1Button = _leftPanel.transform.GetChild(0).gameObject;
            _shot1Button.SetActive(true);

            _shot2Button = _leftPanel.transform.GetChild(1).gameObject;
            _shot2Button.SetActive(true);

            _moveButton = _rightPanel.transform.GetChild(0).gameObject;
            _moveButton.SetActive(true);

            _fireButton = _rightPanel.transform.GetChild(1).gameObject;
            _fireButton.SetActive(false);

            
            //**********************
            // Load Models
            //**********************

            // Load Map Models
            _mapManager.LoadModels();

            // Load Tank Models
            playerManager.LoadModels();

            //TODO: Create EnemyManager
            // Load Enemy Tank Models
            enemyManager.LoadModels();


            //********************************
            // Generate Map, Players, Enemies
            //********************************

            // Generate Map
            _mapManager.GenerateMap(maxMapX, maxMapZ, maxMoveDistance);

            //TODO: Get selection list from Setup Scene
            // Spawn Characters on map
            playerManager.SpawnTanks(new List<int> { 0, 1, 2 }, _mapManager.MapArray);
            enemyManager.SpawnTanks(_numberOfEnemies, _mapManager.MapArray, maxMapX);
            _tanks = new List<GameObject>();
            _tanks.AddRange(playerManager.tankList);
            _tanks.AddRange(enemyManager.tankList);

            foreach (var go in _mapManager.MapArray)
            {
                go.GetComponent<MapTile>().tanks = _tanks;
            }
            
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
                playerManager.tankList,
                enemyManager.tankList,
                this
            );

            _camera = Camera.main;
        }

        public void Update()
        {
            List<GameObject> copyPlayerTanks = playerManager.tankList;
            List<GameObject> copyEmemyTanks = enemyManager.tankList;

            int nPlayerTanks = copyPlayerTanks.Count;
            int nEnemyTanks = copyEmemyTanks.Count;
            
            for(int i = 0; i < nPlayerTanks; i++)
            {
                var go = copyPlayerTanks[i];
                if (go.GetComponent<Tank>().hasDied)
                {
                    go.SetActive(false);
                    playerManager.tankList.RemoveAt(i);
                    _enemyScore += 1;
                    Debug.Log(_enemyScore);
                    break;
                }
            }
            
            
            
            for(int i = 0; i < nEnemyTanks; i++)
            {
                var go = copyEmemyTanks[i];
                if (go.GetComponent<Tank>().hasDied)
                {
                    go.SetActive(false);
                    enemyManager.tankList.RemoveAt(i);
                    _playerScore += 1;
                    Debug.Log(_playerScore);
                    break;
                }
            }
            

            if (_playerScore == 3)
            {
                print("You Win");
            }
            else if (_enemyScore == 3)
            {
                print("You Loss");
            }


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
                                playerManager.MoveTank(_selectedTank, go);
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

        public void SelectTank(GameObject go)
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

            _tankActionsPanel.SetActive(true);
        }

        public void UnselectTank()
        {
            if (_selectedTank == null) return;

            var tank = _selectedTank.GetComponent<Tank>();
            var currentTile = tank.currentTile.gameObject.GetComponent<MapTile>();
            currentTile.Unhighlight(tank.moveDistance);

            _selectedTank = null;
            tank.currentTile.GetComponent<MapTile>().UnhighlightSelect();
            _tankActionsPanel.SetActive(false);
        }

        private void SelectTile(GameObject go)
        {
            if (_selectedTile != null) UnselectTile();
            
            _selectedTile = go;
            
            if (_selectedTank.GetComponent<Tank>().currentTile == _selectedTile) return;
            
            _fireButton.SetActive(true);

            if (_attackType == 0)
            {
                _selectedTile.GetComponent<MapTile>().HighlightAttack();
            }else if (_attackType == 1)
            {
                foreach (var go1 in _selectedTile.GetComponent<MapTile>().movementLists[1])
                {
                    var mapTile = go1.GetComponent<MapTile>();
                    
                    mapTile.HighlightAttack();
                    
                }
            }
            
            _tankActionsPanel.SetActive(true);

            _selectedTank.GetComponent<Tank>().transform.LookAt(_selectedTile.GetComponent<MapTile>().GetTop());
        }

        private void UnselectTile()
        {
            if (_selectedTile == null) return;

            
            if (_attackType == 0)
            {
                _selectedTile.GetComponent<MapTile>().UnhighlightAttack();
            }
            else if (_attackType == 1)
            {
                foreach (var go in _selectedTile.GetComponent<MapTile>().movementLists[1])
                {
                    var mapTile = go.GetComponent<MapTile>();
                    
                    mapTile.UnhighlightAttack();
                    
                }
            }
            
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

        private static GameObject CreateCharacterManagerGameObject(char manType)
        {
            var go = new GameObject();
            switch (manType)
            {
                case 'p':
                    go.AddComponent<PlayerManager>();
                    go.gameObject.name = "PlayerManager";
                    break;
                default:
                    go.AddComponent<EnemyManager>();
                    go.gameObject.name = "EnemyManager";
                    break;
            }

            return go;
        }

        private static GameObject CreateCameraGameObject()
        {
            var go = UnityEngine.Resources.Load<GameObject>("Prefabs/MapCamera");
            var go1 = Instantiate(go);
            go1.gameObject.name = "Camera";
            return go1;
        }
        
        /*
         * Recoilless type shot for every tank, called on button press
         */
        public void Recoilless()
        {
            if(_selectedTile != null) UnselectTile();
            _isMoving = false;
            _isAttacking = true;
            _attackType = 0;
            
            
        }
        /*
         * Shrapnel type shot for every tank, called on button press
         */
        public void Shrapnel()
        {
            if(_selectedTile != null) UnselectTile();
            _isMoving = false;
            _isAttacking = true;
            _attackType = 1;
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
        
        /*
         * Fire is called on button press from the UI, after an attack and tile(s) to be attacked has been selected
         */
        public void Fire()
        {
            _selectedTank.GetComponent<Tank>().hasAttacked = true;

            switch (_attackType)
            {
                case 0:
                    _selectedTank.GetComponent<Tank>().Recoilless(_selectedTile, false);
                    break;
                case 1:
                    _selectedTank.GetComponent<Tank>().Shrapnel(_selectedTile);
                    break;
            }

            _shot1Button.SetActive(false);
            _shot2Button.SetActive(false);

            UnselectTile();
            UnselectTank();
        }

        //TODO Test the enemy turn functionality
        public void EnemyTurn()
        {
            
            Debug.Log("Enemies Turn");

            foreach (var enemyTankGameObject in enemyManager.tankList)
            {
                //Target a random player
                var rand = Random.Range(0, playerManager.tankList.Count);
                var targetedPlayerTank = playerManager.tankList[rand].GetComponent<Tank>();

                //GameObjects
                var targetedPlayerTileGameObject = targetedPlayerTank.currentTile;

                //Map Tiles
                var enemyTankTile = enemyTankGameObject.GetComponent<Tank>().currentTile.GetComponent<MapTile>();
                var targetedPlayerTile = targetedPlayerTileGameObject.GetComponent<MapTile>();

                //Distance variables
                var distEnemyToPlayer = CalculateDistance(enemyTankTile.GetTop(), targetedPlayerTile.GetTop());
                
                //Randomly decide if the enemy is going to move or attack, and execute the corresponding function
                switch (Random.Range(0, 2))
                {
                    case 0:
                        EnemyMove(enemyTankGameObject, targetedPlayerTileGameObject);
                        break;
                    default:
                        EnemyAttack(enemyTankGameObject, targetedPlayerTileGameObject, distEnemyToPlayer);
                        break;
                }
            }
            
            ResetTanks();
            
        }

        /*
         * Resets the tanks functionality after the enemy's turn is complete.
         */
        private void ResetTanks()
        {
            foreach (var go in playerManager.tankList)
            {
                var tank = go.GetComponent<Tank>();

                tank.hasMoved = false;
                tank.hasAttacked = false;

            }
        }

        /* EnemyAttack
         * Param1: GameObject - The tank that is firing
         * Param2: MapTile - The tile that the tank will be firing on
         * Param3: float - The distance to the target tile*/
        public void EnemyAttack(GameObject tank, GameObject targetTile, float distance)
        {
            if (!tank.GetComponent<Tank>().hasDied)
            {
                tank.GetComponent<Tank>().hasAttacked = true;
                var attackType = Random.Range(0, 2);
                switch (attackType)
                {
                    case 0:
                        tank.GetComponent<Tank>().Recoilless(targetTile, true);
                        print("Recoiless Attack");
                        break;
                    case 1:
                        tank.GetComponent<Tank>().Shrapnel(targetTile);
                        print("Shrapnel Attack");
                        break;
                } 
            }

            
        }

        /* EnemyMove
         * On the enemies turn, this function will run.
         * It will go through the list of enemy tanks, select a player at random
         * and then move towards them. */
        public void EnemyMove(GameObject enemyTank, GameObject targetedPlayerTileGameObject)
        {
            if (!enemyTank.GetComponent<Tank>().hasDied)
            {
                Debug.Log("Moving!!!");

                var enemyTileMovementList =
                    enemyTank.GetComponent<Tank>().currentTile.GetComponent<MapTile>().enemyMovementLists[2];
            
                var playerTilePos = targetedPlayerTileGameObject.GetComponent<MapTile>().GetTop();
                playerTilePos.y = 0;

                var closestTile = enemyTank.GetComponent<Tank>().currentTile;
            
                foreach (var tile in enemyTileMovementList)
                {
                
                    foreach (var go1 in _tanks)
                    {
                        if (tile == go1.GetComponent<Tank>().currentTile)
                        {
                            _tileContainsTank = true;
                        }
                    }

                    if (!_tileContainsTank)
                    {
                        var candidateTilePos = tile.GetComponent<MapTile>().GetTop();
                        candidateTilePos.y = 0;

                        var closestTilePos = closestTile.GetComponent<MapTile>().GetTop();
                        closestTilePos.y = 0;

                        if (Vector3.Distance(playerTilePos, closestTilePos) > Vector3.Distance(playerTilePos, candidateTilePos))
                        {
                            closestTile = tile;
                        }
                    }
                
                    _tileContainsTank = false;
                
                }

                enemyManager.MoveTank(enemyTank, closestTile);
            }
            
        }

        /* Returns the distance between 2 gi ven points in Vector3 space
         * Param1: Unity::Vector3
         * Param2: Unity::Vector3
         */
        private float CalculateDistance(Vector3 pos1, Vector3 pos2)
        {
            pos1.y = 0;
            pos2.y = 0;
            return Vector3.Distance(pos1, pos2);
        }
        
    }
}