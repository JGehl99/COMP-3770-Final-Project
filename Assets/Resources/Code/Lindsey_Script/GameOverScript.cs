using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    //*********************
    // OnClick Load Scenes
    //*********************
    
    public void BackToMainMenu(){
        SceneManager.LoadScene("Main Menu");
    }

    public void Replay(){
        SceneManager.LoadScene("MapTestScene");
    }
    
    public void ExitGame(){
        Application.Quit();
    }

}
