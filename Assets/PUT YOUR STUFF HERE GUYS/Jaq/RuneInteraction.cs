using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneInteraction : Interactable
{
    public Animator animator;
    public bool isUnlocked = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Interact()
    {
        isUnlocked = true; 
    }
}
