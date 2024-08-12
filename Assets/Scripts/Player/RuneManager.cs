using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RuneManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    private PlayerInput playerInput;
    private bool runeOpen = false;
    [SerializeField] private CurrentWorld worldEnum;
    [SerializeField] private GameObject runeUIObject;
    [SerializeField] private CanvasGroup runeCanvasGroup;
    private bool isRuneOnCoolDown = false;
    [SerializeField] private float cooldownSeconds;
    [SerializeField] private GameObject coolDownText;
    private Coroutine coolDownCoroutine;
    Interactor interactorScript ; // Finding interactor script from main terrain mesh

    private float fadeInTime = 0.1f;
    private float fadeOutTime = 0.1f;

    private void Awake()
    {
        playerInputManager = PlayerInputManager.instance;
        playerInput = playerInputManager.playerInput;

        playerInput.Gameplay.ActivateRune.started += OpenRuneMenu;
        playerInput.UI.Rune.started += OpenRuneMenu;       
    }

    private void Start() {
        //Initialize the UI.
        worldEnum = CurrentWorld.Flora; //Assuming we start in Flora, idk change it later
        runeCanvasGroup.alpha = 0; // makes sure the alpha is invisible.
        runeUIObject.SetActive(false);
        coolDownText.SetActive(false);


        GameObject mainWorldMesh = GameObject.Find("MainworldMesh");
        interactorScript = mainWorldMesh.GetComponent<Interactor>();
    }

    private void OpenRuneMenu(InputAction.CallbackContext context) {
        if (!context.started) return;

        runeOpen = !runeOpen;

        if(runeOpen) { // Rune Opened
            runeUIObject.SetActive(true);
            StartCoroutine(FadeIn(runeCanvasGroup));
            playerInputManager.SwitchToUIActionMapKeepCamera();
            Time.timeScale = 1f; //Might want to make slower.
        }
        else { // Rune Closed
            StartCoroutine(FadeOut(runeCanvasGroup));
            playerInputManager.SwitchToGameplayActionMapFixCamera();
            Time.timeScale = 1;
        }

    }


    //JAQ PLS PLACE HERE----------------------------------------------------------------------------------------------------------------------
    public void ChangeToFlora() {
        if (isRuneOnCoolDown  || coolDownCoroutine != null) return;
        //Check if already in Flora.
        if (worldEnum == CurrentWorld.Flora) return;
        worldEnum = CurrentWorld.Flora;
        interactorScript.GoingFlora();
        coolDownCoroutine = StartCoroutine(StartCoolDown());

    }

    public void ChangeToFyre() {
        if (isRuneOnCoolDown || coolDownCoroutine != null) return;
        //Check if already in Fyre.
        if (worldEnum == CurrentWorld.Fyre) return;
        worldEnum = CurrentWorld.Fyre;
        interactorScript.GoingFlurry();
        coolDownCoroutine = StartCoroutine(StartCoolDown());

    }

    public void ChangeToFlurry() {
        if (isRuneOnCoolDown || coolDownCoroutine != null) return;
        //Check if already in Flurry.
        if (worldEnum == CurrentWorld.Flurry) return;
        worldEnum = CurrentWorld.Flurry;
        interactorScript.GoingFyre();
        coolDownCoroutine = StartCoroutine(StartCoolDown());

    }
    //JAQ PLS PLACE HERE----------------------------------------------------------------------------------------------------------------------

    private IEnumerator FadeIn(CanvasGroup canvasGroup) {
        canvasGroup.alpha = 0;
        float elapsedTime = 0f;
        while (elapsedTime < fadeInTime) {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeInTime);
            yield return null;
        }
        canvasGroup.alpha = 1f; // ensure it's fully opaque at the end
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup) {
        canvasGroup.alpha = 1;
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutTime) {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeOutTime));
            yield return null;
        }
        canvasGroup.alpha = 0f; // ensure its fully transparent at the end

        runeUIObject.SetActive(false);
    }

    private IEnumerator StartCoolDown() {
        isRuneOnCoolDown = true;
        coolDownText.SetActive(true);
        yield return new WaitForSeconds(cooldownSeconds);
        isRuneOnCoolDown = false;
        coolDownText.SetActive(false);
        coolDownCoroutine = null;
    }
}

public enum CurrentWorld
{
    Flora,
    Fyre,
    Flurry
}
