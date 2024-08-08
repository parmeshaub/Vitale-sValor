using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Magic Move", menuName = "Magic Move/Judgement")]
public class JudgementScript : MagicMoveSO
{
    public override void Activate()
    {
        Debug.Log("Judgement test");
    }
    public override void Cast() {
        throw new System.NotImplementedException();
    }
}
