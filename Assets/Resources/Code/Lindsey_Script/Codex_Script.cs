using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Codex_Script : MonoBehaviour
{

    //**********************
    // Panels
    //**********************

    [Header("Panels")]

    public GameObject sargDieselPanel;
    public GameObject PV1Panel;
    public GameObject PV2Panel;
    public GameObject medicPanel;
    public GameObject pistonPanel;
    public GameObject enemyPanel;
    public GameObject overallPanel;
  

    //**********************
    // Animators
    //**********************
    private Animator _sargDieselAnimator;
    private Animator _PV1Animator;
    private Animator _PV2Animator;
    private Animator _medicAnimator;
    private Animator _pistolAnimator;
    private Animator _enemyAnimator;
    private Animator _overallAnimator;

    
    void Start(){
        // Get animators for panels
        _sargDieselAnimator = sargDieselPanel.GetComponent<Animator>();
        _PV1Animator = PV1Panel.GetComponent<Animator>();
        _PV2Animator = PV2Panel.GetComponent<Animator>();
        _medicAnimator = medicPanel.GetComponent<Animator>();
        _pistolAnimator = pistonPanel.GetComponent<Animator>();
        _enemyAnimator = enemyPanel.GetComponent<Animator>();
        _overallAnimator = overallPanel.GetComponent<Animator>();
    }

    public void ToggleSargPanel()
    {
        // Close other panels
        HidePanel(_PV1Animator);
        HidePanel(_PV2Animator);
        HidePanel(_medicAnimator);
        HidePanel(_pistolAnimator);
        HidePanel(_enemyAnimator);
        HidePanel(_overallAnimator);
        
        // Toggle Sarg panel
        TogglePanel(_sargDieselAnimator);
    }

    public void TogglePV1Panel()
    {
        // Close other panels
        HidePanel(_sargDieselAnimator);
        HidePanel(_PV2Animator);
        HidePanel(_medicAnimator);
        HidePanel(_pistolAnimator);
        HidePanel(_enemyAnimator);
        HidePanel(_overallAnimator);
        
        // Toggle PV1 panel
        TogglePanel(_PV1Animator);
    }

    public void TogglePV2Panel()
    {
        // Close other panels
        HidePanel(_PV1Animator);
        HidePanel(_sargDieselAnimator);
        HidePanel(_medicAnimator);
        HidePanel(_pistolAnimator);
        HidePanel(_enemyAnimator);
        HidePanel(_overallAnimator);
        
        // Toggle PV2 panel
        TogglePanel(_PV2Animator);
    }

    public void ToggleMedicPanel()
    {
        // Close other panels
        HidePanel(_PV1Animator);
        HidePanel(_PV2Animator);
        HidePanel(_sargDieselAnimator);
        HidePanel(_pistolAnimator);
        HidePanel(_enemyAnimator);
        HidePanel(_overallAnimator);
        
        // Toggle Medic panel
        TogglePanel(_medicAnimator);
    }

    public void TogglePistolPanel()
    {
        // Close other panels
        HidePanel(_PV1Animator);
        HidePanel(_PV2Animator);
        HidePanel(_medicAnimator);
        HidePanel(_sargDieselAnimator);
        HidePanel(_enemyAnimator);
        HidePanel(_overallAnimator);
        
        // Toggle Pistol panel
        TogglePanel(_pistolAnimator);
    }

    public void ToggleEnemyPanel()
    {
        // Close other panels
        HidePanel(_PV1Animator);
        HidePanel(_PV2Animator);
        HidePanel(_medicAnimator);
        HidePanel(_sargDieselAnimator);
        HidePanel(_pistolAnimator);
        HidePanel(_overallAnimator);
        
        // Toggle enemy panel
        TogglePanel(_enemyAnimator);
    }

    public void ToggleOverallPanel()
    {
        // Close other panels
        HidePanel(_PV1Animator);
        HidePanel(_PV2Animator);
        HidePanel(_medicAnimator);
        HidePanel(_sargDieselAnimator);
        HidePanel(_enemyAnimator);
        HidePanel(_pistolAnimator);
        
        // Toggle overall panel
        TogglePanel(_overallAnimator);
    }


    public void Back()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private static void TogglePanel(Animator animator)
    {
        animator.SetBool("Show", !animator.GetBool("Show"));
    }

    private static void HidePanel(Animator animator)
    {
        animator.SetBool("Show", false);
    }


}
