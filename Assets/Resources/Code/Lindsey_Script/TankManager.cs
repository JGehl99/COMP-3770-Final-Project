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
    public GameObject Tank2,Tank3;
    public TMP_Text chosenTank1,chosenTank2,chosenTank3;
   
    void Start()
    {     
        Tanks tank = TankDB.GetTank(selectedTank); //loads the first tank's information
        name_text.text = tank.Tank_Name;
        tank_information_text.text = tank.Tank_Information;

        if(DontDestroyOnLoadScript.instance.allyAmountOfTanks == 1){
            Tank2.SetActive(false);
            Tank3.SetActive(false); 
        }
        else if(DontDestroyOnLoadScript.instance.allyAmountOfTanks == 2){
            _chosenTanksPanel.rectTransform.sizeDelta = new Vector2(450, 375);
            Tank2.SetActive(true);
            Tank3.SetActive(false);
        }
        else if(DontDestroyOnLoadScript.instance.allyAmountOfTanks == 3){
            _chosenTanksPanel.rectTransform.sizeDelta = new Vector2(450, 500);
            Tank2.SetActive(true);
            Tank3.SetActive(true); 
        }
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
  
    public void ChooseTank()
    {
        Tanks tank = TankDB.GetTank(selectedTank);
        //DontDestroyOnLoadScript.instance.selectedTanks.Add(selectedTank);  
        
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

/*
        if(DontDestroyOnLoadScript.instance.selectedTanks.IndexOf(selectedTank)==0){
            chosenTank1.text = tank.Tank_Name;
        } 
        else if(DontDestroyOnLoadScript.instance.selectedTanks.IndexOf(selectedTank)==1){
            chosenTank2.text = tank.Tank_Name;
        } 
        else if(DontDestroyOnLoadScript.instance.selectedTanks.IndexOf(selectedTank)==2){
            chosenTank3.text = tank.Tank_Name;
        } 
        else{
            return; 
        }

        */
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
        /*
        Tanks tank = TankDB.GetTank(selectedTank);
            
        if(buttonNumber==1){
            chosenTank1.text = "Tank1: Empty";
            DontDestroyOnLoadScript.instance.selectedTanks.Remove(selectedTank);
        } 
        else if(buttonNumber==2){
            chosenTank2.text = "Tank2: Empty";
            DontDestroyOnLoadScript.instance.selectedTanks.Remove(selectedTank);
        } 
        else if(buttonNumber==3){
            chosenTank3.text = "Tank3: Empty";
            DontDestroyOnLoadScript.instance.selectedTanks.Remove(selectedTank);
        } 
        else{
            return; 
        }
        */
    }
}
