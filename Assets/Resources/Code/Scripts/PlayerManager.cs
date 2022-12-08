using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Resources.Code.Scripts
{
    public class PlayerManager : MonoBehaviour
    {
        private List<GameObject> _tankList;
        private List<int> _selection;
        
        private int _numberOfPlayers;

        private GameObject _tankGameObject;

        private float _moveSpeed = 50f;

        public void LoadModels()
        {
             _tankGameObject = UnityEngine.Resources.Load("Prefabs/Tank") as GameObject;
        }

        public void SpawnTanks(List<int> playerSelection, GameObject[,] mapArray)
        {
            _selection = playerSelection;
            _numberOfPlayers = playerSelection.Count;
            
            _tankList = new List<GameObject>();
            
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
                
                _tankList.Add(tempTank);
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

        public void MoveTank(GameObject tank, Vector3 targetPosition)
        {
            Vector3.MoveTowards(tank.transform.position, targetPosition, _moveSpeed * Time.deltaTime);
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