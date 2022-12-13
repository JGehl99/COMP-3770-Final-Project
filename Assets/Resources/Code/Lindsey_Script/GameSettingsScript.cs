using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSettingsScript : MonoBehaviour{
    
    public Button volumeButton;
    public Image volumeOn,volumeOff;
    public Slider sfxSlider, musicSlider,masterSlider; 
    public bool volumeState = true;//true means on, false means off
   
    void Start(){
        volumeOff.enabled=false;
        volumeButton.onClick.AddListener(Volume_Adjust);
    }

    public void BackToMainMenu(){
        SceneManager.LoadScene("Main Menu");
    }
    
    void OnEnable(){
        musicSlider.onValueChanged.AddListener(delegate { changeVolume(musicSlider.value); });
        masterSlider.onValueChanged.AddListener(delegate { changeVolume(masterSlider.value); });
    }

    void changeVolume(float sliderValue){
        DontDestroyOnLoadScript.instance.backgroundMusic.volume = sliderValue;
    }

    void OnDisable(){
        musicSlider.onValueChanged.RemoveAllListeners();
        masterSlider.onValueChanged.RemoveAllListeners();
    }

    void Volume_Adjust(){
        if(volumeState ==true){//If volume is already on, turn off
            volumeState = false;//set bool to false to turn off
            volumeOn.enabled=false;//change picture (this hides the volume on button)
            volumeOff.enabled=true;//change picture (this shows the volume off button)
            DontDestroyOnLoadScript.instance.backgroundMusic.Pause();
        }
        else{
            volumeState = true;
            volumeOff.enabled=false;//change picture (this hides the volume off button)
            volumeOn.enabled=true;//change picture (this shows the volume on button)
            DontDestroyOnLoadScript.instance.backgroundMusic.UnPause();
        }
    }
}
