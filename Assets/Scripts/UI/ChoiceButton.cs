using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceButton : MonoBehaviour
{
    [SerializeField] private TMP_Text choiceButtonText;
    private DialogueSO nextDialogue;
    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = DialogueManager.instance;
    }
    public void SetText(string dialogue)
    {
        choiceButtonText.text = dialogue;
    }

    public void SetNextDialogue(DialogueSO dialogue)
    {
        nextDialogue = dialogue;
    }

    public void Interacted()
    {
        dialogueManager.InitiateDialogue(nextDialogue);
        dialogueManager.ClearChoices();
    }
}
