using System.Collections;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    private GameObject shieldObject;
    private Vector3 shieldOutScale = new Vector3(1, 1, 0.53f);
    private Vector3 shieldInScale = new Vector3(0.001f, 0.001f, 0.001f);
    private float transitionDuration = 0.13f; // Duration for the lerp transition
    public BoxCollider shieldAttackCollider;

    private void Start() {
        shieldObject = gameObject;
        shieldObject.transform.localScale = shieldInScale;
        shieldAttackCollider.enabled = false;
    }

    public void TakeOutShield() {
        shieldObject.SetActive(true);
        if (shieldObject.transform.localScale == shieldOutScale) return;
        StopAllCoroutines(); // Stop any ongoing scaling coroutine
        StartCoroutine(ScaleShield(shieldOutScale));
    }

    public void TakeInShield() {
        StopAllCoroutines(); // Stop any ongoing scaling coroutine
        StartCoroutine(ScaleShield(shieldInScale));
    }

    private IEnumerator ScaleShield(Vector3 targetScale) {
        Vector3 initialScale = shieldObject.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration) {
            shieldObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        shieldObject.transform.localScale = targetScale; // Ensure the final scale is set
    }

    private IEnumerator HideShield() {
        yield return new WaitForSeconds(0.1f);
        shieldObject.SetActive(false);
    }

    public void EnableShieldAttackCollider() {
        shieldAttackCollider.enabled = true;
    }
    public void DisableShieldAttackCollider() {
        shieldAttackCollider.enabled = false;
    }
}
