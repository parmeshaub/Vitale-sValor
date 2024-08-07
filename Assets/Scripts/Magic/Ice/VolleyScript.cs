using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Magic Move", menuName = "Magic Move/Volley")]
public class VolleyScript : MagicMoveSO
{
    public override void Activate() {
        PlayerCombat playerCombat = PlayerCombat.Instance;
        if (playerCombat == null) {
            Debug.LogError("PlayerCombat instance not found.");
            return;
        }
        playerCombat.animator.SetTrigger("Volley");
        playerCombat.isAttacking = true;
        playerCombat.MoveWhileAttack(1.5f, 0.5f);
        GameObject playerObject = playerCombat.gameObject;
        if (playerObject == null) {
            Debug.LogError("Player object not found.");
            return;
        }

        // Use a helper MonoBehaviour to start the coroutine
        MagicCoroutineHelper.Instance.StartCoroutine(ActivateMagic(playerObject, playerCombat));
    }

    public override void Cast() {
        throw new System.NotImplementedException();
    }

    private IEnumerator ActivateMagic(GameObject playerObject, PlayerCombat playerCombat) {
        yield return new WaitForSeconds(0.7f);

        // Calculate the position in front of the player with an offset
        Vector3 forwardOffset = playerObject.transform.forward * 2.4f;
        Vector3 verticalOffset = new Vector3(0, 0.1f, 0); // Add 0.1 to the Y axis
        Vector3 horizontalOffset = playerObject.transform.right * -0.8f; // Shift left by 0.5 units

        // Calculate the final spawn position with all offsets
        Vector3 spawnPosition = playerObject.transform.position + forwardOffset + verticalOffset + horizontalOffset;

        // Create a rotation offset (45 degrees to the right)
        Quaternion rotationOffset = Quaternion.Euler(0, 40, 0);
        Quaternion spawnRotation = playerObject.transform.rotation * rotationOffset;

        // Instantiate the skillPrefab at the calculated position with the adjusted rotation
        Instantiate(skillPrefab, spawnPosition, spawnRotation);
        playerCombat.isAttacking = false;

    }
}
