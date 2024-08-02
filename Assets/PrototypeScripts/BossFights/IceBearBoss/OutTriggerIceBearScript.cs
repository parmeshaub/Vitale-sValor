using System;
using UnityEngine;
using UnityEngine.Rendering;

public class OutTriggerIceBearScript : MonoBehaviour
{
    public delegate void StopIceBearChargeEdge();
    public static event StopIceBearChargeEdge OnStopIceBearChargeEdge;

    private Collider triggerCollider;

    private void Start()
    {
        triggerCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            OnStopIceBearChargeEdge?.Invoke();
            //triggerCollider.enabled = false; // Disable the trigger
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            OnStopIceBearChargeEdge?.Invoke();
        }
    }
}
