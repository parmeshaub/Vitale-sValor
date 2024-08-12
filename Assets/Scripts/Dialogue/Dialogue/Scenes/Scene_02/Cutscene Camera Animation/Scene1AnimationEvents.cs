using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1AnimationEvents : MonoBehaviour
{
    public BoxCollider blocking;

    private void OnTriggerEnter(Collider other) {
        StartCoroutine(WaitforCollider());
    }
    private IEnumerator WaitforCollider() {
        yield return new WaitForSeconds(55);
        blocking.gameObject.SetActive(false);
    }
}
