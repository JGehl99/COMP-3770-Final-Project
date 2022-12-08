using UnityEngine;

namespace Resources.Code.Scripts
{
    public class Tank : MonoBehaviour
    {

        public string tankName;
        public float health;
        public int moveDistance;
        
        public GameObject currentTile;

        public bool hasMoved;

        public bool hasAttacked;


        public void Create(string tankNameIn, int healthIn, int moveDistanceIn, GameObject tile)
        {
            tankName = tankNameIn;
            health = healthIn;
            moveDistance = moveDistanceIn;
            
            currentTile = tile;
        }
    }
}