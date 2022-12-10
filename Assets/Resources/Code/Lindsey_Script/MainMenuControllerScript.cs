using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuControllerScript : MonoBehaviour{
    public Button Play_Button,Exit_GameOption_Button,Settings_Button,Instruction_Button, HighScore_Button, Quit_Button,File_Button;
    public GameObject Quit_Panel, GameType_Panel;
    public GameObject[] Selected_Tanks; 
    public string filePath; 
    public RawImage file_Viewer;
    //public File load_file; 

    void Start(){
        Play_Button.onClick.AddListener(PlayGame);
        Instruction_Button.onClick.AddListener(Instructions);
        HighScore_Button.onClick.AddListener(HighScores);
        Quit_Button.onClick.AddListener(QuitGame); 
        Settings_Button.onClick.AddListener(Settings); 
        Exit_GameOption_Button.onClick.AddListener(Exit_GameOption); 
        //File_Button.onClick.AddListener(FindFile); 
    }

    void PlayGame(){
        if(GameType_Panel == true){
            GameType_Panel.SetActive(true);
        }
    }

    void Exit_GameOption(){
        if(GameType_Panel == true){
            GameType_Panel.SetActive(false);
        }
    }

    void Instructions(){
        SceneManager.LoadScene("Instructions");
    }

    void Settings(){
        SceneManager.LoadScene("GameSettings");
    }

    void HighScores(){
        SceneManager.LoadScene("HighScores");
    }

    public void GameType_Selection(int choice){//choice == 1 new game    //choice == 0 load game
        if(choice ==1){
            Debug.Log("New Game");
            SceneManager.LoadScene("GameSetUp");
        }
        else{
            Debug.Log("Load Game");
        }
    }

    public void QuitGame_Selection(int choice){//choice == 0 no    //choice == 1 yes
        if(choice ==1){
            Application.Quit();
        }
        else{
            Quit_Panel.SetActive(false);
        }
    }
  
    void QuitGame(){
        if(Quit_Panel == true){//makes quit_panel appear asking if user wants to quit the game 
            Quit_Panel.SetActive(true);
        }
    }
/*
    public void FindFile(){
        var browser = new BrowserProperties();
        //browser.filter();
        new FileBrowser().OpenFileBrowser (browser,path =>{
        
            StartCoroutine(LoadFile(path));
        });
    }

    IEnumerator LoadFile(string path){
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
 
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

 */
}
