using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Magic Move", menuName = "Magic Move/Ablaze")]
public class AblazeScript : MagicMoveSO
{
    public override void Activate()
    {
        Debug.Log("test");
    }

    public override void Cast() {
        throw new System.NotImplementedException();
    }
}
