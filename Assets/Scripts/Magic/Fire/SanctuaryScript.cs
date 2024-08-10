using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Magic Move", menuName = "Magic Move/Sanctuary")]
public class SanctuaryScript : MagicMoveSO
{
    private PlayerCombat playerCombat;
    public override void Activate()
    {
        playerCombat = PlayerCombat.Instance;
        GameObject playerObj = playerCombat.gameObject;

        Instantiate(skillPrefab, playerObj.transform);
    }
    public override void Cast() {
        throw new System.NotImplementedException();
    }
}
