using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour
{
    private List<GameObject> interactablesInRange = new List<GameObject>();
    private PlayerInteract playerInteract;
    private float duration = 0.6f;

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

            StartCoroutine(DecreaseAlpha(playerInteract.currentInteractable));

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

        // Getting alpha of prompt to animate
        Debug.Log(closestInteractable);
        StartCoroutine(IncreaseAlpha(closestInteractable));
        return closestInteractable;
    }


    private IEnumerator IncreaseAlpha(GameObject closestInteractable)
    {
        // Finding UI
        Transform actual = closestInteractable.transform.Find("ArtemisPillar");
        Transform UICanvas = actual.transform.Find("Canvas");
        Canvas canvas = UICanvas.GetComponent<Canvas>();
        CanvasGroup alphaSlider = canvas.GetComponent<CanvasGroup>();

        Renderer renderer = actual.GetComponent<Renderer>();
        Material artemisMat = renderer.material;

        float elapsedTime = 0f;
        Color interactColor = new Color(Mathf.Clamp01(0.3962264f), Mathf.Clamp01(0.3962264f), Mathf.Clamp01(0.3962264f));
        Color currentColor = artemisMat.GetColor("_FresnalEffect");

        while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(0.000000001f);
            elapsedTime += Time.deltaTime;

            alphaSlider.alpha = Mathf.Lerp(alphaSlider.alpha, 1f, elapsedTime / duration);
            Color currentFresnalColor = Color.Lerp(currentColor, interactColor, elapsedTime / duration);
            artemisMat.SetColor("_FresnalEffect", currentFresnalColor);

            yield return null; // Wait for the next frame
        }
    }

    private IEnumerator DecreaseAlpha(GameObject closestInteractable)
    {
        // Finding UI
        Transform actual = closestInteractable.transform.Find("ArtemisPillar");
        Transform UICanvas = actual.transform.Find("Canvas");
        Canvas canvas = UICanvas.GetComponent<Canvas>();
        CanvasGroup alphaSlider = canvas.GetComponent<CanvasGroup>();

        Renderer renderer = actual.GetComponent<Renderer>();
        Material artemisMat = renderer.material;

        float elapsedTime = 0f;
        Color originalColor = new Color(Mathf.Clamp01(0), Mathf.Clamp01(0), Mathf.Clamp01(0));
        Color currentColor = artemisMat.GetColor("_FresnalEffect");

        while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(0.000000001f);
            elapsedTime += Time.deltaTime;
            alphaSlider.alpha = Mathf.Lerp(alphaSlider.alpha, 0f, elapsedTime / duration);
            Color currentFresnalColor = Color.Lerp(currentColor, originalColor, elapsedTime / duration);
            artemisMat.SetColor("_FresnalEffect", currentFresnalColor);

            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set to the endValue
        alphaSlider.alpha = 0f;
    }  
}
