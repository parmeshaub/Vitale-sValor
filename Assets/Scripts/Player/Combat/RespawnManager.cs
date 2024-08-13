using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's Transform
    public Transform defaultRespawnPoint;
    private Transform respawnPoint;
    private List<GameObject> pillarList = new List<GameObject>();
    private PlayerHealthAndDamage playerHealth;
    [SerializeField] private FadeManager fadeManager;
    private Animator animator;
    private PlayerInputManager playerInputManager;
    private PlayerController playerController;

    private void Start() {
        playerHealth = GetComponent<PlayerHealthAndDamage>();
        animator = GetComponentInChildren<Animator>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerController = GetComponent<PlayerController>();
    }

    private void GetClosestRespawnPillar() {
        // Clear the list of pillars each time the method is called
        pillarList.Clear();

        // Find all GameObjects with the tag "ArtemisPillar"
        GameObject[] allPillars = GameObject.FindGameObjectsWithTag("ArtemisPillar");

        // Filter the unlocked pillars
        foreach (GameObject pillar in allPillars) {
            ArtemisPillar pillarScript = pillar.GetComponent<ArtemisPillar>();
            if (pillarScript != null && pillarScript.isUnlocked) {
                pillarList.Add(pillar);
            }
        }

        // If there are no unlocked pillars, handle the case (e.g., default to a specific point)
        if (pillarList.Count == 0) {
            Debug.LogWarning("No unlocked pillars found.");
            return;
        }

        // Find the closest unlocked pillar
        GameObject closestPillar = pillarList
            .OrderBy(pillar => Vector3.Distance(playerTransform.position, pillar.transform.position))
            .FirstOrDefault();

        // Set the closest pillar as the respawn point
        if (closestPillar != null) {
            ArtemisPillar close = closestPillar.GetComponent<ArtemisPillar>();
            respawnPoint = close.respawnPoint;
            Debug.Log("Closest respawn point set to: " + closestPillar.name);
        }
        else {
            Debug.LogWarning("No closest pillar found.");
        }
    }

    public void RespawnPlayer() {
        StartCoroutine(RespawnPlayerCoroutine());
    }

    private IEnumerator RespawnPlayerCoroutine() {
        playerController.isAttacking = true;

        // Disable CharacterController to avoid interference with setting position
        CharacterController characterController = GetComponent<CharacterController>();
        if (characterController != null) {
            characterController.enabled = false;
        }

        // Find the closest respawn pillar
        GetClosestRespawnPillar();

        // Use defaultRespawnPoint if respawnPoint is null
        if (respawnPoint == null) {
            GameObject defaultResPointObject = GameObject.FindGameObjectWithTag("DefaultRespawn");
            defaultRespawnPoint = defaultResPointObject.transform;
            respawnPoint = defaultRespawnPoint;
            Debug.LogWarning("Respawn point was null. Using default respawn point.");
        }

        // Ensure the respawn point is valid before moving the player
        if (respawnPoint != null) {
            this.transform.position = respawnPoint.position;
            this.transform.rotation = respawnPoint.rotation;
            Debug.Log("Player moved to respawn point: " + respawnPoint.position);
        }
        else {
            Debug.LogError("Both respawn point and default respawn point are null. Unable to respawn player.");
        }

        // Re-enable CharacterController after moving the player
        if (characterController != null) {
            characterController.enabled = true;
        }
        yield return new WaitForSeconds(1);

        playerController.isAttacking = false;
        animator.SetTrigger("Respawn");
        playerHealth.InitializeHealth();
        playerInputManager.playerInput.Gameplay.Enable();
        fadeManager.StartFadeOut(2f);

        yield return null;
    }
}
