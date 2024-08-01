using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarScript : MonoBehaviour
{
    [SerializeField] private GameObject breakablePillar;
    [SerializeField] private GameObject pillar;
    [SerializeField] private List<Rigidbody> breakablePillarRBs; // List of Rigidbody components
    [SerializeField] private float explosionForce = 1000f; // Adjust this value as needed
    [SerializeField] private float destructionDelay = 5f; // Delay before the pillar is destroyed
    [SerializeField] private float shrinkDuration = 2f; // Duration of the shrink effect

    public IcebearScript icebear;

    public delegate void IceBearPillarDamage();
    public static event IceBearPillarDamage OnIceBearPillarHit;

    private void Start()
    {
        breakablePillar.SetActive(false);
        pillar.SetActive(true);

        // Ensure all Rigidbody components in breakablePillar are added to the list
        breakablePillarRBs = new List<Rigidbody>(breakablePillar.GetComponentsInChildren<Rigidbody>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            if (icebear.isCharging)
            {
                Rigidbody otherRb = other.GetComponent<Rigidbody>();
                if (otherRb != null)
                {
                    // Get the velocity and direction of the collider
                    Vector3 velocity = otherRb.velocity;
                    Vector3 direction = velocity.normalized;

                    // Deactivate the solid pillar and activate the breakable pillar
                    pillar.SetActive(false);
                    breakablePillar.SetActive(true);

                    // Apply the force to each Rigidbody in the list
                    foreach (Rigidbody rb in breakablePillarRBs)
                    {
                        rb.AddForce(direction * explosionForce, ForceMode.Impulse);
                        StartCoroutine(ShrinkAndDestroy(rb.gameObject, shrinkDuration, destructionDelay));
                    }

                    OnIceBearPillarHit?.Invoke();
                }
            }
        }
    }

    private IEnumerator ShrinkAndDestroy(GameObject obj, float shrinkTime, float delay)
    {
        Vector3 originalScale = obj.transform.localScale;
        float elapsedTime = 0f;

        // Wait for the delay before starting the shrink effect
        yield return new WaitForSeconds(delay - shrinkTime);

        while (elapsedTime < shrinkTime)
        {
            float scale = Mathf.Lerp(1, 0, elapsedTime / shrinkTime);
            obj.transform.localScale = originalScale * scale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object is completely shrunk
        obj.transform.localScale = Vector3.zero;

        // Destroy the object
        Destroy(obj);
    }
}
