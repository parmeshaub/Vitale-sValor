using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Services.Analytics;
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

    private MagicLockManager magicLockManager;
    [SerializeField] private List<DraggableSkill> draggableSkills;

    //[Header("Option Elements")]
    [SerializeField] private GameObject optionsUI;

    private PlayerInputManager playerInputManager;
    private PlayerInput playerInput;

    public bool isInOptions = false;

    private int pageCount = 0;

    private void Awake()
    {
        Instance = this;
        playerInputManager = PlayerInputManager.instance;;
        playerInput = playerInputManager.playerInput;

        playerInput.UI.Unpause.started += UnpauseCalled;
        playerInput.UI.NextLeftPage.started += NextLeftPageButton;
        playerInput.UI.NextRightPage.started += NextRightPageButton;

        playerInput.Gameplay.Pause.started += PauseGame;
        playerInput.Gameplay.MagicSelection.started += OpenMagicSelect;
        playerInput.Gameplay.Map.started += OpenMapUI;
        playerInput.Gameplay.ArmorPage.started += OpenArmorUI;
        playerInput.Gameplay.SkillTree.started += OpenSkillTree;
    }
    private void Start()
    { 
        magicLockManager = MagicLockManager.instance;
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

        UpdateSkillImage();

        //Activate Main UI Object.
        menuUIElements.SetActive(true);

        pauseMenuUI.SetActive(false);
        mapUI.SetActive(false);
        armourUI.SetActive(false);
        skillTreeUI.SetActive(false);
        optionsUI.SetActive(false);

        magicSelectUI.SetActive(true);

        pageCount = 1;
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
        pageCount = 2;
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
        pageCount = 4;
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
        pageCount = 3;
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

        pageCount = 0;
    }

    //public void 
    #endregion

    private void UpdateSkillImage() {
        foreach (var item in draggableSkills) {
            //Debug.Log("done" + item);
            if (item.magicMove.isUnlocked) {
                item.isUnlocked = true;
                item.uiDisplayImage.sprite = item.magicMove.icon;
            }
            else {
                item.isUnlocked = false;
                item.uiDisplayImage.sprite = item.magicMove.lockedIcon;
            }
        }
    }

    private void StartMenu() {
        //Debug.Log("Start");
        playerInputManager.SwitchToUIActionMap();
        Time.timeScale = 0.0f;
        gameplayUIElements.SetActive(false) ;
        optionsUI.SetActive(false);
        isInOptions = false;
    }

    public void EndMenu()
    {
        //Debug.Log("End");
        playerInputManager.SwitchToGameplayActionMap();
        Time.timeScale = 1.0f;
        gameplayUIElements.SetActive(true) ;
        menuUIElements.SetActive(false) ;
        pageCount = 0;
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
        pageCount = 0;
    }

    private void NextLeftPageButton(InputAction.CallbackContext context) {
        if (context.started) PreviousPage();
    }
    private void NextRightPageButton(InputAction.CallbackContext context) {
        if (context.started) NextPage();
    }

    public void NextPage() {
        if (isInOptions) return;
        pageCount++;
        if (pageCount > 4) pageCount = 0;

        SwitchPage();
    }
    public void PreviousPage() {
        if (isInOptions) return;
        pageCount--;
        if (pageCount < 0) pageCount = 4;

        SwitchPage();
    }

    private void SwitchPage() {
        // Deactivate all UI elements first
        pauseMenuUI.SetActive(false);
        magicSelectUI.SetActive(false);
        armourUI.SetActive(false);
        skillTreeUI.SetActive(false);
        optionsUI.SetActive(false);
        mapUI.SetActive(false);

        // Activate the correct UI based on the pageCount
        switch (pageCount) {
            case 0:
                pauseMenuUI.SetActive(true);
                break;
            case 1:
                UpdateSkillImage();
                magicSelectUI.SetActive(true);
                break;
            case 2:
                mapUI.SetActive(true);
                break;
            case 3:
                skillTreeUI.SetActive(true);
                break;
            case 4:
                armourUI.SetActive(true);
                break;
            default:
                // Optional: Handle invalid pageCount values or log an error
                Debug.LogWarning("Invalid pageCount value: " + pageCount);
                break;
        }
    }

}
