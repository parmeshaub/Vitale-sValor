using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingDialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueSO firstDialogue;
    private void Start() {
        StartCoroutine(WaitBeforeStartingDialogue());
    }

    private IEnumerator WaitBeforeStartingDialogue() {
        //FadeManager.instance.fadeCanvas.alpha = 1.0f;
        //FadeManager.instance.StartFadeOut(4);
        yield return new WaitForSeconds(2);
        DialogueManager.instance.InitiateDialogue(firstDialogue);
        //Destroy(gameObject);
    }
}
