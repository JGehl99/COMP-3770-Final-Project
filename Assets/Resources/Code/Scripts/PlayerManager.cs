using System.Collections.Generic;
using UnityEngine;

namespace Resources.Code.Scripts
{
    public class PlayerManger : MonoBehaviour
    {
        private List<Player> _playerList;
        public List<int> selection;
        public int numPlayers;
        private Player _lt = new Player("Lt", 100);
        private Player _sgt = new Player("Sgt", 100);
        private Player _cpl = new Player("Cpl", 100);
        private Player _fm = new Player("Fm", 100);
        private Player _psc = new Player("Psc", 100);

        public GameObject tank;
        public MapManager mapManager;

        public void Start()
        {
            _playerList = new List<Player>();

            //Determining which of the characters the player has selected
            foreach (var i in selection)
            {
                _playerList.Add(SelectPlayerFromList(selection, i));
            }

            var arr = mapManager.GetMapArray();
            
            //Spawning the player objects
            foreach (var player in _playerList)
            {
                //Setting the spawn location for each player object if it is a valid location
                var tile = IsValidSpawn(arr);
                var spawnLocation = tile.GetTop();
                
                spawnLocation.y = spawnLocation.y + 6.25f;
                player.Position = spawnLocation;
                
                Instantiate(tank, player.Position, Quaternion.identity);
            }
        }
        
        /*
         * Checks to see if the given MapTile is a valid spawning location
         * by checking to see how many neighbours it has, if it is greater than
         * 0, then the players are able to spawn on it.
         */
        private MapTile IsValidSpawn(GameObject[,] arr)
        {
            while (true)
            {
                var ranX = Random.Range(0, 10);
                var ranY = Random.Range(0, 10);

                var tile = arr[ranX, ranY].GetComponent<MapTile>();

                if (tile.neighbours.Count > 1)
                    return tile;
            }
        }

        /* Given the index of the list, return the selected player*/
        private Player SelectPlayerFromList(List<int> list, int i)
        {
            switch (list.IndexOf(i))
            {
                case 0:
                    return _lt;
                case 1:
                    return _sgt;
                case 2:
                    return _cpl;
                case 3:
                    return _fm;
                case 4:
                    return _psc;
                default:
                    return _lt;
            }
        }

        public MapManager MapManager
        {
            get => mapManager;
            set => mapManager = value;
        }
    }
}