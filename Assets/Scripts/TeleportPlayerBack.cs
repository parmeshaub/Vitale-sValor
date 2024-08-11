using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerBack : MonoBehaviour
{
    [SerializeField] private Transform respawnPlace;
    private void OnTriggerEnter(Collider other) {
        GameObject player = other.gameObject;
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        FadeManager.instance.StartFadeIn();
        player.transform.position = respawnPlace.transform.position;
        player.transform.rotation = respawnPlace.transform.rotation;
        FadeManager.instance.StartFadeOut();
        controller.enabled = true;
    }
}
