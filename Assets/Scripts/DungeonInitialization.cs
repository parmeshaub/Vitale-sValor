using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInitialization : MonoBehaviour
{
    [SerializeField] private Transform playerSpawnPoint;
    private GameObject playerObject;
    private CharacterController characterController;
    private void Awake() {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        characterController = playerObject.GetComponent<CharacterController>();

    }
    public void SetPlayerLocation() {
        characterController.enabled = false;
        playerObject.transform.position = playerSpawnPoint.position;
        playerObject.transform.rotation = playerSpawnPoint.rotation;
        FadeManager.instance.StartFadeOut();
        characterController.enabled = true;
    }
}
