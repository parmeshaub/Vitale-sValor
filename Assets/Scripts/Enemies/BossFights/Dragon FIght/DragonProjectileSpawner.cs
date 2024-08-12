using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonProjectileSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> projectiles;
    [SerializeField] private float minTime = 10f; // Minimum time before activating a projectile
    [SerializeField] private float maxTime = 25f; // Maximum time before activating a projectile
    private GameObject dragonObject;
    private DragonBoss dragonBoss;

    private void Start() {
        dragonObject = GameObject.FindGameObjectWithTag("Enemy");
        dragonBoss = dragonObject.GetComponent<DragonBoss>();
        // Initially, deactivate all projectiles
        foreach (var projectile in projectiles) {
            projectile.SetActive(false);
        }

        // Start the coroutine to activate projectiles randomly
        StartCoroutine(ActivateRandomProjectile());
    }

    private IEnumerator ActivateRandomProjectile() {
        while (projectiles.Count > 0) {
            // Wait for a random time between minTime and maxTime
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);

            // Remove any destroyed projectiles from the list
            projectiles.RemoveAll(projectile => projectile == null);

            // If there are still projectiles available
            if (projectiles.Count > 0) {
                // Choose a random projectile from the list
                int randomIndex = Random.Range(0, projectiles.Count);
                GameObject selectedProjectile = projectiles[randomIndex];

                // Activate the selected projectile
                selectedProjectile.SetActive(true);
                if (dragonBoss.isDead) yield break;
            }
        }
    }
}
