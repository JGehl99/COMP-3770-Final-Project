using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Highscore_Script : MonoBehaviour{

    public Button Back_Button;
   
    void Start(){
        Back_Button.onClick.AddListener(Back_to_MainMenu);
        
    }

    void Back_to_MainMenu(){
        SceneManager.LoadScene("Main Menu");
    }

   
}

