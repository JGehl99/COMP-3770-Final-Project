using UnityEngine;
using UnityEngine.UI;

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

        private AudioSource _movementAudioSource;
        private AudioSource _shotAudioSource;

        private AudioClip _tankShotSound;
        private AudioClip _tankMoveSound;
        private AudioClip _tankDestroySound;


        private ParticleSystem _shotParticles;

        private GameObject _healthBar;
        private GameObject _health;

        public bool hasDied;


        private void Start()
        {
            _movementAudioSource = gameObject.AddComponent<AudioSource>();
            _shotAudioSource = gameObject.AddComponent<AudioSource>();

            _tankMoveSound = UnityEngine.Resources.Load<AudioClip>("Audio/tankMove");
            _tankShotSound = UnityEngine.Resources.Load<AudioClip>("Audio/tankShot");
            _tankDestroySound = UnityEngine.Resources.Load<AudioClip>("Audio/tankDestroy");

            hasDied = false;
            
            
        }

        private void Update()
        {
            if (transform.position != target)
            {
                if (!_movementAudioSource.isPlaying) _movementAudioSource.PlayOneShot(_tankMoveSound);

                var t = transform;
                t.position = Vector3.MoveTowards(t.position, target, MoveSpeed * Time.deltaTime);
                t.LookAt(target);
            }
            else
            {
                _movementAudioSource.Stop();
            }

            if (health == 0)
            {
                hasDied = true;
            }


        }

        public void Create(string tankNameIn, int healthIn, int moveDistanceIn, GameObject tile)
        {
            tankName = tankNameIn;
            health = healthIn;
            moveDistance = moveDistanceIn;

            currentTile = tile;

            target = transform.position;


            _shotParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
        }

        public void Recoilless(GameObject selectedTile, bool playerTank)
        {
            Debug.Log(selectedTile.GetComponent<MapTile>().GetTop());

            var tileScript = selectedTile.GetComponent<MapTile>();
            var tilePos = tileScript.GetTop();

            Debug.Log("Recoilless!");
            
            _shotAudioSource.PlayOneShot(_tankShotSound, 1.0f);
            _shotParticles.Play();
            
            StartCoroutine(tileScript.TriggerExplosion());
            
            TakeDamage(tileScript);

        }

        public void Shrapnel(GameObject selectedTile)
        {
            Debug.Log(selectedTile.GetComponent<MapTile>().GetTop());
            
            Debug.Log("Shrapnel!");

            _shotAudioSource.PlayOneShot(_tankShotSound, 1.0f);
            _shotParticles.Play();

            foreach (var go in selectedTile.GetComponent<MapTile>().movementLists[1])
            {
                var mapTile = go.GetComponent<MapTile>();
                var tilePos = mapTile.GetTop();

                StartCoroutine(mapTile.TriggerExplosion());
                
                TakeDamage(mapTile);
            }
        }

        public void TakeDamage(MapTile selectedTile)
        {
            var tileScript = selectedTile.GetComponent<MapTile>();
            
            if (tileScript.tankOnTile != null)
            {
                var tankHit = tileScript.tankOnTile;
                
                tankHit.GetComponent<Tank>().health -= 25;
                

                var newScaleX = tankHit.transform.GetChild(2).localScale.x - 2.5;

                var newPosX = tankHit.transform.GetChild(2).localPosition.x - 1;

                Vector3 newScale = new Vector3((float)newScaleX, 2.5f, 0.1f);
                
                Vector3 newPos = new Vector3(newPosX, 15.0f, 0.0f);

                tankHit.transform.GetChild(2).localScale = newScale;
                tankHit.transform.GetChild(2).localPosition = newPos;
                
                Debug.Log(tankHit.GetComponent<Tank>().health);
                
                

            }
        }

    }
}