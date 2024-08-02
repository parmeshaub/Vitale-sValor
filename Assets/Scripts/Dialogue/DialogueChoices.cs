using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueChoices
{
    [TextArea(4, 4)]
    public string choiceDialogue;
    public DialogueSO nextDialogue;
}
