using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Gameplay Elements")]
    [SerializeField] private GameObject gameplayUIElements;

    [Header("Menu Elements")]
    [SerializeField] private GameObject menuUIElements;
    [SerializeField] private GameObject magicSelectUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject mapUI;
    [SerializeField] private GameObject armourUI;
    [SerializeField] private GameObject skillTreeUI;
    //[SerializeField] private GameObject runeUI;

    //[Header("Option Elements")]
    [SerializeField] private GameObject optionsUI;

    private PlayerInputManager playerInputManager;
    private PlayerInput playerInput;

    public bool isInOptions = false;

    private void Awake()
    {
        Instance = this;
        playerInputManager = PlayerInputManager.instance;;
        playerInput = playerInputManager.playerInput;

        playerInput.UI.Unpause.started += UnpauseCalled;

        playerInput.Gameplay.Pause.started += PauseGame;
        playerInput.Gameplay.MagicSelection.started += OpenMagicSelect;
        playerInput.Gameplay.Map.started += OpenMapUI;
        playerInput.Gameplay.ArmorPage.started += OpenArmorUI;
        playerInput.Gameplay.SkillTree.started += OpenSkillTree;
    }
    private void Start()
    {
        InitializeUIForGameplay();
    }
    private void InitializeUIForGameplay()
    {
        gameplayUIElements.SetActive(true);
        menuUIElements.SetActive(false);
        optionsUI.SetActive(false);
    }

    #region Menu Methods
    public void PauseGame(InputAction.CallbackContext context){
        if (!context.started) return;
        StartMenu();

        //Activate Main UI Object.
        menuUIElements.SetActive(true);

        //Set Every Other Object as False.
        magicSelectUI.SetActive(false);
        mapUI.SetActive(false);
        armourUI.SetActive(false);
        skillTreeUI.SetActive(false);
        optionsUI.SetActive(false);

        //Set Pause Menu as True.
        pauseMenuUI.SetActive(true);
    }
    public void OpenMagicSelect(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        StartMenu();

        //Activate Main UI Object.
        menuUIElements.SetActive(true);

        pauseMenuUI.SetActive(false);
        mapUI.SetActive(false);
        armourUI.SetActive(false);
        skillTreeUI.SetActive(false);
        optionsUI.SetActive(false);

        magicSelectUI.SetActive(true);
    }

    public void OpenMapUI(InputAction.CallbackContext context){
        if(!context.started) return;

        StartMenu();

        //Activate Main UI Object.
        menuUIElements.SetActive(true);

        pauseMenuUI.SetActive(false);
        magicSelectUI.SetActive(false);
        armourUI.SetActive(false);
        skillTreeUI.SetActive(false);
        optionsUI.SetActive(false);

        mapUI.SetActive(true);
    }

    public void OpenArmorUI(InputAction.CallbackContext context) { 
        if(!context.started) return;

        StartMenu();

        //Activate Main UI Object.
        menuUIElements.SetActive(true);

        optionsUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        mapUI.SetActive(false);
        magicSelectUI.SetActive(false);
        skillTreeUI.SetActive(false);

        armourUI.SetActive(true);
    }

    public void OpenSkillTree(InputAction.CallbackContext context) {
        if(context.started) return;
        StartMenu();

        //Activate Main UI Object.
        menuUIElements.SetActive(true);

        optionsUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        mapUI.SetActive(false);
        magicSelectUI.SetActive(false);
        armourUI.SetActive(false);

        skillTreeUI.SetActive(true);
    }

    public void PauseGame2() {
        StartMenu();

        //Activate Main UI Object.
        menuUIElements.SetActive(true);

        //Set Every Other Object as False.
        magicSelectUI.SetActive(false);
        mapUI.SetActive(false);
        armourUI.SetActive(false);
        skillTreeUI.SetActive(false);

        //Set Pause Menu as True.
        pauseMenuUI.SetActive(true);
    }

    //public void 
    #endregion

    private void StartMenu() {
        Debug.Log("Start");
        playerInputManager.SwitchToUIActionMap();
        Time.timeScale = 0.0f;
        gameplayUIElements.SetActive(false) ;
        optionsUI.SetActive(false);
        isInOptions = false;
    }

    public void EndMenu()
    {
        Debug.Log("End");
        playerInputManager.SwitchToGameplayActionMap();
        Time.timeScale = 1.0f;
        gameplayUIElements.SetActive(true) ;
        menuUIElements.SetActive(false) ;
    }

    private void UnpauseCalled(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(isInOptions) {
                PauseGame2();
            }
            else {
                EndMenu();
            }
        }
    }

    public void SetOptionsBoolTrue() {
        isInOptions = true;
    }
}
