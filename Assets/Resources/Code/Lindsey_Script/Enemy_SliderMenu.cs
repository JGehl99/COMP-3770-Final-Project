using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Enemy_SliderMenu : MonoBehaviour{
    public GameObject Enemy_Slider_Menu;
    public GameObject amount_of_tanks_panel;
    public Button Amount_Button,Enemy_Slider_Button;
    public bool slider_update;
    public Slider slider;
    public int slider_int;
    public TMP_Text enemy_tank_amount_text;

    void Start(){
        Amount_Button.onClick.AddListener(showAmount);
        Enemy_Slider_Button.onClick.AddListener(ShowHideMenu);
        enemy_tank_amount_text.text = "";
    }

    void Update(){
        if(Enemy_Slider_Menu !=null){
            slider_int = (int)slider.value;
            enemy_tank_amount_text.text = slider_int.ToString();
        }
    }

    public void ShowHideMenu(){
        if(Enemy_Slider_Menu != null){
            Animator animator = Enemy_Slider_Menu.GetComponent<Animator>();
            if(animator != null){
                bool isOpen = animator.GetBool("Show");
                animator.SetBool("Show",!isOpen);
            }
        }
    }

    void showAmount(){
        Debug.Log("amount of tanks");
        if(amount_of_tanks_panel != null){
            Animator animator = amount_of_tanks_panel.GetComponent<Animator>();
            if(animator != null){
                bool isOpen = animator.GetBool("Show");
                animator.SetBool("Show",!isOpen);
            }
        }
        slider_update = true;    
    }
}
