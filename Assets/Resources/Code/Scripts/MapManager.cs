using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;


namespace Resources.Code.Scripts
{
    public class MapManager : MonoBehaviour
    {
        public GameObject tileGameObject;
        public GameObject waterGameObject;
        private GameObject _pm;
        private int _zMax = 10;
        private int _xMax = 10;
        private GameObject[,] _mapArray;

        public int xInput = 10;
        public int zInput = 10;
        public int maxMoveDist = 2;

        private float _waterHeight = 16.4f;

        void Start()
        {
            _xMax = xInput;
            _zMax = zInput;

            _mapArray = new GameObject[_xMax, _zMax];
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
            _mapArray = new GameObject[_xMax, _zMax];

            // Create Blue Plane to represent water
            Instantiate(waterGameObject, new Vector3(0, _waterHeight, 0), Quaternion.identity);

            // Get hexagon tile bounds
            var objBounds = GetBounds(tileGameObject);

            // Loop over cells in array, instantiate Map Tile game object and instantiate the MapTile script

            for (var z = 0; z < _zMax; z++)
            {
                for (var x = 0; x < _xMax; x++)
                {
                    // Instantiate tile at 0, 0, 0
                    _mapArray[x, z] = Instantiate(tileGameObject, new Vector3(0, 0, 0), Quaternion.identity, transform);

                    // Map values of x and z from between 0 and 100, to between 0 and 1
                    var xC = (x - 100) * 1f / (100 - 10);
                    var zC = (z - 10) * 1f / (100 - 10);

                    // Use perlin noise to generate tile height
                    float y = Mathf.PerlinNoise(xC * 30f, zC * 10f + Random.Range(1, 10)) * 15;

                    // Instantiate MapTile script which moves tile to final position
                    _mapArray[x, z].GetComponent<MapTile>().Instantiate(x, y, z, objBounds);
                }
            }

            // Create Neighbour lists, loop over all cells in mapArray
            for (var z = 0; z < _zMax; z++)
            {
                for (var x = 0; x < _xMax; x++)
                {
                    var arList = new List<GameObject>();
                    
                    var tile = _mapArray[x, z];
                    
                    try
                    {
                        var bottom = _mapArray[x, z - 1];

                        if (IsValidNeighbour(tile, bottom))
                        {
                            arList.Add(bottom);
                        }
                    }
                    catch (IndexOutOfRangeException ignored)
                    {
                    }

                    try
                    {
                        var left = _mapArray[x - 1, z];
                        if (IsValidNeighbour(tile, left))
                        {
                            arList.Add(left);
                        }
                    }
                    catch (IndexOutOfRangeException ignored)
                    {
                    }

                    try
                    {
                        var right = _mapArray[x + 1, z];
                        if (IsValidNeighbour(tile, right))
                        {
                            arList.Add(right);
                        }
                    }
                    catch (IndexOutOfRangeException ignored)
                    {
                    }

                    try
                    {
                        var top = _mapArray[x, z + 1];
                        if (IsValidNeighbour(tile, top))
                        {
                            arList.Add(top);
                        }
                    }
                    catch (IndexOutOfRangeException ignored)
                    {
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
                        try
                        {
                            var bottomRight = _mapArray[x + 1, z - 1];
                            if (IsValidNeighbour(tile, bottomRight))
                            {
                                arList.Add(bottomRight);
                            }
                        }
                        catch (IndexOutOfRangeException ignored)
                        {
                        }

                        try
                        {
                            var bottomLeft = _mapArray[x - 1, z - 1];
                            if (IsValidNeighbour(tile, bottomLeft))
                            {
                                arList.Add(bottomLeft);
                            }
                        }
                        catch (IndexOutOfRangeException ignored)
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            var topRight = _mapArray[x + 1, z + 1];
                            if (IsValidNeighbour(tile, topRight))
                            {
                                arList.Add(topRight);
                            }
                        }
                        catch (IndexOutOfRangeException ignored)
                        {
                        }

                        try
                        {
                            var topLeft = _mapArray[x - 1, z + 1];
                            if (IsValidNeighbour(tile, topLeft))
                            {
                                arList.Add(topLeft);
                            }
                        }
                        catch (IndexOutOfRangeException ignored)
                        {
                        }
                    }

                    // Set neighbours list for current Tile
                    _mapArray[x, z].GetComponent<MapTile>().neighbours = arList;
                }
            }

            // Loop over every tile again and generate Dictionary of movement lists for n move distance
            for (var z = 0; z < _zMax; z++)
            {
                for (var x = 0; x < _xMax; x++)
                {
                    _mapArray[x, z].GetComponent<MapTile>().movementLists = GenerateMovementList(x, z, maxMoveDist);
                }
            }
            
            //Instan the player manager
            _pm = UnityEngine.Resources.Load("Prefabs/PlayerManager") as GameObject;
            // Debug.Assert(_pm != null, nameof(_pm) + " != null");
            _pm.GetComponent<PlayerManger>().MapManager = this;
            Instantiate(_pm, new Vector3(0, 0, 0), Quaternion.identity);

        }

        private bool IsValidNeighbour(GameObject tile, GameObject neighbour)
        {
            var neighbourY = neighbour.transform.position.y;
            // Checks if tile height difference is less than 2 and that the neighbour is above water
            return Math.Abs(tile.transform.position.y - neighbourY) < 2 && neighbourY > 3.58f;
        }

        private Dictionary<int, List<GameObject>> GenerateMovementList(int x, int z, int n)
        {
            // Initialize lists
            var visitedList = new List<GameObject>();
            var checkList = new List<List<GameObject>>();

            // Initialize movementLists, set movementLists[0] to empty list so we can use it when a tank can't move
            var movementLists = new Dictionary<int, List<GameObject>>
            {
                [0] = new()
            };

            // Add first tile to checkList
            checkList.Add(new List<GameObject> { _mapArray[x, z] });

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