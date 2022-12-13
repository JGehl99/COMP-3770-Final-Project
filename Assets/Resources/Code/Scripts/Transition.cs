using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    public Animator transitionAnim; 
    public GameObject panel; 
    public string sceneName;

    public void TransitionStart(string scenename){
       
        sceneName = scenename;
        StartCoroutine(LoadScene());
       
    }

    IEnumerator LoadScene(){
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
    }
}
