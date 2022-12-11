using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class setup_script : MonoBehaviour{
    //MAIN-START-----------------------
    public TMP_InputField get_team_name; 
    public TMP_Text team_name_text;
    public Button redButton,greenButton,blueButton,yellowButton,orangeButton,purpleButton;
    public Button Emblem1_Button,Emblem2_Button,Emblem3_Button,Emblem4_Button;
    public Button Back_Button,Start_Button;
    public string team_name;
    public Image playerIcon,emblem1,emblem2,emblem3,emblem4;
    //MAIN-END--------------------------
    
    //ALLY-SLIDER-START-------------------------
    public GameObject Ally_Slider_Menu,team_options_panel, AllyAmountOfTanks_Panel;
    public Button AllyAmount_Button,Option_Button,Team_Button,Ally_Slider_Button;
    public Slider AllySlider;
    public int AllySliderAmount=3;
    public TMP_Text AllyTankAmount_Text;
    private Animator Ally_animator, Amount_animator,Team_animator;
    //ALLY_SLIDER-END-----------------------------

    //ENEMY-SLIDER-START------------------
    public GameObject Enemy_Slider_Menu;
    public GameObject EnemyAmountOfTanks_Panel;
    public Button EnemyAmount_Button,Enemy_Slider_Button;
    public Slider EnemySlider;
    public int EnemySliderAmount;
    public TMP_Text EnemyTankAmount_Text;
    //ENEMY-SLIDER-END-------------

    void Start(){
        //MAIN
        get_team_name.onEndEdit.AddListener(onSubmit);
        redButton.onClick.AddListener(DisplayColour);
        blueButton.onClick.AddListener(DisplayColour);
        greenButton.onClick.AddListener(DisplayColour);
        yellowButton.onClick.AddListener(DisplayColour);
        orangeButton.onClick.AddListener(DisplayColour);
        purpleButton.onClick.AddListener(DisplayColour);

        Emblem1_Button.onClick.AddListener(DisplayEmblem);
        Emblem2_Button.onClick.AddListener(DisplayEmblem);
        Emblem3_Button.onClick.AddListener(DisplayEmblem);
        Emblem4_Button.onClick.AddListener(DisplayEmblem);

        Back_Button.onClick.AddListener(BackToMain);
        Start_Button.onClick.AddListener(StartGame);
        
        emblem1.enabled=false;
        emblem2.enabled=false;
        emblem3.enabled=false;
        emblem4.enabled=false;

        //ALLY-SLIDER
        Ally_Slider_Button.onClick.AddListener(ShowHideMenu);
        AllyAmount_Button.onClick.AddListener(showAmount);
        Option_Button.onClick.AddListener(showOptions);
        Team_Button.onClick.AddListener(showTeam);
        
        //DontDestroyOnLoad_Script.Instance.Ally_Amount_of_Tanks =3;

        Ally_animator = Ally_Slider_Menu.GetComponent<Animator>();
        Ally_animator.SetBool("Show",true);
        Amount_animator = AllyAmountOfTanks_Panel.GetComponent<Animator>();
        Team_animator = team_options_panel.GetComponent<Animator>();

        //ENEMY-SLIDER
        EnemyAmount_Button.onClick.AddListener(Enemy_showAmount);
        Enemy_Slider_Button.onClick.AddListener(Enemy_ShowHideMenu);
        EnemyTankAmount_Text.text = "";
    }

    void onSubmit(string name){//gets the entered text from the InputField, and updates the users name
        team_name_text.text = name;
    }

    void BackToMain(){
        SceneManager.LoadScene("Main Menu");
    }

    void StartGame(){
        Debug.Log("start game");
        //SceneManager.LoadScene("StartGame");
    }

    void DisplayColour(){
        string Clicked_Button_Name = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("Colour: "+Clicked_Button_Name);
        if(Clicked_Button_Name == "button_colour_red"){
            playerIcon.color = new Color32(141,0,0,255);
        }
        if(Clicked_Button_Name == "button_colour_green"){
            playerIcon.color = new Color32(18,94,18,255);
        }
        if(Clicked_Button_Name == "button_colour_blue"){
            playerIcon.color = new Color32(0,0,139,255);
        }
        if(Clicked_Button_Name == "button_colour_yellow"){
            playerIcon.color = new Color32(188,169,0,255);
        }
        if(Clicked_Button_Name == "button_colour_orange"){
            playerIcon.color = new Color32(188,71,0,255);
        }
        if(Clicked_Button_Name == "button_colour_purple"){
            playerIcon.color = new Color32(71,22,74,255);
        }
    }

    void DisplayEmblem(){
        string Clicked_Button_Name = EventSystem.current.currentSelectedGameObject.name;
        //Debug.Log("BUTTON: "+Clicked_Button_Name);

        if(Clicked_Button_Name == "emblem1"){
            emblem1.enabled = !emblem1.enabled; 
            emblem2.enabled = false;
            emblem3.enabled = false;
            emblem4.enabled = false;
            playerIcon.rectTransform.sizeDelta = new Vector2(75, 75);
        }
        if(Clicked_Button_Name == "emblem2"){
            emblem2.enabled = !emblem2.enabled;
            emblem1.enabled = false;
            emblem3.enabled = false;
            emblem4.enabled = false;
            playerIcon.rectTransform.sizeDelta = new Vector2(75, 75);
        }
        if(Clicked_Button_Name=="emblem3"){
            emblem3.enabled = !emblem3.enabled;
            emblem2.enabled = false;
            emblem1.enabled = false;
            emblem4.enabled = false;
            playerIcon.rectTransform.sizeDelta = new Vector2(100, 75);
        }
        if(Clicked_Button_Name=="emblem4"){
            emblem4.enabled = !emblem4.enabled;
            emblem2.enabled = false;
            emblem3.enabled = false;
            emblem1.enabled = false;
            playerIcon.rectTransform.sizeDelta = new Vector2(100, 75);
        }
    }

    public void ShowHideMenu(){
        if(Ally_Slider_Menu != null){
            if(Ally_animator != null){
                bool isOpen = Ally_animator.GetBool("Show");
                Ally_animator.SetBool("Show",!isOpen);
            }
        }
    }

    void showAmount(){
        Team_animator.SetBool("Show",false);
        if(AllyAmountOfTanks_Panel != null){
            if(Amount_animator != null){
                bool isOpen = Amount_animator.GetBool("Show");
                Amount_animator.SetBool("Show",!isOpen);
            }
        }
        AllySliderAmount = (int)AllySlider.value;
        AllyTankAmount_Text.text = AllySliderAmount.ToString();
        DontDestroyOnLoad_Script.Instance.Ally_Amount_of_Tanks= AllySliderAmount;   
    }

    void showOptions(){
        SceneManager.LoadScene("TankSelection");
    }

    void showTeam(){
        Amount_animator.SetBool("Show",false);
        if(Ally_Slider_Menu != null){
            if(Team_animator != null){
                bool isOpen = Team_animator.GetBool("Show");
                Team_animator.SetBool("Show",!isOpen);
            }
        }
    }


  //ENEMY-SLIDER START---------
  public void Enemy_ShowHideMenu(){
        if(Enemy_Slider_Menu != null){
            Animator animator = Enemy_Slider_Menu.GetComponent<Animator>();
            if(animator != null){
                bool isOpen = animator.GetBool("Show");
                animator.SetBool("Show",!isOpen);
            }
        }
    }

    void Enemy_showAmount(){
        Debug.Log("amount of tanks");
        if(EnemyAmountOfTanks_Panel != null){
            Animator animator = EnemyAmountOfTanks_Panel.GetComponent<Animator>();
            if(animator != null){
                bool isOpen = animator.GetBool("Show");
                animator.SetBool("Show",!isOpen);
            }
        }
        EnemySliderAmount = (int)EnemySlider.value;
        EnemyTankAmount_Text.text = EnemySliderAmount.ToString();
        DontDestroyOnLoad_Script.Instance.Enemy_Amount_of_Tanks= EnemySliderAmount;    
    }
    //ENEMY-SLIDER ENDS---
}
