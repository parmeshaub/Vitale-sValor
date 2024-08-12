using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour
{
    private List<GameObject> interactablesInRange = new List<GameObject>();
    private PlayerInteract playerInteract;
    private float duration = 2f;

    private void Awake() {
        playerInteract = GetComponentInParent<PlayerInteract>();
    }

    private void OnTriggerEnter(Collider other) {
        interactablesInRange.Add(other.gameObject);
        playerInteract.currentInteractable = FindClosestInteractable();
    }

    private void OnTriggerExit(Collider other) {
        interactablesInRange.Remove(other.gameObject);
        if (interactablesInRange.Count <= 0) {
            playerInteract.currentInteractable = null;
        }
    }

    public GameObject FindClosestInteractable() {
        interactablesInRange.RemoveAll(item => item == null); // Remove destroyed objects

        if (interactablesInRange.Count == 0) {
            return null; // No interactables in range
        }

        GameObject closestInteractable = null;
        float closestDistance = Mathf.Infinity;

        Vector3 currentPosition = transform.position;

        foreach (GameObject interactable in interactablesInRange) {
            float distance = Vector3.Distance(currentPosition, interactable.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }


        Debug.Log(closestInteractable);

        // Getting alpha of prompt to animate
        Transform UICanvas = closestInteractable.transform.Find("Canvas");
        CanvasGroup alphaSlider = UICanvas.GetComponent<CanvasGroup>();
        StartCoroutine(IncreaseAlpha(alphaSlider, alphaSlider.alpha));


        return closestInteractable;
    }





    private IEnumerator IncreaseAlpha(CanvasGroup alphaSlider, float currentAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(0.000000001f);
            elapsedTime += Time.deltaTime;
            alphaSlider.alpha = Mathf.Lerp(currentAlpha, 1f, elapsedTime / duration);
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set to the endValue
        alphaSlider.alpha = 1f;
    }

    private IEnumerator DecreaseAlpha(CanvasGroup alphaSlider, float currentAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(0.000000001f);
            elapsedTime += Time.deltaTime;
            alphaSlider.alpha = Mathf.Lerp(currentAlpha, 0f, elapsedTime / duration);
            // Optionally, you can do something with currentValue here
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set to the endValue
        alphaSlider.alpha = 0f;
    }


}
