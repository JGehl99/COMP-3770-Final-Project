using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Resources.Code.Scripts
{
    public class PlayerManger : MonoBehaviour
    {
        private List<Player> _playerList;
        [FormerlySerializedAs("_selection")] public List<int> selection;
        public int numPlayers;
        private Player _lt = new Player("Lt", 100);
        private Player _sgt = new Player("Sgt", 100);
        private Player _cpl = new Player("Cpl", 100);
        private Player _fm = new Player("Fm", 100);
        private Player _psc = new Player("Psc", 100);

        public GameObject tank;
        public void Start()
        {
            _playerList = new List<Player>();
            /*
             * On start, instantiate the player game objects, based on number of players
             * that the user selects
             */
            foreach (var i in selection)
            {
                switch (selection[i])
                {
                    case 0:
                        _playerList.Add(_lt);
                        break;
                    case 1:
                        _playerList.Add(_sgt);
                        break;
                    case 2:
                        _playerList.Add(_cpl);
                        break;
                    case 3:
                        _playerList.Add(_fm);
                        break;
                    case 4:
                        _playerList.Add(_psc);
                        break;
                }
            }

            foreach (var player in _playerList)
            {
                //TODO set a position using the MapTile.GetTop(), at a random tile in X range
                player.Position = new Vector3(0, 0, 0);
                Instantiate(tank, player.Position, Quaternion.identity);
            }
        }
        
    }
}