using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Magic Move", menuName = "Magic Move/Wings Of Comfort")]
public class WingsOfComfortScript : MagicMoveSO
{
    private PlayerCombat playerCombat;
    private PlayerController playerController;
    public override void Activate() {
        playerCombat = PlayerCombat.Instance;
        playerController = playerCombat.GetComponent<PlayerController>();

        if (playerController != null) {
            playerController.StartGlide(); // Start gliding when this magic move is activated
        }
    }
    public override void Cast() {
        throw new System.NotImplementedException();
    }
}
