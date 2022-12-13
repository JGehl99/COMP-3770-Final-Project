using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;

public class TankManager : MonoBehaviour{
    public TankDatabase TankDB;
    public TMP_Text name_text,tank_information_text; 
    public Image _chosenTanksPanel; 

    //public GameObject[] Selected_Tanks_tmp; 

    public GameObject[] allTanks; 
    
    public int selectedTank =0;
    public GameObject Tank4,Tank5,Tank6;
    public TMP_Text chosenTank1,chosenTank2,chosenTank3,chosenTank4,chosenTank5,chosenTank6;
   
    void Start(){     
        Tanks tank = TankDB.GetTank(selectedTank); //loads the first tank's information
        name_text.text = tank.Tank_Name;
        tank_information_text.text = tank.Tank_Information;
        
        if(DontDestroyOnLoadScript.instance.allyAmountOfTanks == 4){
            _chosenTanksPanel.rectTransform.sizeDelta = new Vector2(450, 550);
            Tank4.SetActive(true);  
        }
        if(DontDestroyOnLoadScript.instance.allyAmountOfTanks == 5){
            _chosenTanksPanel.rectTransform.sizeDelta = new Vector2(450, 625);
            Tank4.SetActive(true);
            Tank5.SetActive(true);
        }
        if(DontDestroyOnLoadScript.instance.allyAmountOfTanks == 6){
            _chosenTanksPanel.rectTransform.sizeDelta = new Vector2(450, 700);
            Tank4.SetActive(true);
            Tank5.SetActive(true);
            Tank6.SetActive(true); 
        }
        //Selected_Tanks_tmp = new GameObject[DontDestroyOnLoadScript.instance.allyAmountOfTanks]; 
    }


    public void BackToGameSetUp()
    {
        SceneManager.LoadScene("GameSetUp");
    }

    public void NextTank()
    {
        allTanks[selectedTank].SetActive(false);
        selectedTank = (selectedTank+1)%allTanks.Length;
        allTanks[selectedTank].SetActive(true);
        Debug.Log(allTanks[selectedTank]);

        Tanks tank = TankDB.GetTank(selectedTank); 
        name_text.text = tank.Tank_Name;
        tank_information_text.text = tank.Tank_Information;
    }
    
    public void PrevTank()
    {
        allTanks[selectedTank].SetActive(false);
        selectedTank--;
        if(selectedTank <0 ){
            selectedTank+=allTanks.Length;
        }
        allTanks[selectedTank].SetActive(true);
        Debug.Log(allTanks[selectedTank]);

        Tanks tank = TankDB.GetTank(selectedTank);
        name_text.text = tank.Tank_Name;
        tank_information_text.text = tank.Tank_Information;
    }
  
    //0 = pv2
    //1 = sarg
    //2 = pv1
    //3 = medic
    //4 = piston
  
    public void ChooseTank(){
        Tanks tank = TankDB.GetTank(selectedTank);
        if(DontDestroyOnLoadScript.instance.selectedTanks[0]==-1){
            DontDestroyOnLoadScript.instance.selectedTanks[0] = selectedTank;
            chosenTank1.text = tank.Tank_Name;
        }
        else if(DontDestroyOnLoadScript.instance.selectedTanks[1]==-1){
            DontDestroyOnLoadScript.instance.selectedTanks[1] = selectedTank;
            chosenTank2.text = tank.Tank_Name;
        }
        else if(DontDestroyOnLoadScript.instance.selectedTanks[2]==-1){
            DontDestroyOnLoadScript.instance.selectedTanks[2] = selectedTank;
            chosenTank3.text = tank.Tank_Name;
        }
        else if(DontDestroyOnLoadScript.instance.allyAmountOfTanks>3&&DontDestroyOnLoadScript.instance.selectedTanks[3]==-1){
            DontDestroyOnLoadScript.instance.selectedTanks[3] = selectedTank;
            chosenTank4.text = tank.Tank_Name;
        }
        else if(DontDestroyOnLoadScript.instance.allyAmountOfTanks>3&&DontDestroyOnLoadScript.instance.selectedTanks[4]==-1){
            DontDestroyOnLoadScript.instance.selectedTanks[4] = selectedTank;
            chosenTank5.text = tank.Tank_Name;
        }
        else if(DontDestroyOnLoadScript.instance.allyAmountOfTanks>3&&DontDestroyOnLoadScript.instance.selectedTanks[5]==-1){
            DontDestroyOnLoadScript.instance.selectedTanks[5] = selectedTank;
            chosenTank6.text = tank.Tank_Name;
        }
    }

    public void DeleteTank(int buttonNumber)
    {
        if(buttonNumber ==1){
            DontDestroyOnLoadScript.instance.selectedTanks[0] = -1;
            chosenTank1.text = "Tank1: Empty";
        }
        if(buttonNumber ==2){
            DontDestroyOnLoadScript.instance.selectedTanks[1] = -1;
            chosenTank2.text = "Tank2: Empty";
        }
        if(buttonNumber ==3){
            DontDestroyOnLoadScript.instance.selectedTanks[2] = -1;
            chosenTank3.text = "Tank3: Empty";
        }
        if(buttonNumber ==4){
            DontDestroyOnLoadScript.instance.selectedTanks[3] = -1;
            chosenTank4.text = "Tank4: Empty";
        }
        if(buttonNumber ==5){
            DontDestroyOnLoadScript.instance.selectedTanks[4] = -1;
            chosenTank5.text = "Tank5: Empty";
        }
        if(buttonNumber ==6){
            DontDestroyOnLoadScript.instance.selectedTanks[5] = -1;
            chosenTank6.text = "Tank6: Empty";
        }
    }
}
