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
    
    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    }
        
    public void Codex()
    {
        SceneManager.LoadScene("Codex");
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

}
