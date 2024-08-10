using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SanctuaryFunctionality : MonoBehaviour
{
    [SerializeField] private VisualEffect particle;
    private PlayerCombat playerCombat;

    void Start()
    {
        particle.Stop();
        StartHealing();
    }

    private void StartHealing() {
        StartCoroutine(SanctuaryTimer());
    }

    private IEnumerator SanctuaryTimer() {
        playerCombat = GetComponentInParent<PlayerCombat>();
        if (playerCombat == null) {
            Debug.LogError("PlayerCombat component not found on parent objects.");
            yield break; // Exit the coroutine early to avoid further errors
        }

        if (playerCombat.playerHealth == null) {
            Debug.LogError("playerHealth is not assigned in PlayerCombat.");
            yield break; // Exit the coroutine early to avoid further errors
        }

        particle.Play();
        playerCombat.playerHealth.CanHeal = true;
        Debug.Log("Healing Start");
        yield return new WaitForSeconds(5);
        playerCombat.playerHealth.CanHeal = false;
        Debug.Log("Healing End");
        particle.Stop();
    }


}
