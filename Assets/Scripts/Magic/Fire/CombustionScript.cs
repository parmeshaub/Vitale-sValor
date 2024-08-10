using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "New Magic Move", menuName = "Magic Move/Combustion")]
public class CombustionScript : MagicMoveSO
{
    public override void Activate()
    {
        PlayerCombat playerCombat = PlayerCombat.Instance;
        PlayerController playerController = playerCombat.gameObject.GetComponent<PlayerController>();
        GameObject combustionObject = playerController.combustionObject;
        GameObject playerVisual = playerController.playerVisual;
        ParticleSystem wings = playerController.wingsObject;
        VisualEffect effect = playerController.combustionEffect;

        MagicCoroutineHelper.Instance.StartCoroutine(StartCombustion(playerVisual, combustionObject, playerController, wings, effect));

    }
    public override void Cast() {
        throw new System.NotImplementedException();
    }

    private IEnumerator StartCombustion(GameObject playerVisual, GameObject combustObject, PlayerController playerController, ParticleSystem wings, VisualEffect effect) {
        float originalSpeed = playerController.moveSpeed;
        float originalSpint = playerController.sprintMultiplier;
        playerController.moveSpeed = 14f;
        playerController.sprintMultiplier = 1;
        playerVisual.SetActive(false);
        effect.Play();

        yield return new WaitForSeconds(5);

        playerVisual.SetActive(true);
        wings.Stop();
        effect.Stop();
        playerController.moveSpeed = originalSpeed;
        playerController.sprintMultiplier = originalSpint;
    }
}
