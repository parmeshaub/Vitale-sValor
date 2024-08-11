using UnityEngine;
using UnityEngine.Events;

public class EnemyChecker : MonoBehaviour
{
    // UnityEvent that will be invoked when there are no more enemies left
    public UnityEvent OnAllEnemiesDefeated;
    private bool doOnce = false;
    [SerializeField] private DialogueSO dialogue;

    // Update is called once per frame
    void Update() {
        // Find all objects with the tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // If no enemies are found, invoke the event
        if (enemies.Length == 0) {
            if(!doOnce) {
                OnAllEnemiesDefeated.Invoke();
                doOnce = true;
                //dialogue
                DialogueManager.instance.InitiateDialogue(dialogue);
            }
        }
    }
}
