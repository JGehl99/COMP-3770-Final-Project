using System.Collections.Generic;
using UnityEngine;

namespace Resources.Code.Scripts
{
    public class PlayerManager : MonoBehaviour
    {
        public List<GameObject> tankList;
        private List<int> _selection;
        
        private Material _matRed;
        private Material _matGreen;
        private Material _matBlue;
        private Material _matYellow;
        private Material _matOrange;
        private Material _matPurple;

        private Material _tankMat;
        

        private int _numberOfPlayers;
        private GameObject _tankGameObject;

        public void LoadModels()
        {
            _tankGameObject = UnityEngine.Resources.Load("Prefabs/Tank") as GameObject;
            
            _matRed = UnityEngine.Resources.Load("Materials/TankMatRed", typeof(Material)) as Material;
            _matGreen = UnityEngine.Resources.Load("Materials/TankMatGreen", typeof(Material)) as Material;
            _matBlue = UnityEngine.Resources.Load("Materials/TankMatBlue", typeof(Material)) as Material;
            _matYellow = UnityEngine.Resources.Load("Materials/TankMatYellow", typeof(Material)) as Material;
            _matOrange = UnityEngine.Resources.Load("Materials/TankMatOrange", typeof(Material)) as Material;
            _matPurple = UnityEngine.Resources.Load("Materials/TankMatPurple", typeof(Material)) as Material;
        }

        public void SpawnTanks(List<int> playerSelection, GameObject[,] mapArray)
        {
            _selection = playerSelection;
            _numberOfPlayers = playerSelection.Count;

            tankList = new List<GameObject>();
            //Spawn each of the tanks in the selection list
            foreach (var i in _selection)
            {
                //Determine if the tile they are spawning on is valid, see GetValidSpawn
                var tile = GetValidSpawn(mapArray, 0, 10);
                var tempTank = i switch
                {
                    0 => CreateTankGameObject("PVT Hull", 100, 3, tile),
                    1 => CreateTankGameObject("PV2 Clutch", 100, 3, tile),
                    2 => CreateTankGameObject("Cpl Sprocket", 100, 3, tile),
                    3 => CreateTankGameObject("SGM Diesel", 100, 3, tile),
                    4 => CreateTankGameObject("Lt Piston", 100, 3, tile),
                    _ => CreateTankGameObject("Lt Piston", 100, 3, tile)
                };

                tankList.Add(tempTank);
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
            tile.GetComponent<MapTile>().tankOnTile = tank;
        }

        private GameObject CreateTankGameObject(string tankName, int health, int moveDistance, GameObject tile)
        {
            var spawnLocation = tile.GetComponent<MapTile>().GetTop();

            spawnLocation.y += 6.25f; // Adjust for tank model height

            var go = Instantiate(_tankGameObject, spawnLocation, Quaternion.identity);
            go.GetComponent<Tank>().Create(tankName, health, moveDistance, tile);
            go.name = "Tank-" + tankName;
            tile.GetComponent<MapTile>().tankOnTile = go;

            _tankMat = DontDestroyOnLoadScript.instance.selectedColor switch
            {
                0 => _matRed,
                1 => _matGreen,
                2 => _matBlue,
                3 => _matYellow,
                4 => _matOrange,
                5 => _matPurple,
                _ => _matGreen
            };

            var mats = go.transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>().materials;
            mats[0] = _tankMat;
            go.transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>().materials = mats;
            
            mats = go.transform.GetChild(0).transform.GetChild(1).GetComponent<MeshRenderer>().materials;
            mats[0] = _tankMat;
            go.transform.GetChild(0).transform.GetChild(1).GetComponent<MeshRenderer>().materials = mats;
            
            mats = go.transform.GetChild(0).transform.GetChild(2).GetComponent<MeshRenderer>().materials;
            mats[0] = _tankMat;
            go.transform.GetChild(0).transform.GetChild(2).GetComponent<MeshRenderer>().materials = mats;
            
            mats = go.transform.GetChild(0).transform.GetChild(3).GetComponent<MeshRenderer>().materials;
            mats[0] = _tankMat;
            go.transform.GetChild(0).transform.GetChild(3).GetComponent<MeshRenderer>().materials = mats;
            
            return go;
        }
    }
}