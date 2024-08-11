using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorilPillarScript : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> pillarParts;
    [SerializeField] private float scaleDownDuration = 2f; // Duration of the scaling down effect

    private void Start() {
        foreach (var part in pillarParts) {
            part.isKinematic = true;
        }
    }

    public void PillarHit(Vector3 directionHit) {
        foreach (var part in pillarParts) {
            part.isKinematic = false;

            // Ensure the direction is normalized
            Vector3 normalizedDirection = directionHit.normalized;

            // Apply an impulse force to each part in the direction of the hit
            part.AddForce(normalizedDirection * 10f, ForceMode.Impulse); // Adjust the force multiplier (10f) as needed

            // Start a coroutine for each part to scale it down and destroy it
            StartCoroutine(StartDestroy(part));
        }
    }

    private IEnumerator StartDestroy(Rigidbody part) {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Scale down the pillar part over the duration
        float elapsedTime = 0f;
        Vector3 originalScale = part.transform.localScale;

        while (elapsedTime < scaleDownDuration) {
            float scaleProgress = elapsedTime / scaleDownDuration;
            part.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, scaleProgress);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the scale is set to zero at the end
        part.transform.localScale = Vector3.zero;

        // Destroy the individual part
        Destroy(part.gameObject);
    }
}
