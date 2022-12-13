using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SetupScript : MonoBehaviour{

    //**********************
    // Panels
    //**********************

    [Header("Panels")]
    
    public GameObject alliesPanel;
    public GameObject teamOptionsPanel;
    public GameObject alliesAmountOfTanksPanel;
    public GameObject enemyPanel;
    public GameObject enemyAmountOfTanksPanel;
    
    
    //**********************
    // Sliders
    //**********************
    
    [Header("Sliders")]

    [Range(1, 3)]
    public int alliesAmountOfTanksSliderAmount = 3;
    
    [Range(1, 6)]
    public int enemyAmountOfTanksSliderAmount = 3;
    
    public Slider alliesAmountOfTanksSlider;
    public Slider enemyAmountOfTanksSlider;
    

    //**********************
    // Texts and Inputs
    //**********************

    [Header("Texts & Inputs")]
    
    public TMP_Text allyTankAmountText;
    public TMP_Text enemyTankAmountText;
    public TMP_Text teamNameText;

    public TMP_InputField teamNameInput; 
    
    //**********************
    // Images and Sprites
    //**********************
    
    [Header("Player Icon")]
    
    public GameObject playerIcon;
    
    private Image _playerIconEmblem;
    private Image _playerIconColor;

    private Sprite _emblem1;
    private Sprite _emblem2;
    private Sprite _emblem3;
    private Sprite _emblem4;
    
    
    //**********************
    // Animators
    //**********************
    
    private Animator _alliesPanelAnimator;
    private Animator _alliesAmountOfTanksPanelAnimator;
    private Animator _alliesTeamOptionsPanelAnimator;
    private Animator _enemyPanelAnimator;
    private Animator _enemyAmountOfTanksPanelAnimator;


    void Start()
    {
        // Set emblem images
        _emblem1 = UnityEngine.Resources.Load<Sprite>("Pictures/e1");
        _emblem2 = UnityEngine.Resources.Load<Sprite>("Pictures/e2");
        _emblem3 = UnityEngine.Resources.Load<Sprite>("Pictures/e5");
        _emblem4 = UnityEngine.Resources.Load<Sprite>("Pictures/e6");

        // Get emblem and color components of PlayerIcon
        _playerIconEmblem = playerIcon.transform.GetChild(1).GetComponent<Image>();
        _playerIconColor = playerIcon.transform.GetChild(0).GetComponent<Image>();

        // Get animators for panels
        _alliesPanelAnimator = alliesPanel.GetComponent<Animator>();
        _alliesAmountOfTanksPanelAnimator = alliesAmountOfTanksPanel.GetComponent<Animator>();
        _alliesTeamOptionsPanelAnimator = teamOptionsPanel.GetComponent<Animator>();
        _enemyPanelAnimator = enemyPanel.GetComponent<Animator>();
        _enemyAmountOfTanksPanelAnimator = enemyAmountOfTanksPanel.GetComponent<Animator>();
    }

    
    //****************************
    // On Value Changed Functions
    //****************************

    public void OnTeamNameChanged()
    {
        teamNameText.text = teamNameInput.text;
    }

    public void OnAlliesNumberOfTanksAmountChanged()
    {
        allyTankAmountText.text = alliesAmountOfTanksSlider.value.ToString(CultureInfo.CurrentCulture);
        DontDestroyOnLoadScript.instance.allyAmountOfTanks=(int)alliesAmountOfTanksSlider.value;
    }
    
    public void OnEnemyNumberOfTanksAmountChanged()
    {
        enemyTankAmountText.text = enemyAmountOfTanksSlider.value.ToString(CultureInfo.CurrentCulture);
        DontDestroyOnLoadScript.instance.enemyAmountOfTanks =(int) enemyAmountOfTanksSlider.value;
    }

    
    //*********************
    // OnClick Load Scenes
    //*********************
    
    public void BackToMainMenu(){
        SceneManager.LoadScene("Main Menu");
    }

    public void StartGame(){
        if (DontDestroyOnLoadScript.instance.selectedTanks[0] != -1 && DontDestroyOnLoadScript.instance.allyAmountOfTanks == 1)
        {
            SceneManager.LoadScene("MapTestScene");
        }
        else if (DontDestroyOnLoadScript.instance.selectedTanks[0] != -1 && DontDestroyOnLoadScript.instance.selectedTanks[1] != -1 && DontDestroyOnLoadScript.instance.allyAmountOfTanks == 2)
        {
            SceneManager.LoadScene("MapTestScene");
        }
        else if (DontDestroyOnLoadScript.instance.selectedTanks[0] != -1 && DontDestroyOnLoadScript.instance.selectedTanks[1] != -1 && DontDestroyOnLoadScript.instance.selectedTanks[2] != -1 && DontDestroyOnLoadScript.instance.allyAmountOfTanks == 3)
        {
            SceneManager.LoadScene("MapTestScene");
        }
        
    }
    
    public void TankSelection(){
        SceneManager.LoadScene("TankSelection");
    }

    
    //***********************
    // Choose Icon and Color
    //***********************
    
    public void ChooseColor(int c)
    {
        _playerIconColor.color = c switch
        {
            0 => new Color32(255, 0, 0, 255),
            1 => new Color32(18, 255, 18, 255),
            2 => new Color32(75, 128, 255, 255),
            3 => new Color32(255, 255, 0, 255),
            4 => new Color32(255, 125, 0, 255),
            5 => new Color32(255, 0, 255, 255),
            _ => new Color32(255, 255, 255, 255)
        };
        
        DontDestroyOnLoadScript.instance.selectedColor = c;
    }

    public void ChooseEmblem(int e)
    {
        _playerIconEmblem.sprite = e switch
        {
            0 => _emblem1,
            1 => _emblem2,
            2 => _emblem3,
            3 => _emblem4,
            _ => _emblem1
        };
    }
    
    
    //***************
    // Panel Toggles
    //***************
    
    public void ToggleAlliesPanel()
    {
        // Close Enemy panels
        HidePanel(_enemyPanelAnimator);
        HidePanel(_enemyAmountOfTanksPanelAnimator);
        
        // Close sub panels
        HidePanel(_alliesTeamOptionsPanelAnimator);
        HidePanel(_alliesAmountOfTanksPanelAnimator);

        // Toggle Allies panel
        TogglePanel(_alliesPanelAnimator);
    }

    public void ToggleAlliesAmountOfTanksPanel()
    {
        // Close Enemy panels
        HidePanel(_enemyPanelAnimator);
        HidePanel(_enemyAmountOfTanksPanelAnimator);
        
        // Close other ally panel
        HidePanel(_alliesTeamOptionsPanelAnimator);

        // Toggle AlliesAmountOfTanks panel
        TogglePanel(_alliesAmountOfTanksPanelAnimator);
    }
    
    public void ToggleTeamOptionsPanel()
    {
        // Close Enemy panels
        HidePanel(_enemyPanelAnimator);
        HidePanel(_enemyAmountOfTanksPanelAnimator);
        
        // Close other ally panel
        HidePanel(_alliesAmountOfTanksPanelAnimator);
        
        // Toggle AlliesTeamOptions panel
        TogglePanel(_alliesTeamOptionsPanelAnimator);
    }
    
    public void ToggleEnemyPanel()
    {
        // Close Ally panels
        HidePanel(_alliesPanelAnimator);
        HidePanel(_alliesTeamOptionsPanelAnimator);
        HidePanel(_alliesAmountOfTanksPanelAnimator);
        
        // Close other Enemy panel
        HidePanel(_enemyAmountOfTanksPanelAnimator);
        
        // Toggle Enemy panel
        TogglePanel(_enemyPanelAnimator);
    }

    public void ToggleEnemyAmountOfTanksPanel()
    {
        // Close Ally panels
        HidePanel(_alliesPanelAnimator);
        HidePanel(_alliesTeamOptionsPanelAnimator);
        HidePanel(_alliesAmountOfTanksPanelAnimator);
        
        // Toggle EnemyAmountOfTanks panel
        TogglePanel(_enemyAmountOfTanksPanelAnimator);
    }

    //******************************
    // TogglePanel Helper Functions
    //******************************
    
    private static void TogglePanel(Animator animator)
    {
        animator.SetBool("Show", !animator.GetBool("Show"));
    }

    private static void HidePanel(Animator animator)
    {
        animator.SetBool("Show", false);
    }
}
