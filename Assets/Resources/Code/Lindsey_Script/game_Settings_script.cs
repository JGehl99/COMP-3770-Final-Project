using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class game_Settings_script : MonoBehaviour{
    public Button Back_Button,Volume_Button;
    public Image volume_on,volume_off;
    public Slider SFX_Slider, Music_Slider,Master_Slider; 
    public bool Volume_State = true;//true means on, false means off
   
    void Start(){
        volume_off.enabled=false;
        Back_Button.onClick.AddListener(Back_to_MainMenu);
        Volume_Button.onClick.AddListener(Volume_Adjust);    
    }

    void Back_to_MainMenu(){
        SceneManager.LoadScene("Main Menu");
    }
    
    void OnEnable(){
        Music_Slider.onValueChanged.AddListener(delegate { changeVolume(Music_Slider.value); });
    }

    void changeVolume(float sliderValue){
        DontDestroyOnLoad_Script.Instance.Background_Music.volume = sliderValue;
    }

    void OnDisable(){
        Music_Slider.onValueChanged.RemoveAllListeners();
    }

    void Volume_Adjust(){
        if(Volume_State ==true){//If volume is already on, turn off
            Volume_State = false;//set bool to false to turn off
            volume_on.enabled=false;//change picture (this hides the volume on button)
            volume_off.enabled=true;//change picture (this shows the volume off button)
            DontDestroyOnLoad_Script.Instance.Background_Music.Pause();
        }
        else{
            Volume_State = true;
            volume_off.enabled=false;//change picture (this hides the volume off button)
            volume_on.enabled=true;//change picture (this shows the volume on button)
            DontDestroyOnLoad_Script.Instance.Background_Music.UnPause();
        }

      
    }
}
