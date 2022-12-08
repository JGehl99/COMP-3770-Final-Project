﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources.Code.Scripts
{
    public class MapManager : MonoBehaviour
    {
        private GameObject _tileGameObject;
        private GameObject _waterGameObject;
        private GameObject _pm;
        private int _zMax;
        private int _xMax;
        private int _maxMoveDist;

        private const float WaterHeight = 16.4f;

        public void LoadModels()
        {
            // Set Water Plane and Tile objects
            _waterGameObject = UnityEngine.Resources.Load("Prefabs/WaterPlane") as GameObject;
            _tileGameObject = UnityEngine.Resources.Load("Prefabs/Hex - Copy") as GameObject;
        }

        public void GenerateMap(int xx, int zz, int maxMoveDist)
        {

            // Set Variables
            _xMax = xx;
            _zMax = zz;
            _maxMoveDist = maxMoveDist;
            
            // Create map array
            MapArray = new GameObject[_xMax, _zMax];

            // Create Blue Plane to represent water
            Instantiate(_waterGameObject, new Vector3(0, WaterHeight, 0), Quaternion.identity);

            // Get hexagon tile bounds
            var objBounds = GetBounds(_tileGameObject);
            Debug.Log(objBounds);

            // Loop over cells in array, instantiate Map Tile game object and instantiate the MapTile script

            for (var z = 0; z < _zMax; z++)
            {
                for (var x = 0; x < _xMax; x++)
                {
                    // Instantiate tile at 0, 0, 0
                    MapArray[x, z] = Instantiate(_tileGameObject, new Vector3(0, 0, 0), Quaternion.identity, transform);

                    // Map values of x and z from between 0 and 100, to between 0 and 1
                    var xC = (x - 100) * 1f / (100 - 10);
                    var zC = (z - 10) * 1f / (100 - 10);

                    // Use perlin noise to generate tile height
                    float y = Mathf.PerlinNoise(xC * 20f, zC * 20f) * 15;

                    // Instantiate MapTile script which moves tile to final position
                    MapArray[x, z].GetComponent<MapTile>().Instantiate(x, y, z, objBounds);
                }
            }

            // Create Neighbour lists, loop over all cells in mapArray
            for (var z = 0; z < _zMax; z++)
            {
                for (var x = 0; x < _xMax; x++)
                {
                    var arList = new List<GameObject>();
                    
                    var tile = MapArray[x, z];
                    
                    try
                    {
                        var bottom = MapArray[x, z - 1];

                        if (IsValidNeighbour(tile, bottom))
                        {
                            arList.Add(bottom);
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }

                    try
                    {
                        var left = MapArray[x - 1, z];
                        if (IsValidNeighbour(tile, left))
                        {
                            arList.Add(left);
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }

                    try
                    {
                        var right = MapArray[x + 1, z];
                        if (IsValidNeighbour(tile, right))
                        {
                            arList.Add(right);
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }

                    try
                    {
                        var top = MapArray[x, z + 1];
                        if (IsValidNeighbour(tile, top))
                        {
                            arList.Add(top);
                        }
                    }
                    catch (IndexOutOfRangeException)
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
                            var bottomRight = MapArray[x + 1, z - 1];
                            if (IsValidNeighbour(tile, bottomRight))
                            {
                                arList.Add(bottomRight);
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                        }

                        try
                        {
                            var bottomLeft = MapArray[x - 1, z - 1];
                            if (IsValidNeighbour(tile, bottomLeft))
                            {
                                arList.Add(bottomLeft);
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            var topRight = MapArray[x + 1, z + 1];
                            if (IsValidNeighbour(tile, topRight))
                            {
                                arList.Add(topRight);
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                        }

                        try
                        {
                            var topLeft = MapArray[x - 1, z + 1];
                            if (IsValidNeighbour(tile, topLeft))
                            {
                                arList.Add(topLeft);
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                        }
                    }

                    // Set neighbours list for current Tile
                    MapArray[x, z].GetComponent<MapTile>().neighbours = arList;
                }
            }

            // Loop over every tile again and generate Dictionary of movement lists for n move distance
            for (var z = 0; z < _zMax; z++)
            {
                for (var x = 0; x < _xMax; x++)
                {
                    MapArray[x, z].GetComponent<MapTile>().movementLists = GenerateMovementList(x, z, maxMoveDist);
                }
            }
        }

        private bool IsValidNeighbour(GameObject tile, GameObject neighbour)
        {
            var neighbourY = neighbour.transform.position.y;
            // Checks if tile height difference is less than 2 and that the neighbour is above water
            return Math.Abs(tile.transform.position.y - neighbourY) < 2 && neighbourY > 3.58f;
        }
        
        private static Vector3 GetBounds(GameObject go)
        {
            // Create temp object, return bounds of MeshCollider
            var temp = Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity);
            var bounds = temp.GetComponent<MeshCollider>().bounds.size;
            Destroy(temp);
            return bounds;
        }

        private Dictionary<int, List<GameObject>> GenerateMovementList(int x, int z, int n)
        {
            // Initialize lists
            var visitedList = new List<GameObject>();
            var checkList = new List<List<GameObject>>();

            // Initialize movementLists, set movementLists[0] to empty list so we can use it when a tank can't move
            var movementLists = new Dictionary<int, List<GameObject>>();

            // Add first tile to checkList
            checkList.Add(new List<GameObject> { MapArray[x, z] });

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
                movementLists.Add(steps++, visitedList.ToList());

                // if (x == 0 && z == 0)
                // {
                //     foreach(var p in movementLists[steps-1])
                //     {
                //         var q = p.gameObject.GetComponent<MapTile>().id;
                //         Debug.Log(steps + ": " + q);
                //     }
                // }

                // Add nextStep to checkList and remove the list that was just checked
                checkList.Add(nextStep);
                checkList.RemoveAt(0);

                // Once n steps is reached, return movementLists
                if (steps == n) return movementLists;
            }

            // If checkList ever ends up empty, return empty Dictionary
            return new Dictionary<int, List<GameObject>>();
        }


        // Properties
        public GameObject[,] MapArray { get; private set; }
    }
}