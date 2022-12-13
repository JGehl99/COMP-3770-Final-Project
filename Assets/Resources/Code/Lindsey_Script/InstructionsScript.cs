using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructionsScript : MonoBehaviour{

    public void BackToMainMenu(){
        SceneManager.LoadScene("Main Menu");
    }
}

