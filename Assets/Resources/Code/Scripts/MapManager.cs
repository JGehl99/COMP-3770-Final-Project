using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Resources.Code.Scripts
{
    public class MapManager : MonoBehaviour
    {
        public GameObject gameObj;
        private int _zMax = 10;
        private int _xMax = 10;
        private int _yRange = 10;
        private GameObject[,] _mapArray;

        public int xInput = 10;
        public int yRange = 0;
        public int zInput = 10;
        public int maxMoveDist = 2;

        void Start()
        {
            _xMax = xInput;
            _zMax = zInput;
            _yRange = yRange;   //TODO: Generate Y height using perlin noise

            _mapArray = new GameObject[_xMax,_zMax];
            GenerateMap();
        }

        private static Vector3 GetBounds(GameObject go)
        {
            // Create temp object, return bounds of MeshCollider
            var temp = Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity);
            var bounds = temp.GetComponent<MeshCollider>().bounds.size;
            Destroy(temp);
            return bounds;
        }

        public void GenerateMap()
        {
            // Clear existing map
            foreach (var go in _mapArray) Destroy(go);
            
            // Create map array
            _mapArray = new GameObject[_xMax,_zMax];
            
            // Get hexagon tile bounds
            var objBounds = GetBounds(gameObj);
            
            // Loop over cells in array, instantiate Map Tile game object and instantiate the MapTile script

            for (var z = 0; z < _zMax; z++)
            {
                for (var x = 0; x < _xMax; x++)
                {
                    _mapArray[x,z] = Instantiate(gameObj, new Vector3(0, 0, 0), Quaternion.identity, transform);
                    _mapArray[x,z].GetComponent<MapTile>().Instantiate(x, _yRange, z, objBounds);
                    
                    // TODO: Set color based on generated height
                    _mapArray[x,z].GetComponent<MeshRenderer>().material.color = Color.green;
                }
            }
            
            // Create Neighbour lists, loop over all cells in mapArray
            for (var z = 0; z < _zMax; z++)
            {
                for (var x = 0; x < _xMax; x++)
                {
                    var arList = new List<GameObject>();
                    

                    if (Math.Abs(_mapArray[x, z].transform.position.y - _mapArray[x, z - 1].transform.position.y) < 2)
                    {
                        arList.Add(_mapArray[x, z - 1]); // Bottom
                    }

                    if (Math.Abs(_mapArray[x, z].transform.position.y - _mapArray[x - 1, z].transform.position.y) < 2)
                    {
                        arList.Add(_mapArray[x - 1, z]); // Left
                    }

                    if (Math.Abs(_mapArray[x, z].transform.position.y - _mapArray[x + 1, z].transform.position.y) < 2)
                    {
                        arList.Add(_mapArray[x + 1, z]); // Right
                    }

                    if (Math.Abs(_mapArray[x, z].transform.position.y - _mapArray[x, z + 1].transform.position.y) < 2)
                    {
                        arList.Add(_mapArray[x, z + 1]); // Top
                    }

                    // Due to the nature of hexagonal grids, the cells which are neighbours will be different for every
                    // other row
                    //
                    // Even Row Neighbours:      Odd Row Neighbours:
                    //     *                            * * *
                    //   * T *                          * T *
                    //   * * *                            *
                    
                    if (x % 2 == 0)
                    {
                        if (Math.Abs(
                            _mapArray[x, z].transform.position.y - _mapArray[x + 1, z - 1].transform.position.y) < 2)
                        {
                            arList.Add(_mapArray[x + 1, z - 1]); // Bottom Right
                        }
                    
                        if (Math.Abs(
                                _mapArray[x, z].transform.position.y -
                                _mapArray[x - 1, z - 1].transform.position.y) < 2)
                        {
                            arList.Add(_mapArray[x - 1, z - 1]); // Bottom Left 
                        }
                    }
                    else
                    {
                        if (Math.Abs(
                            _mapArray[x, z].transform.position.y -
                            _mapArray[x + 1, z + 1].transform.position.y) < 2)
                        {
                            arList.Add(_mapArray[x + 1, z + 1]); // Top Right
                        }

                        if (Math.Abs(
                                _mapArray[x, z].transform.position.y -
                                _mapArray[x - 1, z + 1].transform.position.y) < 2)
                        {
                            arList.Add(_mapArray[x - 1, z + 1]); // Top Left 
                        }
                    }
                    
                    // Set neighbours list for current Tile
                    _mapArray[x,z].GetComponent<MapTile>().neighbours = arList;
                }
            }

            // Loop over every tile again and generate Dictionary of movement lists for n move distance
            for (var z = 0; z < _zMax; z++)
            {
                for (var x = 0; x < _xMax; x++)
                {
                    _mapArray[x,z].GetComponent<MapTile>().movementLists = GenerateMovementList(x, z, maxMoveDist);
                }
            }
        }

        public Dictionary<int, List<GameObject>> GenerateMovementList(int x, int z, int n)
        {
            // Initialize lists
            var visitedList = new List<GameObject>();
            var checkList = new List<List<GameObject>>();
            
            // Initialize movementLists, set movementLists[0] to empty list so we can use it when a tank can't move
            var movementLists = new Dictionary<int, List<GameObject>>();
            movementLists[0] = new List<GameObject>();
            
            // Add first tile to checkList
            checkList.Add(new List<GameObject> {_mapArray[x,z]});

            var steps = 0;

            // While there are still Tiles to visit
            while (checkList.Count > 0)
            {
                // Reinitialize nextStep list
                var nextStep = new List<GameObject>();
                
                // Grab the first list from checkList
                var step = checkList[0];
                
                // Create notVisited list, all tiles in step that are not already in the visitedList
                var notVisited = step.Where(tile => !visitedList.Contains(tile)).ToList();
                
                // Loop over all Tiles in notVisited
                foreach (var tile in notVisited)
                {
                    // Add Tile to visitedList
                    visitedList.Add(tile);
                    
                    // Add neighbours of current tile to nextStep list
                    nextStep.AddRange(tile.GetComponent<MapTile>().neighbours);
                }
                
                // Add list to movementLists at current step
                movementLists.Add(++steps, visitedList);
                
                // Add nextStep to checkList and remove the list that was just checked
                checkList.Add(nextStep);
                checkList.RemoveAt(0);

                // Once n steps is reached, return movementLists
                if (steps == n) return movementLists;
            }
            // If checkList ever ends up empty, return empty Dictionary
            return new Dictionary<int, List<GameObject>>();
        }

        public GameObject[,] GetMapArray()
        {
            return _mapArray;
        }
    }
}

