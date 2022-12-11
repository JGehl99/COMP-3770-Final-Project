using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuControllerScript : MonoBehaviour{
    
    //*********************
    // Game Objects
    //**********************

    public GameObject quitPanel;
    public GameObject gameTypePanel;
    public GameObject[] selectedTanks; 
    
    //*********************
    // Stuff for files
    //**********************

    public string filePath; 
    public RawImage fileViewer;
    //public File load_file; 

    
    public void OpenGameTypePanel()
    {
        gameTypePanel.SetActive(true);
    }
    
    public void CloseGameTypePanel()
    {
        gameTypePanel.SetActive(false);
    }
    
    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    }
    
    public void Settings()
    {
        SceneManager.LoadScene("GameSettings");
    }
    
    public void HighScores()
    {
        SceneManager.LoadScene("HighScores");
    }

    public void GameSetUp()
    {
        SceneManager.LoadScene("GameSetUp");
    }

    public void LoadGame()
    {
        Debug.Log("Load Game");
    }
    
    public void OpenQuitPanel()
    {
        quitPanel.SetActive(true);
    }

    public void CloseQuitPanel()
    {
        quitPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
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
