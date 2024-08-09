using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Magic Move", menuName = "Magic Move/Judgement")]
public class JudgementScript : MagicMoveSO
{
    private MagicIndicator magicIndicator;
    public override void Activate()
    {
        magicIndicator = FindObjectOfType<MagicIndicator>();
        Transform castTransform = magicIndicator.gameObject.transform;
        Instantiate(skillPrefab, castTransform.position, castTransform.rotation);

    }
    public override void Cast() {
        throw new System.NotImplementedException();
    }
}
