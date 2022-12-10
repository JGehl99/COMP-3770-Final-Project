using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DontDestroyOnLoad_Script: MonoBehaviour{
    public static DontDestroyOnLoad_Script Instance;//my singleton
    public int Ally_Amount_of_Tanks=3,Enemy_Amount_of_Tanks=3; 
    public AudioSource Background_Music,SFX_Music;
    
    public float Background_Music_Volume, SFX_Music_Volume; 

    private void Awake(){
        if(Instance != null){//makes sure not to create any duplicates 
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);//makes sure to not destroy my values between each scene change
    }
    void Start(){
        Background_Music = GetComponent<AudioSource>();
        Background_Music.Play(0);
        SceneManager.LoadScene("Main Menu");
    }
}
