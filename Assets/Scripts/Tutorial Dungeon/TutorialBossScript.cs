using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialBossScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PlayerController playerController;
    private PlayerCombat playerCombat;
    private GameObject player;
    [SerializeField] private GameObject PromptUI;
    [SerializeField] private TMP_Text text;
    [SerializeField] TutorialManager tutorialManager;

    private int currentPhase = 0;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerCombat = player.GetComponent<PlayerCombat>();

        StartCoroutine(BossAttackSequence());
    }

    private IEnumerator BossAttackSequence() {
        yield return new WaitForSeconds(3f); // Initial delay before the first attack
        StartPhaseOne();
    }

    private void StartPhaseOne() {
        currentPhase = 1;

        ShowPrompt("The boss is about to attack! Get ready to dash!");

        // Rotate the boss to look at the player
        RotateBossToPlayer(() => {
            // Trigger the first boss attack (Big undodgable projectile)
            animator.SetTrigger("ArrowProjectile");

            // Wait for the projectile to reach mid-point
            StartCoroutine(HandlePhaseOneMidPoint());
        });
    }

    private void RotateBossToPlayer(TweenCallback onComplete) {
        Vector3 playerPosition = player.transform.position;
        transform.DOLookAt(playerPosition, 1f).OnComplete(onComplete); // Rotate to look at the player over 1 second
    }

    private IEnumerator HandlePhaseOneMidPoint() {
        yield return new WaitForSeconds(0.5f);

        // Slow down time and prompt player to dash
        Time.timeScale = 0.5f;
        playerController.isAttacking = true;
        ShowPrompt("Dash to avoid the attack! 'E'");

        // Wait for the player to dash
        yield return StartCoroutine(WaitForPlayerToDash());

        // Restore time and movement
        Time.timeScale = 1f;
        playerController.isAttacking = false;
        HidePrompt();

        // Wait before starting the next phase
        yield return new WaitForSeconds(2f);
        StartPhaseTwo();
    }

    private void StartPhaseTwo() {
        currentPhase = 2;

        ShowPrompt("Prepare for another attack! Dash again to survive! 'E' " );

        RotateBossToPlayer(() => {
            // Trigger the second boss attack (Same projectile as before)
            animator.SetTrigger("ArrowProjectile");

            StartCoroutine(HandlePhaseTwoMidPoint());
        });
    }

    private IEnumerator HandlePhaseTwoMidPoint() {
        yield return new WaitForSeconds(0.5f);

        // Slow down time and prompt player to dash
        Time.timeScale = 0.5f;
        ShowPrompt("Dash to avoid the attack!");

        yield return StartCoroutine(WaitForPlayerToDash());

        // Restore time and movement
        Time.timeScale = 1f;
        HidePrompt();

        // Wait before starting the next phase
        yield return new WaitForSeconds(2f);
        StartPhaseFour();
    }

    private void StartPhaseFour() {
        currentPhase = 3;

        ShowPrompt("The boss is charging up a beam attack! Prepare to block!");

        RotateBossToPlayer(() => {
            // Trigger the fourth boss attack (Beam attack)
            animator.SetTrigger("BeamAttack");

            // Freeze player movement and prompt to block
            StartCoroutine(WaitForBeamToHit());
        });
    }

    private IEnumerator WaitForBeamToHit() {
        yield return new WaitForSeconds(1f); // Adjust based on beam timing

        // Slow down time, prompt to block
        Time.timeScale = 0.5f;
        ShowPrompt("Block the incoming attack! 'Q' ");

        yield return StartCoroutine(WaitForPlayerToBlock());

        // Restore time and movement
        Time.timeScale = 1f;
        HidePrompt();

        // End of tutorial
        StartCoroutine(EndTutorial());
    }

    private void ShowPrompt(string message) {
        PromptUI.SetActive(true);
        text.text = message;
    }

    private void HidePrompt() {
        PromptUI.SetActive(false);
        text.text = "";
    }

    private IEnumerator WaitForPlayerToDash() {
        while (!playerController.isDashing) // Assuming you track when the player dashes
        {
            yield return null;
        }
    }

    private IEnumerator WaitForPlayerToBlock() {
        while (!playerCombat.isBlocking) // Assuming you track when the player blocks
        {
            yield return null;
        }
    }

    private IEnumerator EndTutorial() {
        ShowPrompt("Tutorial Complete!");
        yield return new WaitForSeconds(2f);
        HidePrompt();
        tutorialManager.EndTutorial();
        // Additional logic to end the tutorial or start the actual boss fight
    }
}
