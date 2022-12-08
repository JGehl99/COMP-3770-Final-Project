using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

namespace Resources.Code.Scripts
{
    public class GameManager : MonoBehaviour
    {
        //**********************
        // Modifiable Values
        //**********************
        
        [Range(1, 99)]
        public int maxMapX;
        
        [Range(1, 99)]
        public int maxMapZ;

        [Range(2, 6)]
        public int maxMoveDistance;
        
        [Range(2, 4)]
        public int numberOfPlayers;
        
        [Range(2, 4)]
        public int numberOfEnemies;
        
        
        //**********************
        // GameObjects
        //**********************
        
        private GameObject _mapManagerGameObject;
        private GameObject _playerManagerGameObject;
        private GameObject _cameraGameObject;
        
        
        //**********************
        // Scripts
        //**********************
        
        private MapManager _mapManager;
        private PlayerManager _playerManager;
        private Camera _camera;
        
        
        //***************************************
        // Variables for Currently Selected Tank
        //***************************************

        private GameObject _selectedTank;
        
        
        
        private void Start()
        {
            //**********************
            // Set up GameManager
            //**********************
            
            
            
            
            //**********************
            // Set up Camera
            //**********************
            
            //TODO: Clamp Camera to size of map

            _cameraGameObject = CreateCameraGameObject();
            _camera = Camera.main;

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
            _playerManager.SpawnTanks(new List<int>{0, 1, 2}, _mapManager.MapArray);
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
                        if (tile.isHighlighted)
                        {
                            // Unhighlight currently highlighted tiles
                            var tankScript = _selectedTank.GetComponent<Tank>();
                            tankScript.currentTile.GetComponent<MapTile>().Unhighlight(tankScript.moveDistance);

                            if (!tankScript.hasMoved)
                            {
                                // Move tank then unselect tank
                                _playerManager.MoveTank(_selectedTank, go);
                                tankScript.hasMoved = true;
                                UnselectTank();
                            }
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
            
            // When the user right clicks anywhere, deselect the currently selected tank
            UnselectTank();
        }
        
        private void SelectTank(GameObject go)
        {
            _selectedTank = go;
            var tank = _selectedTank.GetComponent<Tank>();
            var currentTile = tank.currentTile.gameObject.GetComponent<MapTile>();
            
            // If tank hasn't moved, highlight movement tiles
            if(!tank.hasMoved) currentTile.Highlight(tank.moveDistance);
        }

        private void UnselectTank()
        {
            if (_selectedTank == null) return;
            
            var tank = _selectedTank.GetComponent<Tank>();
            var currentTile = tank.currentTile.gameObject.GetComponent<MapTile>();
            currentTile.Unhighlight(tank.moveDistance);
            
            _selectedTank = null;
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
        
        // private static GameObject CreateEnemyManagerGameObject()
        // {
        //     var go = new GameObject(); 
        //     go.AddComponent<EnemyManager>();
        //     go.gameObject.name = "EnemyManager";
        //     return go;
        // }
    }
}