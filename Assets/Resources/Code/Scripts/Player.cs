using UnityEngine;

namespace Resources.Code.Scripts
{
    public class Player
    {
        private string _playerName;
        private float _health;
        private Vector3 _position;

        private MapTile _currentTile;

        public Player(string playerName, float health)
        {
            _playerName = playerName;
            _health = health;
        }

        /*
         * Accessor Methods
         */
        public string PlayerName
        {
            get => _playerName;
            set => _playerName = value;
        }

        public float Health
        {
            get => _health;
            set => _health = value;
        }

        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public MapTile CurrentTile
        {
            get => _currentTile;
            set => _currentTile = value;
        }
    }
}