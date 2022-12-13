using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public GameObject menu;
    //on button press load a scene, in this case it would be the MainMenu scene.
    public void LoadScene(string scenename){
        SceneManager.LoadScene(scenename);
    }

    //If escape is pressed, unloack the cursor, and if the menu is already active relock the cursor and remove the menu, it it isn't active make it active.
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(menu.activeSelf){
                menu.SetActive(false);
            }else{
                menu.SetActive(true);
            }
        }
    }

    public void DisableMenu()
    {
        menu.SetActive(false);
    }

}
