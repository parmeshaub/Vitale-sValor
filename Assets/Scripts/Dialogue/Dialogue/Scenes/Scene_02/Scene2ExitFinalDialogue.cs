using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2ExitFinalDialogue : MonoBehaviour
{
    [SerializeField] private GameObject toDestroy;
    private void OnTriggerExit(Collider other) {
        Destroy(toDestroy);
    }
}
