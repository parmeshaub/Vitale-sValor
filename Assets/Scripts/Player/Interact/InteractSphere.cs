using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour
{
    private List<GameObject> interactablesInRange = new List<GameObject>();
    private PlayerInteract playerInteract;
    private void Awake()
    {
        playerInteract = GetComponentInParent<PlayerInteract>();
    }
    private void OnTriggerEnter(Collider other)
    {
        interactablesInRange.Add(other.gameObject);
        playerInteract.currentInteractable = FindClosestInteractable();
    }

    private void OnTriggerExit(Collider other)
    {
        interactablesInRange.Remove(other.gameObject);
        if(interactablesInRange.Count <= 0)
        {
            playerInteract.currentInteractable = null;
        }
    }

    public GameObject FindClosestInteractable()
    {
        if (interactablesInRange.Count == 0)
        {
            return null; // No interactables in range
        }

        GameObject closestInteractable = null;
        float closestDistance = Mathf.Infinity;

        Vector3 currentPosition = transform.position;

        foreach (GameObject interactable in interactablesInRange)
        {
            float distance = Vector3.Distance(currentPosition, interactable.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }
        Debug.Log(closestInteractable);
        return closestInteractable;
    }
}
