using System.Collections.Generic;
using UnityEngine;

namespace Resources.Code.Scripts
{
    public class EnemyManager : MonoBehaviour
    {
        // private int _numOfEnemies = 3;
        private GameObject _tankGameObject;
        public List<GameObject> tankList;

        public void LoadModels()
        {
            _tankGameObject = UnityEngine.Resources.Load("Prefabs/Tank") as GameObject;
        }

        public void SpawnTanks(int numOfEnemies, GameObject[,] mapArray, int max)
        {
            
            tankList = new List<GameObject>();

            max = max - 1;
            var min = max - 10;
            //Spawning the enemies on the field at the opposite corner than the player
            for (var i = 0; i < numOfEnemies; i++)
            {
                var tile = GetValidSpawn(mapArray, min, max);
                var tank = CreateTankGameObject($"EnemyTank{i}", 100, 3, tile);
                tankList.Add(tank);
            }
        }

        /*
        * Checks to see if the given MapTile is a valid spawning location
        * by checking to see how many neighbours it has, if it is greater than
        * 0, then the players are able to spawn on it.
        */
        private GameObject GetValidSpawn(GameObject[,] arr, int min, int max)
        {
            while (true)
            {
                var ranX = Random.Range(min, max);
                var ranY = Random.Range(min, max);

                var tile = arr[ranX - 1, ranY - 1];

                var mapTile = tile.GetComponent<MapTile>();

                if (mapTile.neighbours.Count > 1)
                    return tile;
            }
        }

        public void MoveTank(GameObject tank, GameObject tile)
        {
            tank.GetComponent<Tank>().currentTile = tile;
            var target = tile.GetComponent<MapTile>().GetTop();
            target.y += 6.25f; // Adjust for tank model height
            tank.GetComponent<Tank>().target = target;
        }

        private GameObject CreateTankGameObject(string tankName, int health, int moveDistance, GameObject tile)
        {
            var spawnLocation = tile.GetComponent<MapTile>().GetTop();

            spawnLocation.y += 6.25f; // Adjust for tank model height

            var go = Instantiate(_tankGameObject, spawnLocation, Quaternion.identity);
            go.tag = "Enemy";
            go.GetComponent<Tank>().Create(tankName, health, moveDistance, tile);
            go.name = "Enemy-" + tankName;
            return go;
        }
    }
}