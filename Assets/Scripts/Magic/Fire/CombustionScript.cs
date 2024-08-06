using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Magic Move", menuName = "Magic Move/Combustion")]
public class CombustionScript : MagicMoveSO
{
    public override void Activate()
    {
        Debug.Log("test");
    }
    public override void Cast() {
        throw new System.NotImplementedException();
    }
}
