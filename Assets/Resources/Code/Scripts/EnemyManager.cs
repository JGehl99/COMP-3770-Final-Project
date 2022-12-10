using UnityEngine;

namespace Resources.Code.Scripts
{
    public class EnemyManager : MonoBehaviour
    {
        // private int _numOfEnemies = 3;
        private GameObject _tankGameObject;

        public void LoadModels()
        {
            _tankGameObject = UnityEngine.Resources.Load("Prefabs/Tank") as GameObject;
        }

        public void SpawnTanks(int numOfEnemies, GameObject[,] mapArray)
        {
            //Spawning the enemies on the field at the opposite corner than the player
            for (int i = 0; i < numOfEnemies; i++)
            {
                //TODO - Get the max size of the map as the max, - 10 for the min. 
                var tile = GetValidSpawn(mapArray, 0, 10);
                CreateTankGameObject($"EnemyTank{i}", 100, 3, tile);
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
            target.y += 6.25f; // Adjust for tank model height
            tank.GetComponent<Tank>().target = target;
        }

        private GameObject CreateTankGameObject(string tankName, int health, int moveDistance, GameObject tile)
        {
            var spawnLocation = tile.GetComponent<MapTile>().GetTop();

            spawnLocation.y += 6.25f; // Adjust for tank model height

            var go = Instantiate(_tankGameObject, spawnLocation, Quaternion.identity);
            go.GetComponent<Tank>().Create(tankName, health, moveDistance, tile);
            go.name = "Enemy-" + tankName;
            return go;
        }
    }
}