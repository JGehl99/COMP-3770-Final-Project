using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class DontDestroyOnLoadScript: MonoBehaviour{
    
    
    //*******************************************
    // Singleton to hold data across entire game
    //*******************************************
    
    public static DontDestroyOnLoadScript instance;//my singleton
    
    //**********************
    // Modifiable Values
    //**********************
    
    [Range(1, 3)]
    public int allyAmountOfTanks = 1; 
    
    [Range(1, 6)]
    public int enemyAmountOfTanks = 1;

    //public List<int> selectedTanks; 
    public int[] selectedTanks= {-1,-1,-1};

    public int selectedColor = 0;
    
    [Range(0f, 1f)]
    public float backgroundMusicVolume = 1f;
    
    [Range(0f, 1f)]
    public float sfxMusicVolume = 1f; 
    
    
    //***************
    // Audio Sources
    //***************
    
    public AudioSource backgroundMusic, sfxMusic;

    private void Awake(){
        
        //Check if instance isn't null to make sure not to create any duplicates 
        if(instance != null){   
            Destroy(gameObject);
            return;
        }
        
        instance = this;

        //Don't destroy values between each scene change
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start(){
        //selectedTanks = new List<int>();

        // Get AudioSource component for bg music
        backgroundMusic = GetComponent<AudioSource>();
        
        // Play music
        backgroundMusic.loop = true;
        backgroundMusic.Play(0);

        // Load Main Menu scene
        SceneManager.LoadScene("Main Menu");
    }
}
