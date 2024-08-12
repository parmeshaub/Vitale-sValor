using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCameraOnTrigger : MonoBehaviour
{
    public GameObject Camera;
    public BoxCollider boxcollider;
    [SerializeField] private float timeToTurnOff;
    private void OnTriggerEnter(Collider other) {
        Camera.gameObject.SetActive(true);
        boxcollider.enabled = false;
        StartCoroutine(TurnOffCamera());
    }

    private IEnumerator TurnOffCamera() {
        yield return new WaitForSeconds(timeToTurnOff);
        Camera.gameObject.SetActive(false);
    }
}
