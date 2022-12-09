using System;
using UnityEngine;

namespace Resources.Code.Scripts
{
    public class Tank : MonoBehaviour
    {

        public string tankName;
        public float health;
        public int moveDistance;
        
        public GameObject currentTile;
        public Vector3 target;

        private const float MoveSpeed = 50f;

        public bool hasMoved = false;
        public bool hasAttacked = false;

        private void Update()
        {
            if (transform.position != target)
            {
                var t = transform;
                t.position = Vector3.MoveTowards(t.position, target, MoveSpeed * Time.deltaTime);
                t.LookAt(target);
            }
        }

        public void Create(string tankNameIn, int healthIn, int moveDistanceIn, GameObject tile)
        {
            tankName = tankNameIn;
            health = healthIn;
            moveDistance = moveDistanceIn;
            
            currentTile = tile;
            
            target = transform.position;
        }

        public void Recoilless(Vector3 selectedTile)
        {
            Debug.Log(selectedTile);
        }
        public void Special(Vector3 selectedTile)
        {
            
        }


    }
}