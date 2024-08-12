using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CinematicDialogueManager : MonoBehaviour
{
    public static CinematicDialogueManager instance;

    private DialogueSO currentDialogue;
    [SerializeField] private SoundManager soundManager;

    // Cinematic Dialogue UI Elements
    [SerializeField] private GameObject cinematicDialogueObject;
    [SerializeField] private CanvasGroup cinematicDialogueGroup;
    [SerializeField] private TMP_Text cinematicDialogue;

    // Variables
    [SerializeField] private float fadeInTime = 0.1f;
    [SerializeField] private float fadeOutTime = 0.1f;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject); // Ensure only one instance exists
        }

        cinematicDialogueGroup.alpha = 0;
        cinematicDialogueObject.SetActive(false);
    }

    public void InitiateCinematicDialogue(DialogueSO dialogue) {
        // Start the cinematic dialogue.
        currentDialogue = dialogue;

        cinematicDialogueObject.SetActive(true);
        cinematicDialogueGroup.alpha = 1;
        cinematicDialogue.text = dialogue.dialogueText;

        soundManager.StopDialogue();
        soundManager.PlayDialogue(dialogue.audioClip);

        StartCoroutine(WaitUntilNextDialogue(dialogue));
    }

    private IEnumerator WaitUntilNextDialogue(DialogueSO dialogue) {
        yield return new WaitForSeconds(dialogue.dialogueExitTime);

        if (dialogue.nextDialogue != null) {
            InitiateCinematicDialogue(dialogue.nextDialogue);
        }
        else {
            CompleteCinematicDialogue();
        }
    }

    private void CompleteCinematicDialogue() {
        soundManager.StopDialogue();
        cinematicDialogue.text = "";
        StartCoroutine(FadeOut(cinematicDialogueGroup));
        cinematicDialogueObject.SetActive(false);
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup) {
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutTime) {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeOutTime));
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
