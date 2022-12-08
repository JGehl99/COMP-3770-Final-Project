using UnityEngine;
using Debug = System.Diagnostics.Debug;
/*
 * TODO - Use an A* path finding algorithm to move along tiles in graph/map
 */
namespace Resources.Code.Scripts
{
    public class MoveTankToTile : MonoBehaviour
    {
        public Vector3 targetPosition;
        public float speed;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            speed = 50.0f;
        }

        private void Update()
        {
            //Checks for mouse input
            if (Input.GetMouseButtonDown(0))
            {
                //Creating a raycast at the position of the mouse click
                //TODO Check how much this impacts performance
                Debug.Assert(_camera != null, "Camera.main != null");
                Ray raycast = _camera.ScreenPointToRay(Input.mousePosition);
                //The raycast hit object
                RaycastHit hit;
			
                if (Physics.Raycast(raycast, out hit))
                {
                    //Check to see the name of the gameObject
                    //TODO this can be substituted in for another type of check, if needed
                    if (hit.transform.gameObject.name == "Hex - Copy(Clone)")
                    {
                        var go = hit.transform.gameObject.GetComponent<MapTile>();
                        // targetPosition = hit.transform.position; //Sets the target position to the position of the hit
                        //Move the character to the targetPosition
                        targetPosition = go.GetTop();
                    }
                }
            }
            if(transform.position != targetPosition && !targetPosition.Equals(null))
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}