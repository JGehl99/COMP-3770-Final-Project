﻿using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Resources.Code.Scripts
{
    public class PlayerManager : MonoBehaviour
    {
        public List<GameObject> tankList;
        private List<int> _selection;
        
        private int _numberOfPlayers;

        private GameObject _tankGameObject;

        public void LoadModels()
        {
             _tankGameObject = UnityEngine.Resources.Load("Prefabs/Tank") as GameObject;
        }

        public void SpawnTanks(List<int> playerSelection, GameObject[,] mapArray)
        {
            _selection = playerSelection;
            _numberOfPlayers = playerSelection.Count;
            
            tankList = new List<GameObject>();
            
            foreach (var i in _selection)
            {
                var tile = GetValidSpawn(mapArray);

                var tempTank = i switch
                {
                    0 => CreateTankGameObject("Lt", 100, 3, tile),
                    1 => CreateTankGameObject("Sgt", 100, 3, tile),
                    2 => CreateTankGameObject("Cpl", 100, 3, tile),
                    3 => CreateTankGameObject("Fm", 100, 3, tile),
                    4 => CreateTankGameObject("Psc", 100, 3, tile),
                    _ => CreateTankGameObject("Lt", 100, 3, tile)
                };
                
                tankList.Add(tempTank);
            }
        }
        /*
         * Checks to see if the given MapTile is a valid spawning location
         * by checking to see how many neighbours it has, if it is greater than
         * 0, then the players are able to spawn on it.
         */
        private GameObject GetValidSpawn(GameObject[,] arr)
        {
            while (true)
            {
                var ranX = Random.Range(0, 10);
                var ranY = Random.Range(0, 10);

                var tile = arr[ranX, ranY];

                var mapTile = tile.GetComponent<MapTile>();

                if (mapTile.neighbours.Count > 1)
                    return tile;
            }
        }

        public void MoveTank(GameObject tank, GameObject tile)
        {
            tank.GetComponent<Tank>().currentTile = tile;
            var target = tile.GetComponent<MapTile>().GetTop();
            target.y += 6.25f;   // Adjust for tank model height
            tank.GetComponent<Tank>().target = target;
        }

        private GameObject CreateTankGameObject(string tankName, int health, int moveDistance, GameObject tile)
        {
            
            var spawnLocation = tile.GetComponent<MapTile>().GetTop();
                
            spawnLocation.y += 6.25f;   // Adjust for tank model height
            
            var go = Instantiate(_tankGameObject, spawnLocation, Quaternion.identity);
            go.GetComponent<Tank>().Create(tankName, health, moveDistance, tile);
            go.name = "Tank-" + tankName;
            return go;
        }
    }
}