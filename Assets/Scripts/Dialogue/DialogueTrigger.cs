using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager dialogueManager;
    [SerializeField] private DialogueSO dialogue;
    [SerializeField] private BoxCollider boxCollider;
    private void OnTriggerEnter(Collider other) {
        dialogueManager = DialogueManager.instance;
        dialogueManager.InitiateDialogue(dialogue);
        boxCollider.enabled = false;
    }
}
