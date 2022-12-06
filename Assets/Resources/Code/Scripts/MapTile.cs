using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources.Code.Scripts
{
    public class MapTile : MonoBehaviour
    {
        // x and y position in mapArray
        public int x;
        public int z;
        
        // _size is the distance in unity coords form the center of the hexagon to the right side point
        private float _size;
        
        // _xWidth is the distance from the left point to the right point of the hexagon (2 * size)
        private float _xWidth;
        
        // _zLength is the distance from the flat top edge to the flat bottom edge (sqrt(3) * size)
        private float _zLength;
        
        // X, Y, Z in unity coords of the bounds of the hexagon tile
        private Vector3 _bounds;
        
        // Materials for default color and onHover color
        public Material hoverMaterial;
        public Material defaultMaterial;
        
        // Flag if mouse is hovering
        private bool _hoverFlag;
        
        // MeshRenderer
        private MeshRenderer _r;
        
        // Adjacency list
        public List<GameObject> neighbours = new();
        
        // Dictionary that holds lists of which tiles can be stepped to in n moves
        public Dictionary<int, List<GameObject>> movementLists = new();
        
        // Tile ID
        public string id;
        
        // Reference to MapManager to get mapArray
        private MapManager _mm;
        

        // Start is called before the first frame update
        void Start()
        {
            // Set material colors for default color and onHover color
            defaultMaterial = UnityEngine.Resources.Load("Materials/DefaultMaterial", typeof(Material)) as Material;
            hoverMaterial = UnityEngine.Resources.Load("Materials/HoverMaterial", typeof(Material)) as Material;

            _r = GetComponent<MeshRenderer>(); // Get MeshRenderer

            // Sets Material to defaultMaterial
            var mats = new Material[1];
            mats[0] = defaultMaterial;
            _r.materials = mats;

            // Get reference to MapManager
            _mm = GameObject.Find("MapManager").GetComponent<MapManager>();
        }

        // OnMouseOver, highlight all tiles in that movement list for specified step
        void OnMouseOver()
        {
            if (_hoverFlag) return;
            
            _hoverFlag = true;

            foreach (var go in movementLists[_mm.maxMoveDist])
            {
                highlight(go);
            }
            highlight(gameObject);
        }

        // On Mouse Exit, unhighlight all tiles in that movement list for specified step
        void OnMouseExit()
        {
            _hoverFlag = false;
            foreach (var go in movementLists[_mm.maxMoveDist])
            {
                unhighlight(go);
            }
            
            unhighlight(gameObject);
        }

        public void Instantiate(int xi, int y, int zi, Vector3 bounds)
        {
            
            // Set x, z
            x = xi;
            z = zi;

            // Create ID
            id = x + "-" + z;

            // Get bounds, calculate size, width, height in unity coordinates
            _bounds = bounds;
            _size = _bounds.x / 2f;
            _xWidth = _size * 2f;
            _zLength = (float)Math.Sqrt(3f) * _size;

            // If even row, move tile to x z position, if odd row, offset by half the zLength
            if (x % 2 == 0)
            {
                transform.Translate(x * _xWidth * (3 / 4f),  y, z * _zLength);
            }
            else
            {
                transform.Translate(x * _xWidth * (3 / 4f), y, (z * _zLength) + _zLength / 2);
            }
        }

        // Highlight function changes the material of the gameObject
        public void highlight(GameObject go)
        {
            var mat1 = go.GetComponent<MapTile>().defaultMaterial;
            var mat2 = go.GetComponent<MapTile>().hoverMaterial;
            go.GetComponent<MeshRenderer>().material.Lerp(mat1, mat2, 5.0f);
        }

        //UnHighlight function changes the material of the gameObject
        public void unhighlight(GameObject go)
        {
            var mat1 = go.GetComponent<MapTile>().defaultMaterial;
            var mat2 = go.GetComponent<MapTile>().hoverMaterial;
            go.GetComponent<MeshRenderer>().material.Lerp(mat2, mat1, 5.0f);
        }

        public Vector3 GetWorldCoords()
        {
            return transform.position;
        }

        public Vector2 GetMapCoords()
        {
            return new Vector2(x, z);
        }
    }
}
