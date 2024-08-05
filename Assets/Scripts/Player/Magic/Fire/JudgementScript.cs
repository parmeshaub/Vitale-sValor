using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Magic Move", menuName = "Magic Move/Judgement")]
public class JudgementScript : MagicMoveSO
{
    public override void Activate()
    {
        Debug.Log("test");
    }
}
