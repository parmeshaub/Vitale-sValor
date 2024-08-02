using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractable : Interactable
{
    private DialogueManager dialogueManager;
    [SerializeField] private DialogueSO dialogue;

    private void Start()
    {
        dialogueManager = DialogueManager.instance;
    }
    public override void Interact()
    {
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager instance is not set.");
            return; // Exit early to avoid further errors
        }

        if (dialogue == null)
        {
            Debug.LogError("DialogueSO is not assigned.");
            return; // Exit early to avoid further errors
        }

        dialogueManager.InitiateDialogue(dialogue);
    }

}
