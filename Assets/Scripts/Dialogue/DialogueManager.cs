using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private DialogueSO currentDialogue;
    private PlayerInputManager playerInputManager;
    private PlayerInput playerInput;
    private CameraManager cameraManager;

    //UI Elements
    [SerializeField] private TMP_Text speakerName;
    [SerializeField] private TMP_Text speakerRole;
    [SerializeField] private GameObject dialogueArrow;
    [SerializeField] private GameObject dialogueObject; // the Entire Dialogue Object
    [SerializeField] private TMP_Text dialogueText; // Text mesh
    [SerializeField] private GameObject choiceGroup;
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private CanvasGroup dialogueCanvasGroup;

    //Other Dialogue.
    [SerializeField] private GameObject cinematicDialogueObject;
    [SerializeField] private CanvasGroup cinematicDialogueGroup;
    [SerializeField] private TMP_Text cinematicDialogue;

    //Variables.
    [SerializeField] private float fadeInTime = 0.1f;
    [SerializeField] private float fadeOutTime = 0.1f;
    [SerializeField] private float typingSpeed = 0.05f;

    private bool proceedDialogue = false; // used to exit dialogue completed.
    private bool canSkipDialogue = false; // used to skip dialogue. (ONLY WHILE PARSING).
    private bool inDialogue = false;
    private bool skipDialogue = false;
    private bool choosingChoice = false;
    private bool lockEnterDialogue = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }

        cameraManager = CameraManager.instance;
        playerInputManager = PlayerInputManager.instance;
        playerInput = playerInputManager.playerInput;

        playerInput.Dialogue.ProceedDialogue.started += PressToProceedDialogue;
    }

    private void Start()
    {
        dialogueArrow.SetActive(false);
        //dialogueObject.SetActive(false);
        dialogueCanvasGroup.alpha = 0;

        cinematicDialogueGroup.alpha = 0;
        cinematicDialogueObject.SetActive(false);
    }

    public void InitiateDialogue(DialogueSO dialogue)
    {
        //Only enter if lock enter is false.
        if (!lockEnterDialogue)
        {
            //Check Dialogue Type.
            if (dialogue.dialogueType == DialogueType.Normal)
            {
                Debug.Log("Normal Dialogue");
                //turn on dialogue object
                //dialogueObject.SetActive(true);
                dialogueArrow.SetActive(false);
                if (!inDialogue)
                {
                    StartCoroutine(FadeIn(dialogueCanvasGroup));
                }

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                playerInputManager.SwitchToDialogueActionMap();

                // Set initial states.
                inDialogue = true;
                proceedDialogue = false;
                canSkipDialogue = false;
                choosingChoice = false;


                //Sets the dialogue thats passed in.
                currentDialogue = dialogue;

                //Initialize Values
                speakerName.text = currentDialogue.speakerName;
                speakerRole.text = currentDialogue.speakerRole;

                //Make sure that dialogue text is empty.
                dialogueText.text = "";

                //Play animation here.

                //Play Audio Here.

                //Start Parsing words.
                StartCoroutine(StartParsingDialogue(currentDialogue.dialogueText));
            }

            //Cinematic Dialogue, just appear on the screen. Player can move.
            else if (dialogue.dialogueType == DialogueType.Cinematic)
            {
                lockEnterDialogue = true;
                //start the cinematic dialogue.
                cinematicDialogueObject.SetActive(true);
                cinematicDialogueGroup.alpha = 1;
                cinematicDialogue.text = dialogue.dialogueText;

                StartCoroutine(WaitUntilNextDialogue(dialogue));
            }
        }
    }

    #region Normal Dialogue
    private IEnumerator StartParsingDialogue(string originalDialogueText)
    {
        Debug.Log("Parsing Now");
        canSkipDialogue = true;

        dialogueText.text = "";

        for (int i = 0; i < originalDialogueText.Length; i++)
        {
            Debug.Log("Parse");
            dialogueText.text += originalDialogueText[i];

            if(skipDialogue)
            {
                dialogueText.text = originalDialogueText; 
                break;
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        // Once Dialog Completed.
        DialogueCompleted();
    }


    private void DialogueCompleted() // Has to wait in this state. Turn on the dialogue arrow.
    {
        Debug.Log("Completed");
        canSkipDialogue = false;
        skipDialogue = false;

        dialogueArrow.SetActive(true);

        //Check if there are choices.
        if (currentDialogue.choices.Count > 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Clear existing choices to avoid duplication
            foreach (Transform child in choiceGroup.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            for (int i = 0; i < currentDialogue.choices.Count; i++)
            {
                // Instantiate choice button
                GameObject choiceButton = Instantiate(choiceButtonPrefab, choiceGroup.transform);
                ChoiceButton choiceButtonScript  = choiceButton.GetComponent<ChoiceButton>();
                choiceButtonScript.SetText(currentDialogue.choices[i].choiceDialogue);
                choiceButtonScript.SetNextDialogue(currentDialogue.choices[i].nextDialogue);
            }
        }
        //There are no choices.
        else
        {
            Debug.Log("There are no choices.");
            proceedDialogue = true;
        }
        
    }

    private void EndDialogue()
    {
        //if theres a next dialogue
        if(currentDialogue.nextDialogue != null)
        {
            inDialogue = true;
        }
        else
        {
            inDialogue = false;

            //Turn off dialogue box.
            speakerName.text = "";
            speakerRole.text = "";
            dialogueText.text = "";

            StartCoroutine(FadeOut(dialogueCanvasGroup));

            playerInputManager.SwitchToGameplayActionMap();
        }
    }

    private void PressToProceedDialogue(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if (choosingChoice) return;

            //Turns boolean on to skip dialogue.
            if (canSkipDialogue)
            {
                skipDialogue = true;
            }

            if (proceedDialogue)
            {
                //check if there is another dialogue available.
                if (currentDialogue.nextDialogue != null)
                {
                    inDialogue = true;
                    //Play the next dialogue.
                    InitiateDialogue(currentDialogue.nextDialogue);
                }
                // No More Dialog. End Dialogue.
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    public void ClearChoices()
    {
        foreach (Transform child in choiceGroup.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    #endregion
    #region Cinematic Dialogue

    private IEnumerator WaitUntilNextDialogue(DialogueSO dialogue)
    {
        yield return new WaitForSeconds(dialogue.dialogueExitTime);

        if (dialogue.nextDialogue != null)
        {
            lockEnterDialogue = false;
            InitiateDialogue(dialogue.nextDialogue);
        }
        else
        {
            CompleteCinematicDialogue();
        }
    }

    private void CompleteCinematicDialogue()
    {
        lockEnterDialogue = false;
        cinematicDialogue.text = "";
        FadeOut(cinematicDialogueGroup);
        cinematicDialogueObject.SetActive(false);
    }
    #endregion

    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeInTime);
            yield return null;
        }
        canvasGroup.alpha = 1f; // ensure it's fully opaque at the end
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeOutTime));
            yield return null;
        }
        canvasGroup.alpha = 0f; // ensure its fully transparent at the end
        //dialogueObject.SetActive(false);
    }

}