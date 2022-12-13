using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private Material _hoverMaterial;
        private Material _defaultMaterial;
        private Material _attackMaterial;
        private Material _selectedTankMaterial;

        private Material _grassDefault;
        private Material _grassHover;
        private Material _sandDefault;
        private Material _sandHover;
        private Material _snowDefault;
        private Material _snowHover;

        private ParticleSystem _explosionParticles;
        
        private AudioSource _explosionAudioSource;
        private AudioClip _tankShotExplosionSound;
        
        // Flag if mouse is hovering
        private bool _hoverFlag;
        
        // MeshRenderer
        private MeshRenderer _r;
        
        // Adjacency list
        public List<GameObject> neighbours = new();
        public List<GameObject> enemyNeighbours = new();
        
        // Dictionary that holds lists of which tiles can be stepped to in n moves
        public Dictionary<int, List<GameObject>> movementLists;
        public Dictionary<int, List<GameObject>> enemyMovementLists;

        public bool isHighlighted;
        
        // Tile ID
        public string id;
        private int _tileType = 0;
        
        public List<GameObject> tanks;
        public bool tileContainsTank = false;
        public GameObject tankOnTile;

        public void Start()
        {
            _explosionAudioSource = gameObject.AddComponent<AudioSource>();
            _tankShotExplosionSound = UnityEngine.Resources.Load<AudioClip>("Audio/tankShotExplosion");
        }

        public void Instantiate(int xi, float y, int zi, Vector3 bounds)
        {
            // Set material colors for default color and onHover color
            _grassDefault = UnityEngine.Resources.Load("Materials/GrassDefault", typeof(Material)) as Material;
            _grassHover = UnityEngine.Resources.Load("Materials/GrassHover", typeof(Material)) as Material;
            _sandDefault = UnityEngine.Resources.Load("Materials/SandDefault", typeof(Material)) as Material;
            _sandHover = UnityEngine.Resources.Load("Materials/SandHover", typeof(Material)) as Material;
            _snowDefault = UnityEngine.Resources.Load("Materials/SnowDefault", typeof(Material)) as Material;
            _snowHover = UnityEngine.Resources.Load("Materials/SnowHover", typeof(Material)) as Material;
            _attackMaterial = UnityEngine.Resources.Load("Materials/AttackTile", typeof(Material)) as Material;
            _selectedTankMaterial = UnityEngine.Resources.Load("Materials/SelectedTank", typeof(Material)) as Material;

            _explosionParticles = transform.GetComponentInChildren<ParticleSystem>();

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
            
            // Debug.Log(y);
            
            // Set tile to sand, grass, or snow depending on height
            if (y <= 4.5f)
            {
                _defaultMaterial = _sandDefault;
                _hoverMaterial = _sandHover;
            } 
            else if (y is > 4.5f and <= 9f)
            {
                _defaultMaterial = _grassDefault;
                _hoverMaterial = _grassHover;
            }
            else
            {
                _defaultMaterial = _snowDefault;
                _hoverMaterial = _snowHover;
            }

            _r = GetComponent<MeshRenderer>();
    
            // Sets Material to defaultMaterial
            var mats = _r.materials;
            mats[0] = _defaultMaterial;
            _r.materials = mats;
        }

        // Highlight function changes the material of the gameObject
        public void Highlight(int moveDistance)
        {
            foreach (var go in movementLists[moveDistance])
            {

                foreach (var go1 in tanks)
                {
                    if (go == go1.GetComponent<Tank>().currentTile)
                    {
                        tileContainsTank = true;
                    }
                }

                if (!tileContainsTank)
                {
                    var mapTile = go.GetComponent<MapTile>();
                    mapTile.isHighlighted = true;
                
                    var mat1 = mapTile._defaultMaterial;
                    var mat2 = mapTile._hoverMaterial;

                    go.GetComponent<MeshRenderer>().material.Lerp(mat1, mat2, 1.0f);
                }
                
                tileContainsTank = false;
                
            }
            
        }

        //UnHighlight function changes the material of the gameObject
        public void Unhighlight(int moveDistance)
        {
            foreach (var go in movementLists[moveDistance])
            {
                var mapTile = go.GetComponent<MapTile>();
                mapTile.isHighlighted = false;
                
                var mat1 = mapTile._defaultMaterial;
                var mat2 = mapTile._hoverMaterial;
                
                go.GetComponent<MeshRenderer>().material.Lerp(mat2, mat1, 1.0f);
            }
        }

        public void HighlightAttack()
        {
            GetComponent<MeshRenderer>().material.Lerp(_defaultMaterial, _attackMaterial, 1.0f);
        }
        public void UnhighlightAttack()
        {
            GetComponent<MeshRenderer>().material.Lerp(_attackMaterial, _defaultMaterial, 1.0f);
        }
        
        public void HighlightSelect()
        {
            GetComponent<MeshRenderer>().material.Lerp(_defaultMaterial, _selectedTankMaterial, 1.0f);
        }
        public void UnhighlightSelect()
        {
            GetComponent<MeshRenderer>().material.Lerp(_selectedTankMaterial, _defaultMaterial, 1.0f);
        }

        public Vector3 GetTop()
        {
            var position = transform.position;
            return new Vector3(position.x, position.y + _bounds.y/2, position.z);
        }

        public IEnumerator TriggerExplosion()
        {
            yield return new WaitForSeconds(2f);
            _explosionAudioSource.PlayOneShot(_tankShotExplosionSound, 1.0f);
            yield return new WaitForSeconds(0.75f);
            _explosionParticles.Play();
        }
    }
}
