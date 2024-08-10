using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ArtemisPillar : MonoBehaviour
{
    public Animator artemisAnimator;
    public void ActivateArtemis()
    {
        artemisAnimator.SetTrigger("activated");
    }
 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ActivateArtemis();
        }
    }
}
