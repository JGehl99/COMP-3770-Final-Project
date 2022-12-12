using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;

public class TankManager : MonoBehaviour{
    public TankDatabase TankDB;
    public Button Next_Button,Prev_Button,Choose_Button, Back_Button, confirm_Button;
    public TMP_Text name_text,tank_information_text; 
    public GameObject tank_model;
    private int Current_Tank =0,Tank_Amount;
    public Image Tank_Amount1_Image,Tank_Amount2_Image,Tank_Amount3_Image,Tank_Amount4_Image,Tank_Amount5_Image,Tank_Amount6_Image;
    public GameObject[] Selected_Tanks_tmp; 
    public GameObject[] allTanks; 
    public int selectedCharacter =0;
    public Image CheckMark1,CheckMark2,CheckMark3,CheckMark4,CheckMark5,CheckMark6;
    public Image CheckBox4,CheckBox5,CheckBox6;
    public Button Edit_Tanks_Button,Edit_Tanks_Next_Button,Edit_Tanks_Prev_Button,Edit_Tanks_Exit_Button,Edit_Tanks_Delete_Button;
    public GameObject Edit_Tanks_Panel;

    public GameObject PV1,PV2,Sarg_M_Diesel,L_Pistol,Medic;

   
    void Start(){
        Next_Button.onClick.AddListener(NextCharacter);
        Prev_Button.onClick.AddListener(PrevCharacter);
        //Choose_Button.onClick.AddListener(ChooseTank);
        //Back_Button.onClick.AddListener(BackToGameSetUp);
        //confirm_Button.onClick.AddListener(BackToGameSetUp);
        //Edit_Tanks_Button.onClick.AddListener(EditTanks);

        //updateTank(Current_Tank);
        Debug.Log("TANK AMOUNT: "+DontDestroyOnLoadScript.instance.allyAmountOfTanks);
        Tank_Amount = DontDestroyOnLoadScript.instance.allyAmountOfTanks;

        CheckMark1.enabled = false;
        CheckMark2.enabled = false;
        CheckMark3.enabled = false;
        CheckMark4.enabled = false;
        CheckMark5.enabled = false;
        CheckMark6.enabled = false;
        
        if(DontDestroyOnLoadScript.instance.allyAmountOfTanks ==3){
            Tank_Amount1_Image.enabled= true;
            Tank_Amount2_Image.enabled = true;
            Tank_Amount3_Image.enabled= true;

            Tank_Amount4_Image.enabled= false;
            Tank_Amount5_Image.enabled= false;
            Tank_Amount6_Image.enabled= false;

            CheckBox4.enabled = false;
            CheckBox5.enabled = false;
            CheckBox6.enabled = false; 
        }

        if(DontDestroyOnLoadScript.instance.allyAmountOfTanks == 4){
            Tank_Amount4_Image.enabled= true;
            Tank_Amount5_Image.enabled= false;
            Tank_Amount6_Image.enabled= false;
            CheckBox4.enabled = true;
            CheckBox5.enabled = false;
            CheckBox6.enabled = false; 
        }
        if(DontDestroyOnLoadScript.instance.allyAmountOfTanks == 5){
            Tank_Amount4_Image.enabled= true;
            Tank_Amount5_Image.enabled= true;
            Tank_Amount6_Image.enabled= false;
            CheckBox4.enabled = true;
            CheckBox5.enabled = true;
            CheckBox6.enabled = false; 
        }
        if(DontDestroyOnLoadScript.instance.allyAmountOfTanks == 6){
            Tank_Amount4_Image.enabled= true;
            Tank_Amount5_Image.enabled= true;
            Tank_Amount6_Image.enabled= true;
            CheckBox4.enabled = true;
            CheckBox5.enabled = true;
            CheckBox6.enabled = true; 
        }
        Selected_Tanks_tmp = new GameObject[DontDestroyOnLoadScript.instance.allyAmountOfTanks]; 
    }

     void EditTanks(){
        if(Edit_Tanks_Panel == true){
            Edit_Tanks_Panel.SetActive(true);
        }
    }

    public void BackToGameSetUp(){
        SceneManager.LoadScene("GameSetUp");
    }


    public void NextCharacter(){
        /*
      
        
        //updateTank(Current_Tank);
        allTanks[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter+1)%allTanks.Length;
        allTanks[selectedCharacter].SetActive(false);
      
        */
        Current_Tank++;
        
        TankDB.GetTank(Current_Tank).Tank.SetActive(true);

        if(Current_Tank >= TankDB.TankCount){
            Current_Tank = 0; 
        }
        Debug.Log("cur tank name: "+TankDB.GetTank(Current_Tank).Tank_Name);
        Debug.Log("cur tank name: "+TankDB.GetTank(Current_Tank).Tank);
        updateTank(Current_Tank);
    }

    
    public void PrevCharacter(){
        /*
        allTanks[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if(selectedCharacter <0 ){
            selectedCharacter+=allTanks.Length;
        }
        allTanks[selectedCharacter].SetActive(true);
        */
        Current_Tank--;
        if(Current_Tank < 0 ){
            Current_Tank = TankDB.TankCount -1;
        }
        Debug.Log("cur tank name: "+TankDB.GetTank(Current_Tank).Tank_Name);
        Debug.Log("cur tank model: "+TankDB.GetTank(Current_Tank).Tank);
        updateTank(Current_Tank);
    }

     void ChooseTank(){
        if(Selected_Tanks_tmp[0]==null){
            Selected_Tanks_tmp[0] = TankDB.GetTank(Current_Tank).Tank;
            CheckMark1.enabled = true;
        }
        else if(Selected_Tanks_tmp[1]==null){
            Selected_Tanks_tmp[1] = TankDB.GetTank(Current_Tank).Tank;
            CheckMark2.enabled = true;
        }
        else if(Selected_Tanks_tmp[2]==null){
            Selected_Tanks_tmp[2] = TankDB.GetTank(Current_Tank).Tank;
            CheckMark3.enabled = true;
        }
        else if(DontDestroyOnLoadScript.instance.allyAmountOfTanks>3&&Selected_Tanks_tmp[3]==null){
            Selected_Tanks_tmp[3] = TankDB.GetTank(Current_Tank).Tank;
            CheckMark4.enabled = true;
        }
        else if(DontDestroyOnLoadScript.instance.allyAmountOfTanks>3&&Selected_Tanks_tmp[4]==null){
            Selected_Tanks_tmp[4] = TankDB.GetTank(Current_Tank).Tank;
            CheckMark5.enabled = true;
        }
        else if(DontDestroyOnLoadScript.instance.allyAmountOfTanks>3&&Selected_Tanks_tmp[5]==null){
            Selected_Tanks_tmp[5] = TankDB.GetTank(Current_Tank).Tank;
            CheckMark6.enabled = true;
        }
    }
      private void updateTank(int Current_Tank){
        Tanks tank = TankDB.GetTank(Current_Tank); 
        tank_model = tank.Tank;
        name_text.text = tank.Tank_Name;
        tank_information_text.text = tank.Tank_Information;
        //tank model not moving to correct place
        //tank.Tank.transform.Rotate( _axis.normalized * _degreesPerSecond * Time.deltaTime );
    }



/*
    void NextTank(){
        /*
  

        Current_Tank++;

        if(Current_Tank >= TankDB.TankCount){
            Current_Tank = 0; 
        }
        Debug.Log("cur tank name: "+TankDB.GetTank(Current_Tank).Tank_Name);
        Debug.Log("cur tank name: "+TankDB.GetTank(Current_Tank).Tank);
        updateTank(Current_Tank);
    }

    /*
       0 => CreateTankGameObject("Lt", 100, 3, tile),
                    1 => CreateTankGameObject("Sgt", 100, 3, tile),
                    2 => CreateTankGameObject("Cpl", 100, 3, tile),
                    3 => CreateTankGameObject("Fm", 100, 3, tile),
                    4 => CreateTankGameObject("Psc", 100, 3, tile),
                    _ => CreateTankGameObject("Lt", 100, 3, tile)
    

   

   

    void PrevTank(){
        Animator animator = PV1.GetComponent<Animator>();
        animator.SetBool("Prev",true);
        animator.SetBool("Next",false);
        Current_Tank--;
        if(Current_Tank < 0 ){
            Current_Tank = TankDB.TankCount -1;
        }
        Debug.Log("cur tank name: "+TankDB.GetTank(Current_Tank).Tank_Name);
        Debug.Log("cur tank model: "+TankDB.GetTank(Current_Tank).Tank);
        updateTank(Current_Tank);
    }

  
    */
}
