using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDungeonCutsceneCameraEvents : MonoBehaviour
{
    [SerializeField] private DialogueSO dialogue1;
    [SerializeField] private DialogueSO dialogue2;
    [SerializeField] private DialogueSO dialogue3;
    [SerializeField] private DialogueSO dialogue4;
    [SerializeField] private TutorialManager tutorialManager;



    public void PlayDialogue1() {
        CinematicDialogueManager.instance.InitiateCinematicDialogue(dialogue1);
    }

    public void PlayDialogue2() {
        CinematicDialogueManager.instance.InitiateCinematicDialogue(dialogue2);
    }

    public void AvidalDialogue1() {
        CinematicDialogueManager.instance.InitiateCinematicDialogue(dialogue3);

    }
    public void AvidalDialogue2() {
        CinematicDialogueManager.instance.InitiateCinematicDialogue(dialogue4);
    }

    public void ActivateTutorial() {
        tutorialManager.ActivateTutorial();
    }

    public void ExitWorld() {
        tutorialManager.ExitWorld();
    }
}
